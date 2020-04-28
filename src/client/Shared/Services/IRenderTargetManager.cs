using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services
{
    public interface IRenderTargetManager
    {
        Context<RenderTarget2D> SetRenderTarget2D(RenderTarget2D target);
        RenderTarget2D CreateRenderTarget2D(int width, int height);
    }
}
