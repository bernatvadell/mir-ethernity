using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Mir.Client;

namespace Mir.Client
{
	[Activity(Label = "Android"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = true
		, LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.FullUser
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
	public class GameActivity : Microsoft.Xna.Framework.AndroidGameActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			using (var game = new GameContext())
			{
				SetContentView((View)game.Services.GetService(typeof(View)));
				game.Run();
			}
		}
	}
}

