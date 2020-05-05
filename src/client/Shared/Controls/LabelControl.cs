using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using Mir.Client.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls
{
    public class LabelControl : BaseControl
    {
        private readonly IContentAccess _contentAccess;

        private SpriteFont _font;
        private Vector2 _componentSize;


        [Observable]
        public FontType FontType { get; set; }
        [Observable]
        public string Text { get; set; }
        [Observable]
        public Color Color { get; set; }

        public LabelControl(ILifetimeScope scope) : base(scope)
        {
            _contentAccess = scope.Resolve<IContentAccess>();
            Drawable = true;
            Text = string.Empty;
        }

        protected override void UpdateState(GameTime gameTime)
        {
            if (StateChanged(nameof(FontType)))
                _font = _contentAccess.Fonts[FontType];

            if (StateChanged(nameof(Text), nameof(Color), nameof(FontType)))
            {
                _componentSize = _font.MeasureString(Text);
            }
        }

        protected override bool CheckTextureValid()
        {
            return !StateChanged(nameof(Text), nameof(Color), nameof(FontType));
        }

        protected override void DrawTexture()
        {
            using (var ctx = DrawerManager.PrepareSpriteBatch())
            {
                ctx.Instance.DrawString(_font, Text, new Vector2(0, 0), Color);
            }
        }

        protected override Vector2 GetComponentSize()
        {
            return _componentSize;
        }
    }
}
