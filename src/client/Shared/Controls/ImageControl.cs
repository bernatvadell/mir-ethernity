using Microsoft.Xna.Framework;
using Mir.Client.Services;
using Mir.Ethernity.ImageLibrary;
using Mir.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls
{
    public class ImageControl : BaseControl
    {

        private readonly ILibraryResolver _libraryResolver;
        private readonly ITextureGenerator _textureGenerator;

        [Observable]
        public LibraryType LibraryType { get; set; }

        [Observable]
        public ImageType ImageType { get; set; }

        [Observable]
        public int Index { get; set; }

        protected IImageLibrary Library { get; private set; }
        protected IImage Image { get; private set; }

        public ImageControl(
            IDrawerManager drawerManager, IRenderTargetManager renderTargetManager, 
            ILibraryResolver libraryResolver, ITextureGenerator textureGenerator
        ) : base(drawerManager, renderTargetManager)
        {
            _libraryResolver = libraryResolver ?? throw new ArgumentNullException(nameof(libraryResolver));
            _textureGenerator = textureGenerator ?? throw new ArgumentNullException(nameof(textureGenerator));

            ImageType = ImageType.Image;
            LibraryType = LibraryType.None;
            Index = 0;
        }

        protected override void UpdateState()
        {
            if (StateChanged(nameof(LibraryType)))
            {
                Library = _libraryResolver.Resolve(LibraryType);
            }

            if (StateChanged(nameof(ImageType), nameof(Index)))
            {
                Image = Library?[Index][ImageType];
            }
        }

        protected override bool CheckTextureValid()
        {
            return !StateChanged(nameof(LibraryType), nameof(ImageType), nameof(Index));
        }

        protected override void DrawTexture()
        {
            if (Image == null) return;

            using (var ctx = DrawerManager.PrepareSpriteBatch())
            {
                var texture = DrawerManager.GenerateTexture(Image);
                ctx.Data.Draw(texture, Vector2.Zero, Color.White);
            }
        }

        protected override Vector2 GetComponentSize()
        {
            return new Vector2(Image.Width, Image.Height);
        }
    }
}
