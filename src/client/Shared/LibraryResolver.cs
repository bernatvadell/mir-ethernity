using Mir.Ethernity.ImageLibrary;
using Mir.Ethernity.ImageLibrary.Zircon;
using Mir.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
    public static class LibraryResolver 
    {
        private static IDictionary<LibraryType, IImageLibrary> _libraries;

        static LibraryResolver()
        {
            _libraries = new Dictionary<LibraryType, IImageLibrary>
            {
                { LibraryType.Interface1c, new ZirconImageLibrary("data/interface1c.zl") },
                { LibraryType.Interface, new ZirconImageLibrary("data/interface.zl") }
            };
        }

        public static IImageLibrary Resolve(LibraryType type)
        {
            if (!_libraries.ContainsKey(type))
                return null;

            var library = _libraries[type];

            if (!library.Initialized)
                library.Initialize();

            return library;
        }
    }
}
