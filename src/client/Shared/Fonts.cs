using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client
{
    public static class Fonts
    {
        public static SpriteFont Size8 { get; set; }

        public static void Load(ContentManager content)
        {
            Size8 = content.Load<SpriteFont>("fonts/normal");
        }
    }
}
