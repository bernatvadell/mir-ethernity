using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls
{
    public class LabelControl : BaseControl
    {
        private readonly SpriteBatch _spriteBatch;

        public SpriteFont Font { get; set; }

        public string Text { get; set; }
        public Color Color { get; set; }

        public LabelControl(ContentManager content, SpriteBatch spriteBatch)
        {
            Font = content.Load<SpriteFont>("fonts/normal");
            _spriteBatch = spriteBatch;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.DrawString(Font, Text, new Vector2(ScreenX, ScreenY), Color);
            base.Draw(gameTime);
        }
    }
}
