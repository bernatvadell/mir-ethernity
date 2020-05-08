using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Models;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Mir.Client.MyraCustom
{
    public class MirWindowHeader : Panel
    {
        private readonly Label _label;
        private readonly MirImageBrush _background;
        private readonly MirImageBrush _topBar;
        private readonly MirImageBrush _cornerTopLeft;
        private readonly MirImageBrush _cornerTopRight;

        public string Title { get => _label.Text; set => _label.Text = value; }

        public MirWindowHeader()
        {
            Margin = Thickness.Zero;
            Padding = Thickness.Zero;

            _background = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 3,
            };

            _topBar = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 0,
            };

            _cornerTopLeft = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 11,
            };

            _cornerTopRight = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 12,
            };

            Widgets.Add(_label = new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom
            });

            Height = 28;
        }

        public override void InternalRender(RenderContext batch)
        {
            _topBar.SourceRectangle = new Rectangle(0, 0, Bounds.Width, _topBar.Size.Y);
            batch.Draw(_topBar, new Rectangle(Bounds.Location.X, Bounds.Location.Y, Bounds.Width, _topBar.Size.Y), Color.White);

            _background.SourceRectangle = new Rectangle(0, 0, Bounds.Width - 6, _background.Size.Y);
            batch.Draw(_background, new Rectangle(Bounds.Location.X + 3, Bounds.Location.Y + _topBar.Size.Y, Bounds.Width - 6, _background.Size.Y), Color.White);

            batch.Draw(_cornerTopLeft, Bounds.Location, Color.White);
            batch.Draw(_cornerTopRight, new Point(Bounds.Location.X + Bounds.Width - _cornerTopRight.Size.X, Bounds.Location.Y), Color.White);

            base.InternalRender(batch);
        }
    }

    public class MirWindowContent : SingleItemContainer<Widget>
    {
        private readonly MirImageBrush _barHorizontal;
        private readonly MirImageBrush _contentCornerTopLeft;
        private readonly MirImageBrush _contentCornerTopRight;
        private readonly MirImageBrush _contentCornerBottomLeft;
        private readonly MirImageBrush _contentCornerBottomRight;


        private bool _hasTitle = false;
        public bool HasTitle
        {
            get => _hasTitle;
            set
            {
                if (_hasTitle == value) return;
                _hasTitle = value;
                OnHasTitleChanged();
            }
        }

        private bool _hasFooter = false;
        public bool HasFooter
        {
            get => _hasFooter;
            set
            {
                if (_hasFooter == value) return;
                _hasFooter = value;
                OnHasFooterChanged();
            }
        }

        public Widget Content { get => InternalChild; set => InternalChild = value; }

        public MirWindowContent()
        {
            Margin = Thickness.Zero;
            Padding = new Thickness(8);
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            _barHorizontal = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 2,
            };

            _contentCornerTopLeft = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 25,
            };

            _contentCornerTopRight = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 26,
            };

            _contentCornerBottomLeft = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 8,
            };

            _contentCornerBottomRight = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 9,
            };
        }

        public override void InternalRender(RenderContext batch)
        {
            _barHorizontal.SourceRectangle = new Rectangle(0, 0, Bounds.Width, _barHorizontal.Size.Y);

            batch.Draw(_barHorizontal, new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, _barHorizontal.Size.Y), Color.White);
            batch.Draw(_barHorizontal, new Rectangle(Bounds.X, Bounds.Y + Bounds.Height - _barHorizontal.Size.Y, Bounds.Width, _barHorizontal.Size.Y), Color.White);

            batch.Draw(_contentCornerTopLeft, Bounds.Location, Color.White);
            batch.Draw(_contentCornerTopRight, new Point(Bounds.Location.X + Bounds.Width - _contentCornerTopRight.Size.X, Bounds.Location.Y), Color.White);

            batch.Draw(_contentCornerBottomLeft, new Point(Bounds.Location.X, Bounds.Location.Y + Bounds.Height - _contentCornerBottomLeft.Size.Y), Color.White);
            batch.Draw(_contentCornerBottomRight, new Point(Bounds.Location.X + Bounds.Width - _contentCornerBottomRight.Size.X, Bounds.Location.Y + Bounds.Height - _contentCornerBottomRight.Size.Y), Color.White);

            base.InternalRender(batch);
        }

        private void OnHasTitleChanged()
        {
            if (HasTitle)
            {
                _contentCornerTopLeft.Index = 4;
                _contentCornerTopRight.Index = 5;
            }
            else
            {
                _contentCornerTopLeft.Index = 8;
                _contentCornerTopRight.Index = 9;
            }
        }

        private void OnHasFooterChanged()
        {
            if (HasFooter)
            {
                _contentCornerBottomLeft.Index = 6;
                _contentCornerBottomRight.Index = 7;
            }
            else
            {
                _contentCornerBottomLeft.Index = 25;
                _contentCornerBottomRight.Index = 26;
            }
        }
    }

    public class MirWindowFooter : SingleItemContainer<Widget>
    {
        private readonly MirImageBrush _background;
        private readonly MirImageBrush _cornerBottomLeft;
        private readonly MirImageBrush _cornerBottomRight;
        private readonly MirImageBrush _footerBar;

        public Widget Content { get => InternalChild; set => InternalChild = value; }

        public MirWindowFooter()
        {
            Margin = Thickness.Zero;
            Padding = new Thickness(5);
            HorizontalAlignment = HorizontalAlignment.Stretch;
            Height = 42;

            _background = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 10,
            };

            _cornerBottomLeft = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 13,
            };

            _cornerBottomRight = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 14,
            };

            _footerBar = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 0,
            };
        }

        public override void InternalRender(RenderContext batch)
        {
            // _barHorizontal.SourceRectangle = new Rectangle(0, 0, Bounds.Width, _barHorizontal.Size.Y);
            // batch.Draw(_barHorizontal, new Rectangle(Bounds.X, Bounds.Y + 1, Bounds.Width, _barHorizontal.Size.Y), Color.White);

            _background.SourceRectangle = new Rectangle(0, 0, Bounds.Width - 6, _background.Size.Y);
            batch.Draw(_background, new Rectangle(Bounds.Location.X + 3, Bounds.Location.Y, Bounds.Width - 6, _background.Size.Y), Color.White);

            _footerBar.SourceRectangle = new Rectangle(0, 0, Bounds.Width, _footerBar.Size.Y);
            batch.Draw(_footerBar, new Rectangle(Bounds.Location.X, Bounds.Location.Y + Bounds.Height - _footerBar.Size.Y, Bounds.Width, _footerBar.Size.Y), Color.White);

            batch.Draw(
                _cornerBottomLeft,
                new Rectangle(
                    Bounds.Location.X,
                    Bounds.Location.Y + Bounds.Height - _cornerBottomLeft.Size.Y,
                    _cornerBottomLeft.Size.X,
                    _cornerBottomLeft.Size.Y
                ),
                Color.White
            );

            batch.Draw(
                _cornerBottomRight,
                new Rectangle(
                    Bounds.Location.X + Bounds.Width - _cornerBottomRight.Size.X,
                    Bounds.Location.Y + Bounds.Height - _cornerBottomRight.Size.Y,
                    _cornerBottomRight.Size.X,
                    _cornerBottomRight.Size.Y
                ),
                Color.White
            );

            base.InternalRender(batch);
        }
    }

    public class MirWindow : Window
    {
        private readonly MirWindowContent _content;
        private readonly MirWindowFooter _footer;
        private readonly MirWindowHeader _title;
        private readonly MirImageBrush _barVertical;

        public bool HasTitle => !string.IsNullOrEmpty(Title);
        public bool HasFooter => _footer.Content != null;

        public Widget InnerContent { get => _content.Content; set => _content.Content = value; }
        public Widget FooterContent { get => _footer.Content; set => _footer.Content = value; }

        public MirWindow()
        {
            Padding = Thickness.Zero;
            Border = null;
            BorderThickness = Thickness.Zero;
            InternalChild.Spacing = 0;
            Background = new SolidBrush(Color.Black);

            MinWidth = 240;
            MinHeight = 100;

            Border = null;

            _title = new MirWindowHeader();
            _content = new MirWindowContent();
            _footer = new MirWindowFooter();

            _barVertical = new MirImageBrush
            {
                Library = LibraryType.Interface,
                Index = 1,
            };

            TitleGrid.ColumnsProportions.Clear();
            TitleGrid.Margin = Thickness.Zero;
            TitleGrid.Padding = Thickness.Zero;
            TitleGrid.Widgets.Clear();
            TitleGrid.Widgets.Add(_title);
            Content = _content;
            InternalChild.Widgets.Add(_footer);
        }

        public override void InternalRender(RenderContext batch)
        {
            batch.Draw(_barVertical, new Rectangle(Bounds.X, ActualBounds.Y, _barVertical.Size.X, Bounds.Height), Color.White);
            batch.Draw(_barVertical, new Rectangle(Bounds.X + Bounds.Width - _barVertical.Size.X, Bounds.Y, _barVertical.Size.X, Bounds.Height), Color.White);

            if (HasTitle)
            {
                _title.Title = Title;
                _title.Visible = true;
                _content.HasTitle = true;
            }
            else
            {
                _title.Visible = false;
                _content.HasTitle = false;
            }

            if (HasFooter)
            {
                _footer.Visible = true;
                _content.HasFooter = true;
            }
            else
            {
                _footer.Visible = false;
                _content.HasFooter = false;
            }

            base.InternalRender(batch);
        }

        protected override void InternalSetStyle(Stylesheet stylesheet, string name)
        {
        }

        public static MirWindow ShowDialog(string title, string body)
        {
            var lines = body.Split('\n');
            var content = new VerticalStackPanel();
            foreach (var line in lines) content.Widgets.Add(new Label { Text = line, HorizontalAlignment = HorizontalAlignment.Center });

            var footer = new HorizontalStackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var cancelButton = new MirButton { Text = "Cancel" };
            footer.Widgets.Add(cancelButton);

            var window = new MirWindow
            {
                Title = title,
                InnerContent = content,
                FooterContent = footer
            };

            cancelButton.Click += (s, e) => window.Close();

            window.ShowModal();

            return window;
        }
    }
}
