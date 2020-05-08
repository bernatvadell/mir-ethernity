using Autofac;
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
        private ILibraryResolver _libraryResolver;


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

        public MirImageBrush()
        {
            _libraryResolver = GameBuilder.Container.Resolve<ILibraryResolver>();
        }

        public Point Size => Image == null ? Point.Zero : new Point(Image.Width, Image.Height);
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
                    _texture = Image != null ? DrawerManager.Instance.GenerateTexture(Image) : null;
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

            batch.Draw(_texture, dest, SourceRectangle, color);

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
            var image = _libraryResolver.Resolve(Library)?[Index]?[Type];
            if (image == Image) return;

            _validTexture = false;
            Image = image;

            ImageChanged?.Invoke(this, image);
        }
    }
}
