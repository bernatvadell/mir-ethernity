using Microsoft.Xna.Framework;
using Mir.Client.MyraCustom;
using Mir.Network.TCP;
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
		public static TCPNetworkClient Client { get; set; }

		public static void OnLostConnection(object sender, EventArgs e)
		{
			MirWindow.ShowDialog("You are disconnected", "Connection lost, you want to go login?");
		}
	}
}
