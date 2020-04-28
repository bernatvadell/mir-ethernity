using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mir.Client.Services
{
    public class AssetLoader : IAssetLoader
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string GetPath(string relativePath)
        {
            return relativePath;
        }

        public Stream Load(string name)
        {
            return new FileStream(name, FileMode.Open, FileAccess.Read);
        }

        public void Save(string path, Stream stream)
        {
            CreateTreeDirectory(path);
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                stream.CopyTo(fs);
        }

        private void CreateTreeDirectory(string path)
        {
            var chunks = path.Split('/', '\\');
            var tmp = string.Empty;
            for (var i = 0; i < chunks.Length - 1; i++)
            {
                tmp = Path.Combine(tmp, chunks[i]);
                if (!Directory.Exists(tmp))
                    Directory.CreateDirectory(tmp);
            }
        }
    }
}
