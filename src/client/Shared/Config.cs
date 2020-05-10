using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client
{
	public static class Config
	{
		public static bool FPSCap { get; set; } = false;
		public static bool VSync { get; set; } = false;
		public static string ServerIP { get; set; } = "127.0.0.1";
		public static ushort Port { get; set; } = 7000;
	}
}
