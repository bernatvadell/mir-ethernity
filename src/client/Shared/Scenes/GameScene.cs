using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes
{
    public sealed class GameScene : BaseScene
    {
        private LabelControl _label;

        public GameScene(ILifetimeScope container) : base(container)
        {
            _label = CreateControl<LabelControl>();
            _label.ScreenX = 10;
            _label.ScreenY = 10;
            _label.Color = Color.Red;
            _label.Text = "Write your text here...";
        }
    }
}
