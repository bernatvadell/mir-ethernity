using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Packets
{
    public enum PacketIndex : int
    {
        #region Common Packets (from 1 at 1000)
        Ping = 1,
        Pong = 2,
        Disconnect = 3,
        #endregion

        #region Gate Packets (from 1001 to 2000)
        ClientPacket = 1001,
        ClientConnectionChanged = 1002,
        #endregion

        #region Client Packets (from 2001 to 12000)
        Login = 2001,
        #endregion


        #region Server Packets (from 12001 to 22000)
        
        LoginResult = 12001

        #endregion
    }
}
