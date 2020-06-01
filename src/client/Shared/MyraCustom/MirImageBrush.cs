using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Services;
using Mir.Client.Services.Default;
using Mir.Ethernity.ImageLibrary;
using Mir.Models;
using Myra.Graphics2D;
using System;
using System.Collections.Generic;
using System.Text;

using IImage = Mir.Ethernity.ImageLibrary.IImage;

namespace Mir.Client.MyraCustom
{
    public class MirImageBrush : Myra.Graphics2D.IImage
    {
        private LibraryType _libraryType;
        private ImageType _type;
        private int _index;
        private bool _validTexture = false;
        private Texture2D _texture;
        private Point _size;
        private Point _textureSize;

        public event EventHandler<IImage> ImageChanged;
        public IImage Image { get; private set; }
        public bool UseOffset { get; set; }

        public LibraryType Library
        {
            get => _libraryType;
            set
            {
                if (_libraryType == value) return;
                _libraryType = value;
                RefreshImageData();
            }
        }

        public ImageType Type
        {
            get => _type;
            set
            {
                if (_type == value) return;
                _type = value;
                RefreshImageData();
            }
        }
        public int Index
        {
            get => _index;
            set
            {
                if (_index == value) return;
                _index = value;
                RefreshImageData();
            }
        }

        public Rectangle? SourceRectangle { get; set; }

        public Point Size => _size;
        public bool Blend { get; set; }


        public void Draw(SpriteBatch batch, Rectangle dest, Color color)
        {
            if (!_validTexture || _texture.IsDisposed)
            {
                _texture?.Dispose();
                _texture = null;
                _validTexture = false;

                if (Image != null)
                {
                    _texture = Image != null ? DrawerManager.GenerateTexture(Image) : null;
                    _validTexture = _texture != null;
                }
            }

            if (!_validTexture)
                return;

            if (Blend)
            {
                batch.End();
                batch.Begin(
                    sortMode: SpriteSortMode.Deferred,
                    rasterizerState: batch.GraphicsDevice.RasterizerState,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: null,
                    blendState: BlendState.Additive
                );
            }

            if (UseOffset) dest.Offset(Image.OffsetX, Image.OffsetY);

            batch.Draw(_texture, new Rectangle(dest.Location, _textureSize), SourceRectangle, color);

            if (Blend)
            {
                batch.End();
                batch.Begin(
                    sortMode: SpriteSortMode.Deferred,
                    rasterizerState: batch.GraphicsDevice.RasterizerState,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: null,
                    blendState: BlendState.AlphaBlend
                );
            }
        }

        private void RefreshImageData()
        {
            var image = LibraryResolver.Resolve(Library)?[Index]?[Type];
            if (image == Image) return;

            _validTexture = false;
            Image = image;

            _size = Image == null ? Point.Zero : new Point(Image.Width + (4 - Image.Width % 4) % 4, Image.Height + (4 - Image.Height % 4) % 4);
            _textureSize = new Point(_size.X + (4 - _size.X % 4) % 4, _size.Y + (4 - _size.Y % 4) % 4);

            ImageChanged?.Invoke(this, image);
        }
    }
}
