using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mir.Client.Controls;
using Mir.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class KeyboardGamePadService : IGamePadService
    {
        private KeyboardState? _previousState;
        private TimeController _keyboardTime = new TimeController(TimeSpan.FromMilliseconds(60));
        private ISceneManager _sceneManager;

        public KeyboardGamePadService(ISceneManager sceneManager)
        {
            _sceneManager = sceneManager ?? throw new ArgumentNullException(nameof(sceneManager));
        }

        public void Update(GameTime gameTime)
        {
            var control = BaseControl.FocusControl ?? _sceneManager.Active;
            if (control == null) return;

            var state = Keyboard.GetState();
            var prevKeys = _previousState?.GetPressedKeys();
            var currKeys = state.GetPressedKeys();
            var empty = ((prevKeys?.Length ?? 0) == 0 && currKeys.Length == 0);

            if ((!_keyboardTime.CheckProcess(gameTime) && prevKeys != null && Enumerable.SequenceEqual(prevKeys, currKeys)) || empty)
            {
                return;
            }

            var keysDown = currKeys;
            var keysUp = prevKeys == null ? new Keys[0] : prevKeys.Where(x => !currKeys.Contains(x)).ToArray();

            if (keysDown.Length > 0)
            {
                var eventArgs = new KeyEventArgs(keysDown);
                var tmp = control;
                do
                {
                    if (tmp.OnKeyDown(eventArgs))
                        break;

                    tmp = tmp.Parent;
                } while (tmp != null);
            }

            if (keysUp.Length > 0)
            {
                var eventArgs = new KeyEventArgs(keysUp);
                var tmp = control;
                do
                {
                    if (tmp.OnKeyUp(eventArgs))
                        break;

                    tmp = tmp.Parent;
                } while (tmp != null);
            }


            _previousState = state;
            _keyboardTime.Reset(TimeSpan.FromMilliseconds(prevKeys?.Length == 0 ? 500 : 60));
        }
    }
}
