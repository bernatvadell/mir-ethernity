using Mir.Client.Models;
using Mir.Client.MyraCustom;
using Mir.Models;
using Mir.Network.TCP;
using Mir.Packets;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mir.Client.Scenes.Login
{
	public class LoginScene : BaseScene
	{
		private MirWindow _window;
		private LoginBox _loginBox;
		private bool _connecting = false;
		private int _attemps = 0;
		private TimeController _reconnectController = new TimeController(TimeSpan.FromSeconds(3), runOnFirstCheck: true);

		public LoginScene()
		{
			Background = new MirImageBrush
			{
				Index = 20,
				Library = LibraryType.Interface1c
			};

			Widgets.Add(new MirImage
			{
				Id = "Sun",
				Library = LibraryType.Interface1c,
				UseOffset = true,
				Blend = true
			}.WithAnimation((s, e) => ((MirImage)s).Index = e, 2300, 2329, TimeSpan.FromSeconds(5), true));

			Widgets.Add(new MirImage
			{
				Id = "Birds",
				Library = LibraryType.Interface1c,
				UseOffset = true
			}.WithAnimation((s, e) => ((MirImage)s).Index = e, 2200, 2288, TimeSpan.FromSeconds(10), true));

			Widgets.Add(new MirImage
			{
				Id = "Flags",
				Library = LibraryType.Interface1c,
				UseOffset = true
			}.WithAnimation((s, e) => ((MirImage)s).Index = e, 2400, 2429, TimeSpan.FromSeconds(4), true));

			Widgets.Add(new MirImage
			{
				Id = "Water",
				Library = LibraryType.Interface1c,
				UseOffset = true
			}.WithAnimation((s, e) => ((MirImage)s).Index = e, 2500, 2529, TimeSpan.FromSeconds(4), true));


			Widgets.Add(new MirImage
			{
				Id = "LogoShadow",
				Library = LibraryType.Interface1c,
				Index = 23,
				HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center,
				Top = 100
			});

			Widgets.Add(new MirImage
			{
				Id = "Logo",
				Blend = true,
				Library = LibraryType.Interface1c,
				Index = 22,
				HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center,
				Top = 66
			});

			Widgets.Add(new MirImage
			{
				Id = "PegiA",
				Library = LibraryType.Interface1c,
				Index = 3,
				HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right,
			});


			Widgets.Add(new MirImage
			{
				Id = "PegiB",
				Library = LibraryType.Interface1c,
				Index = 4,
				Top = 100,
				HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right,
			});

			Widgets.Add(_loginBox = new LoginBox());
		}

		public override void Load()
		{
			Envir.Network.PrepareConnection();

			_loginBox.Visible = false;
			_window = MirWindow.ShowDialog("Loading", $"Trying connect to server\nAttemps: {_attemps}");
		}

		public override void Update()
		{
			if (!_connecting && !Envir.Network.Client.Connected && _reconnectController.CheckProcess())
				TryConnect();
		}

		private async void TryConnect()
		{
			_connecting = true;
			_attemps++;

			(_window.InnerContent as Label).Text = $"Trying connect to server\nAttemps: {_attemps}";

			try
			{
				await Envir.Network.Client.Connect();

				_window.Visible = false;
				_loginBox.Visible = true;
			}
			catch (Exception)
			{
				_connecting = false;
				_reconnectController.Reset();
			}
		}
	}
}
