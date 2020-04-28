using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services
{
    public interface IContentAccess
    {
        IDictionary<FontType, SpriteFont> Fonts { get; }
        Texture2D BlackBackground { get; }

        void LoadContent();
    }
}
