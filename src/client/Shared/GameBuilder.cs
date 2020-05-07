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
using Mir.Ethernity.MapLibrary;
using System;
using System.Collections.Generic;
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
        private Dictionary<Type, Type> _overridedControls = new Dictionary<Type, Type>();
        private Type _gameContext;
        private List<Type> _gamePadServices = new List<Type>();

        private GameBuilder()
        {
            _containerBuilder = new ContainerBuilder();
            SetDefaultServices();
        }

        private void SetDefaultServices()
        {;
        }

        public GameBuilder OverrideControl<TOriginal, TOverride>() where TOriginal : BaseControl where TOverride : TOriginal
        {
            _overridedControls.Add(typeof(TOriginal), typeof(TOverride));
            return this;
        }

        public GameBuilder UseAssetLoader<TAssetLoader>() where TAssetLoader : IAssetLoader
        {
            _assetLoaderType = typeof(TAssetLoader);
            return this;
        }

        public GameBuilder UseGamePadService<TGamePadService>() where TGamePadService : IGamePadService
        {
            _gamePadServices.Add(typeof(TGamePadService));
            return this;
        }

        public GameBuilder UseContext<TGameContext>() where TGameContext : GameContext
        {
            _gameContext = typeof(TGameContext);
            return this;
        }

        public GameBuilder UseImageLibrary<TImageLibrary>() where TImageLibrary : IImageLibrary
        {
            _imageLibraryType = typeof(TImageLibrary);
            return this;
        }

        public GameBuilder UseMapReader<TMapReader>() where TMapReader : IMapReader
        {
            _mapReaderType = typeof(TMapReader);
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
            _containerBuilder.RegisterType(_gameContext ?? throw new ServiceNotSpecifiedException(nameof(GameContext))).As<Game>().SingleInstance();

            _containerBuilder.RegisterType(_imageLibraryType).As<IImageLibrary>().InstancePerDependency();
            _containerBuilder.RegisterType(_mapReaderType).As<IMapReader>().SingleInstance();

            _containerBuilder.RegisterType<ContentAccess>().As<IContentAccess>().SingleInstance();
            _containerBuilder.RegisterType<DrawerManager>().As<IDrawerManager>().SingleInstance();
            _containerBuilder.RegisterType<RenderTargetManager>().As<IRenderTargetManager>().SingleInstance();
            _containerBuilder.RegisterType<SceneManager>().As<ISceneManager>().SingleInstance();
            _containerBuilder.RegisterType<LibraryResolver>().As<ILibraryResolver>().SingleInstance();

            _containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(BaseScene)));

            _containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(BaseControl)));

            foreach (var gps in _gamePadServices)
                _containerBuilder.RegisterType(gps).As<IGamePadService>().SingleInstance();

            foreach (var control in _overridedControls)
                _containerBuilder.RegisterType(control.Value).As(control.Key);

            _containerBuilder.Register((component) => component.Resolve<Game>().Window).As<GameWindow>().SingleInstance();
            _containerBuilder.Register((component) => component.Resolve<Game>().GraphicsDevice).As<GraphicsDevice>().SingleInstance();
            _containerBuilder.Register((component) => component.Resolve<Game>().Content).As<ContentManager>().SingleInstance();
            _containerBuilder.Register((component) => new GraphicsDeviceManager(component.Resolve<Game>())).As<GraphicsDeviceManager>().SingleInstance();
            _containerBuilder.Register((component) => new SpriteBatch(component.Resolve<GraphicsDevice>())).As<SpriteBatch>().InstancePerDependency();

            var container = _containerBuilder.Build();

            var game = container.Resolve<Game>();
            game.IsFixedTimeStep = Config.FPSCap;

            var device = container.Resolve<GraphicsDeviceManager>();
            device.SynchronizeWithVerticalRetrace = false;

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

