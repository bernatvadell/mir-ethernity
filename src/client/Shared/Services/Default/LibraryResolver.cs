using Autofac;
using Mir.Ethernity.ImageLibrary;
using Mir.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class LibraryResolver : ILibraryResolver
    {
        private IDictionary<LibraryType, IImageLibrary> _libraries;

        public LibraryResolver(IAssetLoader assetLoader, ILifetimeScope container)
        {
            _libraries = new Dictionary<LibraryType, IImageLibrary>
            {
                { LibraryType.Interface1c, container.Resolve<IImageLibrary>(new TypedParameter(typeof(string), assetLoader.GetPath("data/interface1c.zl"))) },
                { LibraryType.Interface, container.Resolve<IImageLibrary>(new TypedParameter(typeof(string), assetLoader.GetPath("data/interface.zl"))) }
            };
        }

        public IImageLibrary Resolve(LibraryType type)
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
