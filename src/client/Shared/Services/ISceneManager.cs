using Mir.Client.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services
{
    public interface ISceneManager
    {
        BaseScene Active { get; }
        TScene Load<TScene>() where TScene : BaseScene;
    }
}
