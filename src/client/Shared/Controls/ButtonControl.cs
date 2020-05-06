using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Services;
using Mir.Ethernity.ImageLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Mir.Models;

namespace Mir.Client.Controls
{
    public class ButtonControl : BaseControl
    {
        private readonly Texture2D _leftTexture;
        private readonly Texture2D _rightTexture;
        private readonly Texture2D _middleTexture;
        private readonly LabelControl _label;
        private Color _textureColor = Color.White;

        [Observable]
        public string Text { get => _label.Text; set => _label.Text = value; }

        public ButtonControl(ILifetimeScope scope, ILibraryResolver libraryResolver) : base(scope)
        {
            IsControl = true;
            Drawable = true;

            var library = libraryResolver.Resolve(LibraryType.Interface);

            var leftImage = library[16][ImageType.Image];
            var rightImage = library[17][ImageType.Image];
            var middleImage = library[18][ImageType.Image];

            _leftTexture = DrawerManager.GenerateTexture(leftImage);
            _rightTexture = DrawerManager.GenerateTexture(rightImage);
            _middleTexture = DrawerManager.GenerateTexture(middleImage);

            _label = CreateControl<LabelControl>((control) =>
            {
                control.VerticalCenter = true;
                control.HorizontalCenter = true;
            });
        }

        protected override void UpdateState(GameTime gameTime)
        {
            if (StateChanged(nameof(Touching), nameof(Hovered), nameof(Enabled)))
            {
                _textureColor = Enabled
                    ? new Color(0.2F, 0.2F, 0.2F)
                    : Touching || Hovered
                        ? new Color(1F, 1F, 1F)
                        : new Color(0.85F, 0.85F, 0.85F);
            }
        }

        protected override bool CheckTextureValid()
        {
            return !StateChanged(nameof(Touching), nameof(Hovered), nameof(Enabled), nameof(Text));
        }

        protected override void DrawTexture()
        {
            using (var ctx = DrawerManager.PrepareSpriteBatch())
            {
                ctx.Instance.Draw(_middleTexture, new Rectangle(0, 0, OuterWidth, OuterHeight), new Rectangle(0, 0, OuterWidth, OuterHeight), _textureColor);
                ctx.Instance.Draw(_leftTexture, new Vector2(0, 0), _textureColor);
                ctx.Instance.Draw(_rightTexture, new Vector2(OuterWidth - _rightTexture.Width, 0), _textureColor);
            }
        }

        internal override Vector2 GetComponentSize()
        {
            var t = _label.GetComponentSize();
            return new Vector2(t.X + _leftTexture.Width + _rightTexture.Width, _leftTexture.Height);
        }
    }
}
