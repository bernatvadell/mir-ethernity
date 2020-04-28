using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls
{
    public class LabelControl : BaseControl
    {
        [Observable]
        public SpriteFont Font { get; set; }
        [Observable]
        public string Text { get; set; }
        [Observable]
        public Color Color { get; set; }

        public LabelControl(ContentManager content, IDrawerManager drawerManager, IRenderTargetManager renderTargetManager) : base(drawerManager, renderTargetManager)
        {
            Font = content.Load<SpriteFont>("fonts/normal");
        }

        protected override bool CheckTextureValid()
        {
            return !StateChanged(nameof(Text), nameof(Color), nameof(Font));
        }

        protected override void DrawTexture()
        {
            using (var ctx = DrawerManager.PrepareSpriteBatch())
            {
                ctx.Data.DrawString(Font, Text, new Vector2(0, 0), Color);
            }
        }

        protected override Vector2 GetComponentSize()
        {
            return Font.MeasureString(Text);
        }
    }
}
