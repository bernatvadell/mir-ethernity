namespace Mir.GameServer.Models
{
    public class ClientState
	{
		public ClientState(GateConnection gateConnection, int socketHandle)
		{
			GateConnection = gateConnection;
			SocketHandle = socketHandle;
		}

		public GateConnection GateConnection { get; }
		public int SocketHandle { get; }
	}
}
