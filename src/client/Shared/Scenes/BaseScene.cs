using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Controls;
using Mir.Client.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes
{
    public abstract class BaseScene : BaseControl
    {
        protected ISceneManager SceneManager { get; private set; }

        public BaseScene(ILifetimeScope scope) : base(scope)
        {
            SceneManager = scope.Resolve<ISceneManager>();
        }
    }
}
