using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Controls.Animators;
using Mir.Client.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls
{
    public class TextureControl : BaseControl
    {
        [Observable]
        public Texture2D Texture { get; set; }

        public TextureControl(ILifetimeScope scope) : base(scope)
        {
            Drawable = true;
        }

        protected override bool CheckTextureValid()
        {
            return !StateChanged(nameof(Texture));
        }

        protected override void DrawTexture()
        {
            if (Texture == null) return;

            using (var ctx = DrawerManager.PrepareSpriteBatch())
            {
                ctx.Instance.Draw(Texture, Vector2.Zero, Color.White);
            }
        }

        protected override Vector2 GetComponentSize()
        {
            if (Texture == null) return Vector2.Zero;
            return new Vector2(Texture.Width, Texture.Height);
        }

       
    }
}
