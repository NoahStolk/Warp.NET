using Warp.NET.Content.Binaries.Data;
using Warp.NET.Content.Binaries.Parsers;
using Warp.NET.Utils;

namespace Warp.NET.Content.Binaries.Types;

public class TextureBinary : IBinary
{
	private TextureData? _textureData;

	public ContentType ReadFromPath(string path)
	{
		_textureData = TgaParser.Parse(File.ReadAllBytes(path));
		return ColorDataUtils.DetermineTextureContentType(_textureData.ColorData);
	}

	public byte[] ToBytes(ContentType contentType)
	{
		if (_textureData == null)
			throw new InvalidOperationException("Binary is not initialized.");

		return ColorDataUtils.WriteTextureBinary(_textureData.Width, _textureData.Height, _textureData.ColorData, contentType);
	}
}
