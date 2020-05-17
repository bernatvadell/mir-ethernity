using Microsoft.Extensions.Logging;
using Mir.GameServer.Models;
using Mir.GameServer.Services.LoopTasks;
using Mir.GameServer.Services.PacketProcessor;
using Mir.Network;
using Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.Default
{

    public class GameService : IService
    {
        private readonly ILogger<GameService> _logger;
        private readonly Tuple<int, ILoopTask[]>[] _loopTasks;
        private readonly GameState _state;
        private readonly IListener _listener;
        private readonly PacketProcessExecutor _packetProcessExecutor;
        private readonly DbConnection _db;
        private bool _executedMigrations = false;

        public ConcurrentDictionary<int, GateConnection> Gates { get; } = new ConcurrentDictionary<int, GateConnection>();

        public GameService(
            IListener listener,
            ILogger<GameService> logger,
            GameState state,
            PacketProcessExecutor packetProcessExecutor,
            IEnumerable<ILoopTask> loopTasks,
            DbConnection dbConnection
        )
        {
            _logger = logger;
            _loopTasks = loopTasks?
                .GroupBy(x => x.Order)
                .OrderBy(x => x.Key)
                .Select(x => new Tuple<int, ILoopTask[]>(x.Key, x.ToArray()))
                .ToArray() ?? throw new ArgumentNullException(nameof(LoopTasks));

            _state = state ?? throw new ArgumentNullException(nameof(state));
            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
            _packetProcessExecutor = packetProcessExecutor ?? throw new ArgumentNullException(nameof(packetProcessExecutor));
            _db = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

            _listener.OnClientConnect += Listener_OnClientConnect;
            _listener.OnClientData += Listener_OnClientData;
            _listener.OnClientDisconnect += Listener_OnClientDisconnect;
        }

        private void Listener_OnClientData(object sender, Message e)
        {
            _state.IncommingPackets.Enqueue(e);
        }

        private async void Listener_OnClientDisconnect(object sender, IConnection e)
        {
            if (_state.Gates.TryRemove(e.Handle, out GateConnection gate))
                await gate.Disconnect();
        }

        private async void Listener_OnClientConnect(object sender, IConnection e)
        {
            var gate = new GateConnection(e);
            if (!_state.Gates.TryAdd(e.Handle, gate))
                await e.Disconnect();
        }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            await Task.WhenAny(
                Update(cancellationToken),
                _listener.Listen(cancellationToken)
            );
        }

        private async Task Update(CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();

            sw.Start();

            var fpsExpected = 120;
            var msPerUpdate = 1000 / fpsExpected;

            while (!cancellationToken.IsCancellationRequested)
            {
                _state.GameTime = sw.Elapsed;

                if (_db.State != ConnectionState.Open)
                    await TryConnectToDB(cancellationToken);


                for (var i = 0; i < _loopTasks.Length; i++)
                {
                    var group = _loopTasks[i];
                    var tasks = new Task[group.Item2.Length];
                    for (var t = 0; t < tasks.Length; t++)
                        tasks[t] = group.Item2[t].Update(_state);
                    await Task.WhenAll(tasks);
                }

                var elapsedMiliseconds = (sw.Elapsed - _state.GameTime).TotalMilliseconds;

                if (msPerUpdate > elapsedMiliseconds)
                    await Task.Delay(TimeSpan.FromMilliseconds(msPerUpdate - elapsedMiliseconds));
            }

            sw.Stop();
        }

        private async Task TryConnectToDB(CancellationToken cancellationToken)
        {
            const int RETRY_CONNECTION_SECONDS = 3;
            try
            {
                if (!_executedMigrations) RunMigrations();

                _logger.LogInformation("Connecting to database...");
                await _db.OpenAsync(cancellationToken);
                _logger.LogInformation("Connected to database");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred connecting to database, retry in {0} seconds", RETRY_CONNECTION_SECONDS);
                await Task.Delay(TimeSpan.FromSeconds(RETRY_CONNECTION_SECONDS));
                await TryConnectToDB(cancellationToken);
            }
        }

        private void RunMigrations()
        {
            _logger.LogInformation("Running migrations...");
            // Migrator.Execute(IoCBuilder.PostgreSQLConnectionString);
            _logger.LogInformation("Migrations updated");
            _executedMigrations = true;
        }
    }
}
