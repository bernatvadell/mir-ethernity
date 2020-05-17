using Mir.Client.MyraCustom;
using Mir.Client.Scenes.Characters;
using Mir.Packets.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Network.Processors
{
    public class LoginResultProcessor : BaseProcessor<LoginResult>
    {
        public override void Process(LoginResult packet)
        {
            switch (packet.Result)
            {
                case LoginResultEnum.BadUsernameOrPassword:
                    MirWindow.ShowDialog("Login", "Your username or password are not correct");
                    break;
                case LoginResultEnum.Succcess:
                    SceneManager.Load(new CharacterScene());
                    break;
            }
        }
    }
}
