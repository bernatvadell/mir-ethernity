using Mir.Client.Services;
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
               .Build();

            using (game) game.Run();
        }
    }
}
