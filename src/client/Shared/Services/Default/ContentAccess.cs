using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class ContentAccess : IContentAccess
    {
        private GraphicsDevice _device;
        private GraphicsDeviceManager _graphics;
        private ContentManager _contentManager;

        public IDictionary<FontType, SpriteFont> Fonts { get; private set; }

        public Texture2D BlackBackground { get; private set; }

        public ContentAccess(ContentManager contentManager, GraphicsDeviceManager graphics, GraphicsDevice device)
        {
            _device = device;
            _graphics = graphics;
            _contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
        }

        public void LoadContent()
        {
            Fonts = new Dictionary<FontType, SpriteFont>
            {
                { FontType.Normal, _contentManager.Load<SpriteFont>("fonts/normal") }
            };

            BlackBackground = new Texture2D(_device, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, false, SurfaceFormat.Color);
            BlackBackground.SetData(Enumerable.Range(0, _graphics.PreferredBackBufferWidth * _graphics.PreferredBackBufferHeight).Select(x => Color.Black).ToArray());
        }
    }
}
