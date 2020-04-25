using Microsoft.Xna.Framework.Graphics;
using Mir.Ethernity.ImageLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mir.Client.Services
{
    public class TextureGenerator : ITextureGenerator
    {
        public Texture2D Generate(GraphicsDevice device, int width, int height, ImageDataType dataType, byte[] buffer)
        {
            Texture2D texture;

            int w = width + (4 - width % 4) % 4;
            int h = height + (4 - height % 4) % 4;

            switch (dataType)
            {
                case ImageDataType.Dxt1:
                    texture = new Texture2D(device, w, h, false, SurfaceFormat.Dxt1);
                    texture.SetData(buffer);
                    break;
                case ImageDataType.Dxt3:
                    texture = new Texture2D(device, w, h, false, SurfaceFormat.Dxt3);
                    texture.SetData(buffer);
                    break;
                case ImageDataType.Dxt5:
                    texture = new Texture2D(device, w, h, false, SurfaceFormat.Dxt5);
                    texture.SetData(buffer);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return texture;
        }
    }
}
