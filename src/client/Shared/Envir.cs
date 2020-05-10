using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client
{
	public class Envir
	{
		public static GameTime Time { get; set; } = new GameTime(TimeSpan.Zero, TimeSpan.Zero);
		public static GameContext Game { get; set; }
		public static GraphicsDeviceManager Device { get; set; }
		public static ITextureGenerator TextureGenerator { get; set; }
	}
}
