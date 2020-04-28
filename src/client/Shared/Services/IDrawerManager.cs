using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using Mir.Ethernity.ImageLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services
{
    public interface IDrawerManager
    {
        int Width { get; }
        int Height { get; }

        Context<SpriteBatch> PrepareSpriteBatch(BlendState blendState = null);
        void Clear(Color color);
        Texture2D GenerateTexture(IImage image);
    }
}
