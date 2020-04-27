using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Models
{
    public class DrawerContext : IDisposable
    {
        private readonly Action<DrawerContext> _disposeCallback;

        public DrawerContext ParentContext { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        internal DrawerContext(SpriteBatch spriteBatch, Action<DrawerContext> disposeCallack, DrawerContext parentContext = null)
        {
            SpriteBatch = spriteBatch;
            ParentContext = parentContext;
            _disposeCallback = disposeCallack;
        }

        public void Dispose()
        {
            _disposeCallback?.Invoke(this);
        }
    }
}
