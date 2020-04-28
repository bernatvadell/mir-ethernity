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
        private ImageControl _image;

        public GameScene(ILifetimeScope container) : base(container)
        {
            _image = CreateControl<ImageControl>();
            _image.Left = 10;
            _image.Bottom = 10;
            _image.LibraryType = Mir.Models.LibraryType.Interface1c;
            _image.Index = 2;


            _label = CreateControl<LabelControl>();
            _label.Left = 10;
            _label.Top = 10;
            _label.Color = Color.Red;
            _label.Text = "Write your text here...";

            _label = CreateControl<LabelControl>();
            _label.Right = 10;
            _label.Bottom = 10;
            _label.Color = Color.Red;
            _label.Text = "Write your text here...";
        }

        protected override bool CheckTextureValid()
        {
            return true;
        }

        protected override void DrawTexture() { }
    }
}
