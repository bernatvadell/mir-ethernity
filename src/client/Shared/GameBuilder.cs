using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client
{
    public class GameBuilder
    {
        private ContainerBuilder _containerBuilder;

        private GameBuilder()
        {
            _containerBuilder = new ContainerBuilder();
            RegisterDefaultServices();
        }

        private void RegisterDefaultServices()
        {
            _containerBuilder.RegisterType<GameWindow>().As<Game>().SingleInstance();
        }

        public static GameBuilder Create()
        {
            return new GameBuilder();
        }

        public Game Build()
        {
            var container = _containerBuilder.Build();
            return container.Resolve<Game>();
        }
    }
}
