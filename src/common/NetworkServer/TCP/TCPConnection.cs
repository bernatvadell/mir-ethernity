using Microsoft.Extensions.Logging;
using Mir.Packets;
using Mir.Packets.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.Network.TCP
{
	public class TCPConnection : IConnection
	{
		private readonly SemaphoreSlim _waiterDisconnect = new SemaphoreSlim(0);
		private readonly SemaphoreSlim _waiterSend = new SemaphoreSlim(0);
		private readonly SemaphoreSlim _waiterData = new SemaphoreSlim(0);
		private readonly Socket _socket;
		private readonly ILogger<TCPConnection> _logger;
		private byte[] _buffer = new byte[1024 * 16];
		private byte[] _extra = new byte[0];
		private CancellationToken _cancellationToken;
		private PacketSource _source;
		private bool _isListener;

		public event EventHandler<Packet> OnReceivePacket;
		public event EventHandler OnDisconnect;

		public bool Connected { get; private set; } = true;

		public int Handle => (int)_socket.Handle;

		public TCPConnection(Socket socket, ILogger<TCPConnection> logger = null)
		{
			_logger = logger;
			_socket = socket ?? throw new ArgumentNullException(nameof(socket));
		}

		internal async void StartListenData(CancellationToken cancellationToken, PacketSource source, bool isListener)
		{
			_cancellationToken = cancellationToken;
			_source = source;
			_isListener = isListener;

			while (Connected && !cancellationToken.IsCancellationRequested)
			{
				if (!_socket.IsBound)
				{
					break;
				}

				_socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), null);

				try
				{
					if (_isListener)
					{
						if (!await _waiterData.WaitAsync(TimeSpan.FromSeconds(60), cancellationToken))
						{
							await Send(Ping.Default);

							if (!await _waiterData.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken))
								await Disconnect();
						}
					}
					else
					{
						await _waiterData.WaitAsync(Timeout.Infinite, cancellationToken);
					}
				}
				catch (OperationCanceledException)
				{
					break;
				}
			}
		}

		public async Task Send(Packet packet)
		{
			if (_waiterSend.CurrentCount > 0)
				await _waiterSend.WaitAsync(_cancellationToken);

			if (!Connected || !_socket.IsBound)
				return;

			using (var ms = new MemoryStream())
			{
				ProtoBuf.Serializer.Serialize(ms, packet);
				var buffer = ms.ToArray();
				_socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
			}
			await _waiterSend.WaitAsync();
		}

		public async Task Disconnect()
		{
			if (!Connected) return;

			Connected = false;

			if (_socket.IsBound)
			{
				_socket.BeginDisconnect(false, new AsyncCallback(DisconnectCallback), null);
				await _waiterDisconnect.WaitAsync();
			}

			_logger?.LogInformation($"[{_socket.Handle}] Client disconnected");

			if (_waiterData.CurrentCount > 0)
				_waiterData.Release();

			OnDisconnect?.Invoke(this, EventArgs.Empty);
		}

		private void DisconnectCallback(IAsyncResult ar)
		{
			_socket.EndDisconnect(ar);
			_waiterDisconnect.Release();
		}

		private async void ReadCallback(IAsyncResult ar)
		{
			if (_cancellationToken.IsCancellationRequested)
				return;

			int length;

			try
			{
				length = _socket.EndReceive(ar);
			}
			catch
			{
				await Disconnect();
				return;
			}


			if (length == 0)
			{
				await Disconnect();
				return;
			}

			var data = new byte[length + _extra.Length];
			Array.Copy(_extra, 0, data, 0, _extra.Length);
			Array.Copy(_buffer, 0, data, _extra.Length, length);

			try
			{
				Packet packet;

				using (var ms = new MemoryStream(data))
				{
					packet = ProtoBuf.Serializer.Deserialize<Packet>(ms);
					_extra = new byte[ms.Length - ms.Position];
					ms.Read(_extra, 0, _extra.Length);
				}

				switch (packet)
				{
					case Ping p:
						await Send(Pong.Default);
						break;
					case Pong p:
						break;
					default:
						_logger?.LogDebug($"[Socket] {_socket.Handle} - Receive data: {Convert.ToBase64String(data)}");
						OnReceivePacket?.Invoke(this, packet);
						break;
				}
			}
			catch (EndOfStreamException)
			{
				_extra = data;
			}
			catch (Exception)
			{
				await Disconnect();
			}

			_waiterData.Release();
		}

		private void SendCallback(IAsyncResult ar)
		{
			if (_cancellationToken.IsCancellationRequested)
				return;

			_socket.EndSend(ar);

			_waiterSend.Release();
		}
	}
}
