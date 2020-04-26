using Autofac;
using Mir.Client.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class SceneManager : ISceneManager
    {
        private readonly ILifetimeScope _container;

        public BaseScene Active { get; private set; }

        public SceneManager(ILifetimeScope container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public TScene Load<TScene>() where TScene : BaseScene
        {
            var scene = _container.Resolve<TScene>();
            Active = scene;
            return scene;
        }
    }
}
