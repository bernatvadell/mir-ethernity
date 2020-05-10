using Mir.Client.Services;
using Mir.Client.Services.Default;
using Mir.Ethernity.ImageLibrary.Zircon;
using Mir.Ethernity.MapLibrary.Wemade;
using System;

namespace Mir.Client
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new GameContext(new TextureGenerator()))
                game.Run();
        }

    }
}
