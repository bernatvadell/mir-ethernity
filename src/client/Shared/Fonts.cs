using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client
{
    public class Fonts
    {
        public static Fonts Instance = new Fonts();

        public SpriteFont Size8 { get; set; }

        public void Load(ContentManager content)
        {
            Size8 = content.Load<SpriteFont>("fonts/normal");
        }
    }
}
