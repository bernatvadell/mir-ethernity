using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Models
{
    public class Context<TDataType> : IDisposable
    {
        private readonly Action<Context<TDataType>> _disposeCallback;

        public Context<TDataType> ParentContext { get; private set; }
        public TDataType Instance { get; private set; }

        internal Context(TDataType instance, Action<Context<TDataType>> disposeCallack, Context<TDataType> parentContext = default)
        {
            Instance = instance;
            ParentContext = parentContext;
            _disposeCallback = disposeCallack;
        }

        public void Dispose()
        {
            _disposeCallback?.Invoke(this);
        }
    }
}
