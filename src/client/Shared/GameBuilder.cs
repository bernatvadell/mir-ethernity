using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Exceptions;
using Mir.Client.Services;
using Mir.Client.Services.Default;
using Mir.Ethernity.ImageLibrary;
using Mir.Ethernity.ImageLibrary.Zircon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client
{
    public class GameBuilder
    {
        private ContainerBuilder _containerBuilder;

        private Type _imageLibrary;
        private Type _textureGenerator;

        private GameBuilder()
        {
            _containerBuilder = new ContainerBuilder();
            SetDefaultServices();
        }

        private void SetDefaultServices()
        {
            _imageLibrary = typeof(ZirconImageLibrary);
        }

        public GameBuilder UseImageLibrary<TImageLibrary>() where TImageLibrary : IImageLibrary
        {
            _imageLibrary = typeof(TImageLibrary);
            return this;
        }

        public GameBuilder UseTextureGenerator<TTextureGenerator>() where TTextureGenerator : ITextureGenerator
        {
            _textureGenerator = typeof(TTextureGenerator);
            return this;
        }

        public Game Build()
        {
            _containerBuilder.RegisterType(_textureGenerator ?? throw new ServiceNotSpecifiedException(nameof(ITextureGenerator))).As<ITextureGenerator>();
            _containerBuilder.RegisterType(_imageLibrary).As<IImageLibrary>();
            _containerBuilder.RegisterType<GameWindow>().As<Game>().SingleInstance();

            var container = _containerBuilder.Build();

            return container.Resolve<Game>();
        }


        public static GameBuilder Create()
        {
            return new GameBuilder();
        }
    }
}
