using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Exceptions;
using Mir.Client.Scenes;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client
{
    public class SceneManager 
    {
        public static BaseScene Active { get; private set; }

        public static void Load(BaseScene scene, bool throwException = true)
        {
            Active?.RemoveFromDesktop();
            Active = scene;
            Desktop.Root = Active;
            Active.Load();
        }
    }
}
