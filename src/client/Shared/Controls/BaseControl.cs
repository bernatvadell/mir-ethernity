using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Controls.Animators;
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
    public abstract partial class BaseControl : IDisposable
    {
        private IDictionary<string, PropertyInfo> _allProperties;
        private IDictionary<string, object> _previousState;
        private RenderTarget2D _renderTarget2D = null;
        private List<Animation> _animations = new List<Animation>();

        protected IDrawerManager DrawerManager { get; }
        protected IRenderTargetManager RenderTargetManager { get; }

        protected bool ValidTexture { get; set; }

        public string Id { get; set; }

        public bool IsDisposed { get; private set; }

        [Observable]
        public float Opacity { get; set; } = 1f;
        [Observable]
        public int? Left { get; set; }
        [Observable]
        public int? Top { get; set; }
        [Observable]
        public int? Right { get; set; }
        [Observable]
        public int? Bottom { get; set; }
        [Observable]
        public int? Width { get; set; }
        [Observable]
        public int? Height { get; set; }
        [Observable]
        public int? PaddingTop { get; set; }
        [Observable]
        public int? PaddingBottom { get; set; }
        [Observable]
        public int? PaddingLeft { get; set; }
        [Observable]
        public int? PaddingRight { get; set; }
        [Observable]
        public int? MarginTop { get; set; }
        [Observable]
        public int? MarginBottom { get; set; }
        [Observable]
        public int? MarginLeft { get; set; }
        [Observable]
        public int? MarginRight { get; set; }
        [Observable]
        public Color BackgroundColor { get; set; }

        public ControlCollection Controls { get; private set; }
        public BaseControl Parent { get; private set; }

        public int InnerWidth { get; private set; }
        public int OuterWidth { get; private set; }

        public int InnerHeight { get; private set; }
        public int OuterHeight { get; private set; }

        public int ScreenX { get; private set; }
        public int ScreenY { get; private set; }

        public BaseControl(IDrawerManager drawerManager, IRenderTargetManager renderTargetManager)
        {
            DrawerManager = drawerManager;
            RenderTargetManager = renderTargetManager;

            BackgroundColor = Color.Transparent;

            Controls = new ControlCollection(this);

            _previousState = new Dictionary<string, object>();

            _allProperties = GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttribute<ObservableAttribute>() != null)
                .ToDictionary(x => x.Name, x => x);

            ValidTexture = true;
        }

        #region Public Methods
        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < _animations.Count; i++)
                _animations[i].Process(gameTime);

            UpdateState(gameTime);

            UpdateSizes();

            UpdatePositions();

            ValidTexture = _renderTarget2D != null
                && CheckTextureValid();

            if (StateChanged(
                    nameof(Width), nameof(Height),
                    nameof(PaddingBottom), nameof(PaddingTop), nameof(PaddingLeft), nameof(PaddingRight),
                    nameof(BackgroundColor), nameof(Opacity)
                ))
            {
                Parent?.InvalidateTexture();
            }

            if (!ValidTexture)
                InvalidateTexture();

            UpdateCurrentState();

            for (var i = 0; i < Controls.Length; i++)
                Controls[i].Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            if (!ValidTexture || _renderTarget2D == null || _renderTarget2D.IsDisposed)
            {
                _renderTarget2D?.Dispose();
                _renderTarget2D = null;

                if (OuterWidth > 0 && OuterHeight > 0)
                {
                    _renderTarget2D = RenderTargetManager.CreateRenderTarget2D(OuterWidth, OuterHeight);

                    using (RenderTargetManager.SetRenderTarget2D(_renderTarget2D))
                    {
                        DrawerManager.Clear(BackgroundColor);

                        DrawTexture();

                        for (var i = 0; i < Controls.Length; i++)
                            Controls[i].Draw(gameTime);
                    }

                    ValidTexture = true;
                }
            }

            if (ValidTexture)
            {
                var useOpacity = Opacity >= 0 && Opacity < 1;
                using (var ctx = DrawerManager.PrepareSpriteBatch())
                    ctx.Data.Draw(_renderTarget2D, new Vector2(ScreenX, ScreenY), useOpacity ? Color.White * (float)Opacity : Color.White);
            }
        }

        public void AddAnimator(Animation animation)
        {
            animation.AttachToControl(this);
            _animations.Add(animation);
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            _renderTarget2D?.Dispose();
            _allProperties?.Clear();
            _animations?.Clear();
            _previousState?.Clear();

            _renderTarget2D = null;
            _allProperties = null;
            _animations = null;
            _previousState = null;

            Controls.Dispose();

            IsDisposed = true;
        }
        #endregion

        #region Protected methods
        protected bool StateChanged(params string[] props)
        {
            if (props.Length == 0)
                props = _allProperties.Select(x => x.Key).ToArray();

            foreach (var prop in props)
            {
                if (!_allProperties.ContainsKey(prop)) throw new ApplicationException($"Property {prop} not exists or not observable");
                var oldValue = _previousState.ContainsKey(prop) ? _previousState[prop] : null;
                var newValue = _allProperties[prop].GetValue(this);

                var changed = oldValue == null && newValue != null
                    || oldValue != null && newValue == null
                    || (oldValue != null && newValue != null && !oldValue.Equals(newValue));

                if (changed) return true;
            }

            return false;
        }

        protected virtual Vector2 GetComponentSize()
        {
            return Parent?.GetComponentSize() ?? new Vector2(DrawerManager.Width, DrawerManager.Height);
        }

        protected abstract void DrawTexture();
        protected abstract bool CheckTextureValid();
        protected virtual void UpdateState(GameTime gameTime) { }
        protected void InvalidateTexture()
        {
            ValidTexture = false;
            Parent?.InvalidateTexture();
        }

        #endregion

        #region Private Methods
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

        private void UpdatePositions()
        {
            var parentScreenX = Parent?.ScreenX ?? 0;
            var parentScreenY = Parent?.ScreenY ?? 0;

            var parentWidth = Parent?.InnerWidth ?? 1024; // todo: get width from device manager
            var parentHeight = Parent?.InnerHeight ?? 768; // todo: get height from device manager

            var screenX = 0;
            var screenY = 0;

            if (Left != null) screenX = parentScreenX + Left.Value + (MarginLeft ?? 0);
            else if (Right != null) screenX = parentScreenX + parentWidth - Right.Value - (MarginRight ?? 0) - OuterWidth;

            if (Top != null) screenY = parentScreenY + Top.Value + (MarginTop ?? 0);
            else if (Bottom != null) screenY = parentScreenY + parentHeight - Bottom.Value - (MarginBottom ?? 0) - OuterHeight;

            ScreenX = screenX;
            ScreenY = screenY;
        }
        private void UpdateSizes()
        {
            var size = GetComponentSize();

            if (size.X <= 0 || size.Y <= 0)
            {
                InnerHeight = 0;
                InnerWidth = 0;
                OuterHeight = 0;
                OuterHeight = 0;
                return;
            }

            InnerWidth = (int)size.X + (PaddingLeft ?? 0) + (PaddingRight ?? 0);
            InnerHeight = (int)size.Y + (PaddingTop ?? 0) + (PaddingBottom ?? 0);

            // TODO: pending add border sizes
            OuterWidth = InnerWidth;
            OuterHeight = InnerHeight;
        }
        #endregion
    }
}
