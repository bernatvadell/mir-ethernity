using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Controls;
using Mir.Client.Exceptions;
using Mir.Client.Scenes;
using Mir.Client.Services;
using Mir.Client.Services.Default;
using Mir.Ethernity.ImageLibrary;
using Mir.Ethernity.ImageLibrary.Zircon;
using Mir.Ethernity.MapLibrary;
using Mir.Ethernity.MapLibrary.Wemade;
using System;
using System.IO;

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
            var assembly = typeof(BaseScene).Assembly;

            _containerBuilder.RegisterType(_textureGeneratorType ?? throw new ServiceNotSpecifiedException(nameof(ITextureGenerator))).As<ITextureGenerator>().SingleInstance();
            _containerBuilder.RegisterType(_imageLibraryType).As<IImageLibrary>().SingleInstance();
            _containerBuilder.RegisterType(_mapReaderType).As<IMapReader>().SingleInstance();

            _containerBuilder.RegisterType<SceneManager>().As<ISceneManager>().SingleInstance();

            // _containerBuilder.RegisterType<GameScene>();

            _containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(BaseScene)));

            _containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(BaseControl)));

            _containerBuilder.RegisterType<GameWindow>().As<Game>().SingleInstance();
            _containerBuilder.Register((component) => component.Resolve<Game>().GraphicsDevice).As<GraphicsDevice>().SingleInstance();
            _containerBuilder.Register((component) => component.Resolve<Game>().Content).As<ContentManager>().SingleInstance();
            _containerBuilder.Register((component) => new SpriteBatch(component.Resolve<GraphicsDevice>())).As<SpriteBatch>().SingleInstance();
            
            _containerBuilder.RegisterType<GraphicsDeviceManager>().SingleInstance();

            var container = _containerBuilder.Build();

            return container.Resolve<Game>();
        }


        public static GameBuilder Create()
        {
            return new GameBuilder();
        }
    }
}

