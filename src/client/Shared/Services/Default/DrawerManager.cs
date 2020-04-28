using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using Mir.Ethernity.ImageLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class DrawerManager : IDrawerManager
    {
        private readonly GraphicsDevice _device;
        private readonly GraphicsDeviceManager _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;
        private readonly ITextureGenerator _textureGenerator;

        public Context<SpriteBatch> ActiveContext { get; private set; }

        public int Width { get => _graphicsDevice.PreferredBackBufferWidth; }
        public int Height { get => _graphicsDevice.PreferredBackBufferHeight; }

        public DrawerManager(
            GraphicsDevice device,
            GraphicsDeviceManager graphicsDevice,
            SpriteBatch spriteBatch,
            ITextureGenerator textureGenerator
        )
        {
            _device = device;
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _textureGenerator = textureGenerator;
        }

        public Context<SpriteBatch> PrepareSpriteBatch()
        {
            if (ActiveContext != null)
            {
                _spriteBatch.End();
            }

            _spriteBatch.Begin(sortMode: SpriteSortMode.Texture);

            ActiveContext = new Context<SpriteBatch>(_spriteBatch, DisposeContext, ActiveContext);

            return ActiveContext;
        }

        public void Clear(Color color)
        {
            _device.Clear(ClearOptions.Target, color, 0, 0);
        }

        private void DisposeContext(Context<SpriteBatch> context)
        {
            _spriteBatch.End();
            ActiveContext = context.ParentContext;
        }

        public Texture2D GenerateTexture(IImage image)
        {
            var data = image.GetBuffer();
            return _textureGenerator.Generate(_device, image.Width, image.Height, image.DataType, data);
        }
    }
}
