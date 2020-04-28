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
        private Type _assetLoaderType;

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

        public GameBuilder UseAssetLoader<TAssetLoader>() where TAssetLoader : IAssetLoader
        {
            _assetLoaderType = typeof(TAssetLoader);
            return this;
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
            _containerBuilder.RegisterType(_assetLoaderType ?? throw new ServiceNotSpecifiedException(nameof(IAssetLoader))).As<IAssetLoader>().SingleInstance();

            _containerBuilder.RegisterType(_imageLibraryType).As<IImageLibrary>().SingleInstance();
            _containerBuilder.RegisterType(_mapReaderType).As<IMapReader>().SingleInstance();

            _containerBuilder.RegisterType<DrawerManager>().As<IDrawerManager>().SingleInstance();
            _containerBuilder.RegisterType<RenderTargetManager>().As<IRenderTargetManager>().SingleInstance();
            _containerBuilder.RegisterType<SceneManager>().As<ISceneManager>().SingleInstance();
            _containerBuilder.RegisterType<LibraryResolver>().As<ILibraryResolver>().SingleInstance();

            _containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(BaseScene)));

            _containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(BaseControl)));

            _containerBuilder.RegisterType<GameWindow>().As<Game>().SingleInstance();
            _containerBuilder.Register((component) => component.Resolve<Game>().GraphicsDevice).As<GraphicsDevice>().SingleInstance();
            _containerBuilder.Register((component) => component.Resolve<Game>().Content).As<ContentManager>().SingleInstance();
            _containerBuilder.Register((component) => new GraphicsDeviceManager(component.Resolve<Game>())).As<GraphicsDeviceManager>().SingleInstance();
            _containerBuilder.Register((component) => new SpriteBatch(component.Resolve<GraphicsDevice>())).As<SpriteBatch>().InstancePerDependency();

            var container = _containerBuilder.Build();

            var game = container.Resolve<Game>();
            var device = container.Resolve<GraphicsDeviceManager>();

            device.PreferredBackBufferWidth = 1024;
            device.PreferredBackBufferHeight = 768;

            return game;
        }


        public static GameBuilder Create()
        {
            return new GameBuilder();
        }
    }
}

