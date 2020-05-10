using Microsoft.Xna.Framework.Graphics;
using Mir.Ethernity.ImageLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client
{
	public interface ITextureGenerator
	{
		Texture2D Generate(GraphicsDevice graphics, int width, int height, ImageDataType dataType, byte[] buffer);
	}
}
