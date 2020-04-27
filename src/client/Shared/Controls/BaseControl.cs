using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mir.Client.Controls
{
    public abstract partial class BaseControl
    {
        private IDictionary<string, PropertyInfo> _allProperties;
        private IDictionary<string, object> _previousState;
        private RenderTarget2D _renderTarget2D = null;

        protected IDrawerManager DrawerManager { get; private set; }
        protected bool ValidTexture { get; set; }

        [Observable]
        public int ScreenX { get; set; }
        [Observable]
        public int ScreenY { get; set; }
        [Observable]
        public int Width { get; set; }
        [Observable]
        public int Height { get; set; }
        [Observable]
        public int OuterWidth { get; protected set; }
        [Observable]
        public int OuterHeight { get; protected set; }
        [Observable]
        public Color BackgroundColor { get; set; }

        public ControlCollection Controls { get; private set; }
        public BaseControl Parent { get; private set; }

        public BaseControl(IDrawerManager drawerManager)
        {
            DrawerManager = drawerManager;

            Controls = new ControlCollection(this);

            _previousState = new Dictionary<string, object>();

            _allProperties = GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttribute<ObservableAttribute>() != null)
                .ToDictionary(x => x.Name, x => x);

            ValidTexture = true;
        }

        public void Update(GameTime gameTime)
        {
            UpdateState();
            UpdateOuterSize();

            ValidTexture = _renderTarget2D != null
                && !StateChanged(nameof(Width), nameof(Height), nameof(OuterWidth), nameof(OuterHeight))
                && CheckTextureValid();

            if (!ValidTexture)
                InvalidateTexture();

            UpdateCurrentState();

            for (var i = 0; i < Controls.Length; i++)
                Controls[i].Update(gameTime);
        }

        public void UpdateOuterSize()
        {
            var inner = GetInnerSize();
            OuterWidth = Width > 0 ? Width : (int)inner.X;
            OuterHeight = Height > 0 ? Height : (int)inner.Y;
        }

        public virtual void UpdateState() { }

        public void Draw(GameTime gameTime)
        {
            if (!ValidTexture)
            {
                _renderTarget2D?.Dispose();
                _renderTarget2D = null;

                if (OuterWidth > 0 && OuterHeight > 0)
                {
                    _renderTarget2D = new RenderTarget2D(DrawerManager.Device, OuterWidth, OuterHeight, false, SurfaceFormat.Color, DepthFormat.None);

                    DrawerManager.Device.Clear(ClearOptions.Target, BackgroundColor, 0, 0);

                    DrawTexture();

                    for (var i = 0; i < Controls.Length; i++)
                        Controls[i].Draw(gameTime);

                    ValidTexture = true;
                }
            }

            if (ValidTexture)
            {
                using (var ctx = DrawerManager.BuildContext())
                    ctx.SpriteBatch.Draw(_renderTarget2D, new Vector2(ScreenX, ScreenY), Color.White);
            }
        }

        protected virtual Vector2 GetInnerSize()
        {
            return new Vector2(Width, Height);
        }

        protected bool StateChanged(params string[] props)
        {
            if (props.Length == 0)
                props = _allProperties.Select(x => x.Key).ToArray();

            foreach (var prop in props)
            {
                if (!_allProperties.ContainsKey(prop)) throw new ApplicationException($"Property {prop} not exists or not observable");
                var oldValue = _previousState.ContainsKey(prop) ? _previousState[prop] : null;
                var newValue = _allProperties[prop].GetValue(this);
                if (oldValue != newValue) return true;
            }

            return false;
        }

        protected virtual void DrawTexture() { }

        protected virtual bool CheckTextureValid()
        {
            return true;
        }

        protected void InvalidateTexture()
        {
            ValidTexture = false;
            Parent?.InvalidateTexture();
        }

        private void UpdateCurrentState()
        {
            foreach (var prop in _allProperties)
            {
                var value = prop.Value.GetValue(this);
                if (!_previousState.ContainsKey(prop.Key))
                    _previousState.Add(prop.Key, value);
                else
                    _previousState[prop.Key] = value;
            }
        }
    }
}
