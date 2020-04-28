using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class ContentAccess : IContentAccess
    {
        private ContentManager _contentManager;

        public IDictionary<FontType, SpriteFont> Fonts { get; private set; }

        public ContentAccess(ContentManager contentManager)
        {
            _contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
        }

        public void LoadContent()
        {
            Fonts = new Dictionary<FontType, SpriteFont>
            {
                { FontType.Normal, _contentManager.Load<SpriteFont>("fonts/normal") }
            };
        }
    }
}
