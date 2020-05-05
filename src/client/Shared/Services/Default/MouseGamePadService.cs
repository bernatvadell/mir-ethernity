using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mir.Client.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class MouseGamePadService : IGamePadService
    {
        private MouseState _oldMouseState;
        private IDrawerManager _drawerManager;
        private ISceneManager _sceneManager;

        public MouseState MouseState { get; set; }
        public Point MousePoint { get; private set; }
        public BaseControl ClickedControl { get; set; }

        public MouseGamePadService(IDrawerManager drawerManager, ISceneManager sceneManager)
        {
            _drawerManager = drawerManager;
            _sceneManager = sceneManager;
        }

        public void Update(GameTime gameTime)
        {
            MouseState = Mouse.GetState();

            if (MouseState.X < 0 || MouseState.Y < 0
                || MouseState.X > _drawerManager.Width
                || MouseState.Y > _drawerManager.Height)
                return;

            MousePoint = _drawerManager.GetResizedPoint(new Point(MouseState.X, MouseState.Y));

            var mouseHoverControl = GetControlByPosition(MousePoint);

            if (mouseHoverControl == null && BaseControl.HoverControl != null)
            {
                BaseControl.HoverControl.OnLostHover();
            }
            else if (BaseControl.HoverControl != mouseHoverControl)
            {
                mouseHoverControl?.OnGotHover(MousePoint);
            }

            if (ClickedControl != null && MouseState.LeftButton == ButtonState.Released)
            {
                ClickedControl.OnTouched(MousePoint);

                if (ClickedControl.IsControl)
                    ClickedControl.Focus();

                ClickedControl = null;
            }
            else if (MouseState.LeftButton == ButtonState.Pressed)
            {
                if (ClickedControl == null)
                {
                    if (mouseHoverControl != null)
                    {
                        ClickedControl = mouseHoverControl;
                        ClickedControl.OnTouchStart(MousePoint);
                    }
                }
                else
                {
                    if (_oldMouseState.X != MouseState.X || _oldMouseState.Y != MouseState.Y)
                        ClickedControl.OnTouchMoving(MousePoint);
                }
            }

            _oldMouseState = MouseState;
        }

        private BaseControl GetControlByPosition(Point point)
        {
            return GetControlByPositionRecursive(point, _sceneManager.Active);
        }

        private BaseControl GetControlByPositionRecursive(Point point, BaseControl control)
        {
            if (control == null) return null;

            for (var i = control.Controls.Length - 1; i >= 0; i--)
            {
                var found = GetControlByPositionRecursive(point, control.Controls[i]);
                if (found != null)
                    return found;
            }

            return control.IsControl && control.DisplayArea.Contains(point)
                ? control : null;
        }
    }
}
