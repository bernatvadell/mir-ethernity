using Mir.Ethernity.ImageLibrary;
using Mir.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services
{
    public interface ILibraryResolver
    {
        IImageLibrary Resolve(LibraryType type);
    }
}
