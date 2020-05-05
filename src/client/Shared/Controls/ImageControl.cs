using Autofac;
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

        [Observable]
        public LibraryType Library { get; set; }

        [Observable]
        public ImageType ImageType { get; set; }

        [Observable]
        public int Index { get; set; }

        [Observable]
        public bool UseOffset { get; set; }

        protected IImageLibrary LibraryManager { get; private set; }
        protected IImage Image { get; private set; }

        public ImageControl(
           ILifetimeScope scope
        ) : base(scope)
        {
            _libraryResolver = scope.Resolve<ILibraryResolver>();

            Drawable = true;
            ImageType = ImageType.Image;
            Library = LibraryType.None;
            Index = 0;
        }

        protected override void UpdateState(GameTime gameTime)
        {
            if (StateChanged(nameof(Library)))
                LibraryManager = _libraryResolver.Resolve(Library);

            var imageChanged = StateChanged(nameof(ImageType), nameof(Index));
            if (imageChanged)
                Image = LibraryManager?[Index]?[ImageType];

            if (imageChanged || StateChanged(nameof(UseOffset)))
            {
                if (Image != null && UseOffset)
                {
                    OffsetX = Image.OffsetX;
                    OffsetY = Image.OffsetY;
                }
                else
                {
                    OffsetX = OffsetY = null;
                }
            }
        }

        protected override bool CheckTextureValid()
        {
            return !StateChanged(nameof(Library), nameof(ImageType), nameof(Index), nameof(UseOffset));
        }

        protected override void DrawTexture()
        {
            if (Image == null) return;

            using (var ctx = DrawerManager.PrepareSpriteBatch())
            {
                var texture = DrawerManager.GenerateTexture(Image);
                ctx.Instance.Draw(texture, Vector2.Zero, Color.White);
            }
        }

        protected override Vector2 GetComponentSize()
        {
            if (Image == null) return Vector2.Zero;
            return new Vector2(Image.Width, Image.Height);
        }
    }
}
