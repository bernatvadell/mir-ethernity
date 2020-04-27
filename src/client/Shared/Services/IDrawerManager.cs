using Mir.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services
{
    public interface IDrawerManager
    {
        DrawerContext BuildContext();
    }
}
