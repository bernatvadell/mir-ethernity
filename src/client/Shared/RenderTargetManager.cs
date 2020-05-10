using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
	public static class RenderTargetManager
	{
		public static Context<RenderTarget2D> ActiveContext { get; private set; }

		public static Context<RenderTarget2D> SetRenderTarget2D(RenderTarget2D target)
		{
			DrawerManager.Graphics.SetRenderTarget(target);

			ActiveContext = new Context<RenderTarget2D>(target, DisposeContext, ActiveContext);

			return ActiveContext;
		}

		private static void DisposeContext(Context<RenderTarget2D> context)
		{
			DrawerManager.Graphics.SetRenderTarget(context.ParentContext?.Instance);
			ActiveContext = context.ParentContext;
		}

		public static RenderTarget2D CreateRenderTarget2D(int width, int height)
		{
			return new RenderTarget2D(
				DrawerManager.Graphics,
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
