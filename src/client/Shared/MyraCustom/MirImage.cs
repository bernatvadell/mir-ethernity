using Microsoft.Xna.Framework;
using Mir.Ethernity.ImageLibrary;
using Mir.Models;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Mir.Client.MyraCustom
{
    public class MirImage : MirWidget
    {
        private MirImageBrush _image;

        public LibraryType Library { get => _image.Library; set => _image.Library = value; }
        public ImageType Type { get => _image.Type; set => _image.Type = value; }
        public int Index { get => _image.Index; set => _image.Index = value; }
        public bool Blend { get => _image.Blend; set => _image.Blend = value; }
        public bool UseOffset { get; set; }
        public Color TintColor { get; set; } = Color.White;

        public MirImage()
        {
            _image = new MirImageBrush();
            _image.ImageChanged += ImageChanged;
        }

        private void ImageChanged(object sender, IImage e)
        {
            if (e == null)
            {
                Visible = false;
            }
            else
            {
                Visible = true;
                Width = e.Width;
                Height = e.Height;

                if (UseOffset)
                    Padding = new Myra.Graphics2D.Thickness(e.OffsetX, e.OffsetY, 0, 0);
            }
        }

        public override void InternalRender(RenderContext context)
        {
            base.InternalRender(context);
            context.Draw(_image, ActualBounds, TintColor * context.Opacity);
        }
    }
}
