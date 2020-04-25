using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Exceptions;
using Mir.Client.Services;
using Mir.Client.Services.Default;
using Mir.Ethernity.ImageLibrary;
using Mir.Ethernity.ImageLibrary.Zircon;
using Mir.Ethernity.MapLibrary;
using Mir.Ethernity.MapLibrary.Wemade;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client
{
    public class GameBuilder
    {
        private ContainerBuilder _containerBuilder;

        private Type _imageLibraryType;
        private Type _textureGeneratorType;
        private Type _mapReaderType;

        private GameBuilder()
        {
            _containerBuilder = new ContainerBuilder();
            SetDefaultServices();
        }

        private void SetDefaultServices()
        {
            _imageLibraryType = typeof(ZirconImageLibrary);
            _mapReaderType = typeof(WemadeMapReader);
        }

        public GameBuilder UseImageLibrary<TImageLibrary>() where TImageLibrary : IImageLibrary
        {
            _imageLibraryType = typeof(TImageLibrary);
            return this;
        }

        public GameBuilder UseTextureGenerator<TTextureGenerator>() where TTextureGenerator : ITextureGenerator
        {
            _textureGeneratorType = typeof(TTextureGenerator);
            return this;
        }

        public Game Build()
        {
            _containerBuilder.RegisterType(_textureGeneratorType ?? throw new ServiceNotSpecifiedException(nameof(ITextureGenerator))).As<ITextureGenerator>().SingleInstance();
            _containerBuilder.RegisterType(_imageLibraryType).As<IImageLibrary>().SingleInstance();
            _containerBuilder.RegisterType(_mapReaderType).As<IMapReader>().SingleInstance();
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
