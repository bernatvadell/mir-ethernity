using Microsoft.Xna.Framework;
using Mir.Models;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.MyraCustom
{
    public class MirButton : ButtonBase<HorizontalStackPanel>
    {
        private readonly Label _label;
        private readonly MirImageBrush _background;
        private readonly MirImageBrush _left;
        private readonly MirImageBrush _right;

        private bool _beforeIsPressed = false;
        private Color _foregroundColor = Color.White;

        public string Text { get => _label.Text; set => _label.Text = value; }
        public Color ForegroundColor { get; set; }

        public MirButton()
        {
            InternalChild = new HorizontalStackPanel
            {
                VerticalAlignment = VerticalAlignment.Stretch
            };

            Height = 24;

            _background = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 18
            };

            _left = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 16,
            };

            _right = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 17,
            };

            InternalChild.Widgets.Add(_label = new Label
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Wrap = true,
                Margin = new Thickness(19, 0)
            });



            UpdateImageState();
        }

        public override void OnPressedChanged()
        {
            base.OnPressedChanged();
            UpdateImageState();
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            UpdateImageState();
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            UpdateImageState();
        }

        public override void InternalRender(RenderContext batch)
        {
            batch.Draw(_background, ActualBounds, _foregroundColor);
            batch.Draw(_left, ActualBounds.Location, _foregroundColor);
            batch.Draw(_right, new Point(ActualBounds.Location.X + ActualBounds.Width - _right.Size.X, ActualBounds.Location.Y), _foregroundColor);

            base.InternalRender(batch);
        }

        private void UpdateImageState()
        {
            if (_beforeIsPressed && !IsPressed)
            {
                _beforeIsPressed = false;
                Top--;
            }

            if (IsPressed)
            {
                ForegroundColor = new Color(1F, 1F, 1F);
                _beforeIsPressed = true;
                Top++;
            }
            else if (IsMouseInside)
            {
                ForegroundColor = new Color(1F, 1F, 1F);
            }
            else
            {
                ForegroundColor = new Color(0.85F, 0.85F, 0.85F);
            }
        }
    }
}
