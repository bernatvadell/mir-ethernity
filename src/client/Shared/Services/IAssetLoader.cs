using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mir.Client.Services
{
    public interface IAssetLoader
    {
        Stream Load(string name);
        bool Exists(string path);
        string GetPath(string relativePath);
        void Save(string path, Stream stream);
    }
}
