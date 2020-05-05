using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class RenderTargetManager : IRenderTargetManager
    {
        private GraphicsDevice _device;

        public Context<RenderTarget2D> ActiveContext { get; private set; }

        public RenderTargetManager(GraphicsDevice device)
        {
            _device = device;
        }

        public Context<RenderTarget2D> SetRenderTarget2D(RenderTarget2D target)
        {
            _device.SetRenderTarget(target);

            ActiveContext = new Context<RenderTarget2D>(target, DisposeContext, ActiveContext);

            return ActiveContext;
        }

        private void DisposeContext(Context<RenderTarget2D> context)
        {
            _device.SetRenderTarget(context.ParentContext?.Instance);
            ActiveContext = context.ParentContext;
        }

        public RenderTarget2D CreateRenderTarget2D(int width, int height)
        {
            return new RenderTarget2D(
                _device,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                1,
                RenderTargetUsage.PreserveContents
            );
        }
    }
}
