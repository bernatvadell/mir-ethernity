using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mir.Client.Models;
using Mir.Client.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls
{
    public enum TextBoxInputType
    {
        Text = 0,
        Password = 1,
    }

    public class TextBoxControl : BaseControl
    {
        [Observable]
        private bool _visibleTextCursor = false;

        private TimeController _visibleTextCursorController;
        private IContentAccess _contentAccess;
        private Vector2 _cursorPosition = Vector2.Zero;
        private ushort _cursorCharPosition = 0;
        private Vector2 _charPasswordLength = Vector2.Zero;

        [Observable]
        public TextBoxInputType Type { get; set; }
        [Observable]
        public Color FontColor { get; set; }
        [Observable]
        public string Text { get; set; }
        [Observable]
        public FontType FontType { get; set; }

        public SpriteFont Font { get => _contentAccess.Fonts[FontType]; }

        public TextBoxControl(IDrawerManager drawerManager, IRenderTargetManager renderTargetManager, IContentAccess contentAccess) : base(drawerManager, renderTargetManager)
        {
            _visibleTextCursorController = new TimeController(TimeSpan.FromSeconds(0.5));
            _contentAccess = contentAccess;

            IsControl = true;
            Drawable = true;
            Type = TextBoxInputType.Text;
            BackgroundColor = Color.Black;
            FontColor = Color.White;
            Width = 200;
            Height = 18;
            Text = string.Empty;
            FontType = FontType.Normal;

            LostFocus += TextBoxControl_LostFocus;
        }

        protected override Vector2 GetComponentSize()
        {
            return new Vector2(Width ?? 0, Height ?? 0);
        }

        private void TextBoxControl_LostFocus(object sender, EventArgs e)
        {
            _visibleTextCursor = false;
        }

        protected override bool CheckTextureValid()
        {
            return !StateChanged(nameof(Type), nameof(FontColor), nameof(Text), nameof(FontType), nameof(_visibleTextCursor));
        }

        protected override void UpdateState(GameTime gameTime)
        {
            if (!CheckTextureValid())
            {
                CalculateCursorPosition();
                CalculateCharLength();
            }

            if (Focused && _visibleTextCursorController.CheckProcess(gameTime))
                _visibleTextCursor = !_visibleTextCursor;
        }

        protected override void DrawTexture()
        {
            using (var ctx = DrawerManager.PrepareSpriteBatch())
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    var drawText = Type == TextBoxInputType.Text ? Text : new string('*', Text.Length);
                    ctx.Instance.DrawString(Font, drawText, new Vector2(2, 1), FontColor);
                }

                if (_visibleTextCursor) ctx.Instance.DrawLine(new Vector2(_cursorPosition.X + 3, 2), new Vector2(_cursorPosition.X + 3, InnerHeight - 2), Color.White, 1);
            }
        }

        public override bool OnTextInput(Keys key, char character)
        {
            if (base.OnTextInput(key, character))
                return true;

            if (((byte)character >= 32 && (byte)character <= 126) || ((byte)character >= 161 && (byte)character <= 191))
            {
                Text = Text.Insert(_cursorCharPosition++, character.ToString());
                _visibleTextCursor = true;
                CalculateCursorPosition();
                return true;
            }

            return false;
        }

        public override bool OnKeyDown(KeyEventArgs args)
        {
            var currentText = Text;
            var currentCharPos = _cursorCharPosition;

            if (args.PressedKeys.Length != 1)
                return base.OnKeyUp(args);


            switch (args.PressedKeys[0])
            {
                case Keys.Left:
                    if (_cursorCharPosition > 0) _cursorCharPosition--;
                    break;
                case Keys.Right:
                    if (_cursorCharPosition < Text.Length) _cursorCharPosition++;
                    break;
                case Keys.Delete:
                    if (_cursorCharPosition < Text.Length) Text = Text.Remove(_cursorCharPosition, 1);
                    break;
                case Keys.Back:
                    if (_cursorCharPosition > 0) Text = Text.Remove(_cursorCharPosition-- - 1, 1);
                    break;
            }

            if (currentText != Text || currentCharPos != _cursorCharPosition)
            {
                _visibleTextCursor = true;
                CalculateCursorPosition();
                return true;
            }

            return base.OnKeyDown(args);
        }

        private void CalculateCursorPosition()
        {
            if (string.IsNullOrEmpty(Text))
            {
                _cursorPosition = new Vector2(0, 0);
            }
            else if (Type == TextBoxInputType.Password)
            {
                _cursorPosition = new Vector2(_charPasswordLength.X * _cursorCharPosition, _charPasswordLength.Y);
            }
            else
            {
                var str = Text.Substring(0, _cursorCharPosition);
                _cursorPosition = Font.MeasureString(str);
            }
            InvalidateTexture();
        }

        private void CalculateCharLength()
        {
            if (Type == TextBoxInputType.Password)
            {
                _charPasswordLength = Font.MeasureString("*");
            }
            InvalidateTexture();
        }
    }
}
