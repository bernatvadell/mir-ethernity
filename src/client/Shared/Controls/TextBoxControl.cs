using Mir.Client.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls
{
    public class TextBoxControl : BaseControl
    {
        public TextBoxControl(IDrawerManager drawerManager, IRenderTargetManager renderTargetManager) : base(drawerManager, renderTargetManager)
        {
        }

        protected override bool CheckTextureValid()
        {
            return true;
        }

        protected override void DrawTexture()
        {
            
        }
    }
}
