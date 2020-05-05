using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mir.Client.Controls.Animators;
using Mir.Client.Models;
using Mir.Client.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mir.Client.Controls
{

    public abstract class BaseControl : IDisposable
    {
        public static BaseControl HoverControl { get; private set; }
        public static BaseControl FocusControl { get; private set; }

        private IDictionary<string, PropertyInfo> _allProperties;
        private IDictionary<string, FieldInfo> _allFields;

        private IDictionary<string, object> _previousState;
        private RenderTarget2D _renderTarget2D = null;
        private List<Animation> _animations = new List<Animation>();
        private ILifetimeScope _scope;

        protected IDrawerManager DrawerManager { get; }
        protected IRenderTargetManager RenderTargetManager { get; }

        protected bool ValidTexture { get; set; }
        protected bool Drawable { get; set; }

        public bool IsControl { get; set; } = false;
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
        public bool HorizontalCenter { get; set; }
        [Observable]
        public bool VerticalCenter { get; set; }
        [Observable]
        public int? Width { get; set; }
        [Observable]
        public int? Height { get; set; }
        [Observable]
        public bool UseBlend { get; set; }
        [Observable]
        public int? OffsetX { get; set; }
        [Observable]
        public int? OffsetY { get; set; }

        [Observable]
        public Color BackgroundColor { get; set; }

        [Observable]
        public bool Hovered { get; private set; }
        [Observable]
        public bool Touching { get; private set; }
        [Observable]
        public bool Focused { get => this == FocusControl; }

        public ControlCollection Controls { get; private set; }
        public BaseControl Parent { get; internal set; }

        public int OuterWidth { get; private set; }
        public int OuterHeight { get; private set; }

        public int ScreenX { get; private set; }
        public int ScreenY { get; private set; }

        public Rectangle DisplayArea { get; private set; }

        public event EventHandler<TouchEventArgs> GotHover;
        public event EventHandler LostHover;
        public event EventHandler<TouchEventArgs> Touch;

        public event EventHandler GotFocus;
        public event EventHandler LostFocus;

        public BaseControl(ILifetimeScope scope)
        {
            _scope = scope;

            DrawerManager = scope.Resolve<IDrawerManager>();
            RenderTargetManager = scope.Resolve<IRenderTargetManager>();

            BackgroundColor = Color.Transparent;

            Controls = new ControlCollection(this);

            _previousState = new Dictionary<string, object>();

            _allProperties = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute<ObservableAttribute>() != null)
                .ToDictionary(x => x.Name, x => x);

            _allFields = GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute<ObservableAttribute>() != null)
                .ToDictionary(x => x.Name, x => x);

            ValidTexture = true;
        }

        #region Public Methods

        public TControl CreateControl<TControl>(Action<TControl> configurer = null) where TControl : BaseControl
        {
            var control = _scope.Resolve<TControl>();
            configurer?.Invoke(control);
            Controls.Add(control);
            return control;
        }

        public void Focus()
        {
            if (!IsControl) return;
            FocusControl?.OnLostFocus();
            FocusControl = this;
            GotFocus?.Invoke(this, EventArgs.Empty);
        }

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
                    nameof(Width), nameof(Height), nameof(OffsetX), nameof(OffsetY),
                    nameof(BackgroundColor), nameof(Opacity), nameof(HorizontalCenter), nameof(VerticalCenter)
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

                        if (Drawable)
                        {
                            DrawTexture();
                        }

                        for (var i = 0; i < Controls.Length; i++)
                            Controls[i].Draw(gameTime);
                    }

                    ValidTexture = true;
                }
            }

            if (ValidTexture)
            {
                var useOpacity = Opacity >= 0 && Opacity < 1;

                var blendState = UseBlend ? new BlendState
                {
                    ColorSourceBlend = Blend.SourceAlpha,
                    AlphaSourceBlend = Blend.SourceAlpha,
                    ColorDestinationBlend = Blend.One,
                    AlphaDestinationBlend = Blend.One,
                    BlendFactor = new Color(255, 255, 255, 255)
                } : BlendState.AlphaBlend;

                using (var ctx = DrawerManager.PrepareSpriteBatch(blendState))
                {
                    ctx.Instance.Draw(_renderTarget2D, new Vector2(ScreenX, ScreenY), useOpacity ? Color.White * (float)Opacity : Color.White);
                }
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

        public virtual bool OnTextInput(Keys key, char character)
        {
            return false;
        }

        public virtual bool OnKeyDown(KeyEventArgs args)
        {
            var flag = false;

            for (var i = Controls.Length - 1; i >= 0 && !flag; i--)
                flag = Controls[i].OnKeyDown(args);

            return flag;
        }
        public virtual bool OnKeyUp(KeyEventArgs args)
        {
            var flag = false;

            for (var i = Controls.Length - 1; i >= 0 && !flag; i--)
                flag = Controls[i].OnKeyUp(args);

            return flag;
        }

        public void OnTouched(Point touchPoint)
        {
            Touching = false;
            Touch?.Invoke(this, new TouchEventArgs { Target = this, Point = touchPoint });
        }

        public void OnTouchStart(Point touchPoint)
        {
            Touching = true;
        }
        public void OnTouchMoving(Point touchPoint)
        {
            // TODO: Implements Movable control
        }

        public void OnLostHover()
        {
            if (HoverControl != this) return;
            HoverControl = null;
            LostHover?.Invoke(this, EventArgs.Empty);
        }

        public void OnGotHover(Point touchPoint)
        {
            HoverControl?.OnLostHover();
            HoverControl = this;
            GotHover?.Invoke(this, new TouchEventArgs { Target = this, Point = touchPoint });
        }

        #endregion

        #region Protected methods
        protected bool StateChanged(params string[] props)
        {
            if (props.Length == 0)
                props = _allProperties.Select(x => x.Key).ToArray();

            foreach (var prop in props)
            {
                if (!_allProperties.ContainsKey(prop) && !_allFields.ContainsKey(prop))
                    throw new ApplicationException($"Property {prop} not exists or not observable");

                var oldValue = _previousState.ContainsKey(prop) ? _previousState[prop] : null;
                object newValue = null;

                if (_allProperties.ContainsKey(prop))
                    newValue = _allProperties[prop].GetValue(this);
                else
                    newValue = _allFields[prop].GetValue(this);

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

        protected virtual void DrawTexture() { }
        protected virtual bool CheckTextureValid() { return false; }
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

            var parentWidth = Parent?.OuterWidth ?? DrawerManager.Width;
            var parentHeight = Parent?.OuterHeight ?? DrawerManager.Height;

            var screenX = 0;
            var screenY = 0;

            if (HorizontalCenter) screenX = (parentWidth / 2) - (OuterWidth / 2);
            else if (Left != null) screenX = Left.Value;
            else if (Right != null) screenX = parentWidth - Right.Value - OuterWidth;

            if (VerticalCenter) screenY = (parentHeight / 2) - (OuterHeight / 2);
            else if (Top != null) screenY = Top.Value;
            else if (Bottom != null) screenY = parentHeight - Bottom.Value - OuterHeight;

            ScreenX = screenX + (OffsetX ?? 0);
            ScreenY = screenY + (OffsetY ?? 0);

            DisplayArea = new Rectangle(parentScreenX + ScreenX, parentScreenY + ScreenY, OuterWidth, OuterHeight);
        }
        private void UpdateSizes()
        {
            var size = GetComponentSize();

            if (Width != null) size.X = Width.Value;
            if (Height != null) size.Y = Height.Value;

            if (size.X <= 0 || size.Y <= 0)
            {
                OuterHeight = 0;
                OuterHeight = 0;
                return;
            }

            // TODO: pending add border sizes
            OuterWidth = (int)size.X;
            OuterHeight = (int)size.Y;
        }

        private void OnLostFocus()
        {
            if (FocusControl != this) return;
            LostFocus?.Invoke(this, EventArgs.Empty);
            FocusControl = null;
        }
        #endregion
    }
}
