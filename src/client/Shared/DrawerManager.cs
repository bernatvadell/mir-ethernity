using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using Mir.Ethernity.ImageLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Mir.Client
{
    public static class DrawerManager
    {
     
        public static GraphicsDeviceManager Device { get => Envir.Device; }
        public static GraphicsDevice Graphics { get => Envir.Game.GraphicsDevice; }
        public static Context<SpriteBatch> ActiveContext { get; private set; }
        public static SpriteBatch Sprite { get; private set; }

        public static int Width { get => Device.PreferredBackBufferWidth; }
        public static int Height { get => Device.PreferredBackBufferHeight; }

        public static void Load()
        {
            Device.PreferredBackBufferWidth = 1024;
            Device.PreferredBackBufferHeight = 768;
            Sprite = new SpriteBatch(Graphics);
        }

    
        public static Context<SpriteBatch> PrepareSpriteBatch(BlendState blendState = null)
        {
            if (ActiveContext != null)
            {
                Sprite.End();
            }

            Sprite.Begin(blendState: blendState);

            ActiveContext = new Context<SpriteBatch>(Sprite, DisposeContext, ActiveContext);

            return ActiveContext;
        }

        public static void Clear(Color color)
        {
            Graphics.Clear(ClearOptions.Target, color, 0, 0);
        }

        private static void DisposeContext(Context<SpriteBatch> context)
        {
            Sprite.End();
            ActiveContext = context.ParentContext;
        }

        public static Texture2D GenerateTexture(IImage image)
        {
            var buffer = image.GetBuffer();

            if (image.Compression == CompressionType.Deflate)
            {
                using (var output = new MemoryStream())
                using (var gz = new DeflateStream(new MemoryStream(buffer), CompressionMode.Decompress))
                {
                    gz.CopyTo(output);
                    gz.Close();
                    buffer = output.ToArray();
                }
            }

            return Envir.TextureGenerator.Generate(Graphics, image.Width, image.Height, image.DataType, buffer);
        }

        public static Point GetResizedPoint(Point point)
        {
            // TODO: Pending implement scaling window
            return point;
        }
    }
}
