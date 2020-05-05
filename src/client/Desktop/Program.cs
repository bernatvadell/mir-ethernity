using Mir.Client.Services;
using Mir.Client.Services.Default;
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
            var game = GameBuilder.Create()
               .UseTextureGenerator<TextureGenerator>()
               .UseAssetLoader<AssetLoader>()
               .UseGamePadService<MouseGamePadService>()
               .UseGamePadService<KeyboardGamePadService>()
               .Build();

            using (game) game.Run();
        }
    }
}
