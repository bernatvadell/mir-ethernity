using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class RenderTargetManager : IRenderTargetManager
    {
        public GraphicsDevice Device { get; private set; }

        public Context<RenderTarget2D> ActiveContext { get; private set; }

        public RenderTargetManager(GraphicsDevice device)
        {
            Device = device;
        }

        public Context<RenderTarget2D> SetRenderTarget2D(RenderTarget2D target)
        {
            Device.SetRenderTarget(target);

            ActiveContext = new Context<RenderTarget2D>(target, DisposeContext, ActiveContext);

            return ActiveContext;
        }

        private void DisposeContext(Context<RenderTarget2D> context)
        {
            Device.SetRenderTarget(context.ParentContext?.Data);
            ActiveContext = context.ParentContext;
        }
    }
}
