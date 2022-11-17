using Warp.NET.Content.Binaries.Data;
using Warp.NET.Content.Binaries.Parsers;
using Warp.NET.Utils;

namespace Warp.NET.Content.Binaries.Types;

public record TextureBinary(ContentType ContentType, byte[] Contents) : IBinary<TextureBinary>
{
	public static TextureBinary Construct(string inputPath)
	{
		TextureData textureData = TgaParser.Parse(File.ReadAllBytes(inputPath));
		ContentType contentType = ColorDataUtils.DetermineTextureContentType(textureData.ColorData);
		return new(contentType, ColorDataUtils.WriteTextureBinary(textureData.Width, textureData.Height, textureData.ColorData, contentType));
	}
}
