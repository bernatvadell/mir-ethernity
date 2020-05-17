﻿using Mir.GameServer.Models;
using Mir.Network;
using Mir.Packets.Client;
using Mir.Packets.Gate;
using Mir.Packets.Server;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.PacketProcessor
{
    public class LoginPacketProcess : PacketProcess<Login>
    {
        private IAccountRepository _accountRepository;
        
        public override Stage Stage => Stage.Login;

        public LoginPacketProcess(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        protected override async Task ProcessPacket(ClientState client, Login packet)
        {
            var account = await _accountRepository.FindByUsername(packet.Username);

            if (account == null || !BCrypt.Net.BCrypt.Verify(packet.Password, account.Password))
            {
                await client.Send(new LoginResult { Result = LoginResultEnum.BadUsernameOrPassword });
            }
            else
            {
                await client.Send(new LoginResult { Result = LoginResultEnum.Succcess });
            }
        }
    }
}
