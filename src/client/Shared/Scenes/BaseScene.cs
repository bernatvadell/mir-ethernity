using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.MyraCustom;
using Mir.Client.Services;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes
{
    public abstract class BaseScene : Panel
    {
        public virtual void Update()
        {
            //foreach (var widget in MirWidget.Widgets)
            //{
            //    widget.Update();
            //}
        }

        public virtual void Load() { }
    }
}
