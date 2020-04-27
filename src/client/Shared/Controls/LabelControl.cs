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
        private readonly IDrawerManager _drawerManager;

        public SpriteFont Font { get; set; }

        public string Text { get; set; }
        public Color Color { get; set; }

        public LabelControl(ContentManager content, IDrawerManager drawerManager)
        {
            Font = content.Load<SpriteFont>("fonts/normal");
            _drawerManager = drawerManager;
        }

        public override void Draw(GameTime gameTime)
        {
            using (var ctx = _drawerManager.BuildContext())
            {
                ctx.SpriteBatch.DrawString(Font, Text, new Vector2(ScreenX, ScreenY), Color);
            }
            base.Draw(gameTime);
        }
    }
}
