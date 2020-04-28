using Mir.Client.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services
{
    public interface ISceneManager
    {
        BaseScene Active { get; }
        void Load<TScene>(bool throwException = true) where TScene : BaseScene;
    }
}
