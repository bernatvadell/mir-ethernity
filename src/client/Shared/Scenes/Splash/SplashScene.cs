using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Models;
using Mir.Client.MyraCustom;
using Mir.Client.Scenes.Login;
using Mir.Client.Services;
using Mir.Models;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes.Splash
{
    public class SplashScene : BaseScene
    {
        private TimeController _splashTimeController = new TimeController(TimeSpan.FromSeconds(3));

        public SplashScene()
        {
            Widgets.Add(new MirImage
            {
                Index = 6,
                Library = LibraryType.Interface1c
            }.WithAnimation((s, e) => s.Opacity = (100 - e) / 100f, 0, 100, TimeSpan.FromSeconds(2), false));

var labelDragHandle = new Label() { Text = "Drag handle" };
var draggableWidget = new VerticalStackPanel
{
    Border = new SolidBrush(Color.Red),
    BorderThickness = new Thickness(1),
    Left = 100,
    Top = 100,
    Width = 250,
    Height = 250,

    IsDraggable = true,
    DragHandle = labelDragHandle,
    DragDirection = DragDirection.Horizontal,

    Widgets = {
        labelDragHandle,
        new Label() { Text = "This is body content" }
    }
};

            Widgets.Add(draggableWidget);
        }

        public override void Update()
        {
            if (_splashTimeController.CheckProcess())
                SceneManager.Instance.Load(new LoginScene());

            base.Update();
        }
    }
}
