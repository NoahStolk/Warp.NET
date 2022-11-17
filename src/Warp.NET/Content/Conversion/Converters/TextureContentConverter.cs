using Warp.NET.Content.Conversion.Binaries;
using Warp.NET.Content.Conversion.Data;
using Warp.NET.Content.Conversion.Parsers;

namespace Warp.NET.Content.Conversion.Converters;

public sealed class TextureContentConverter : IContentConverter<TextureBinary>
{
	public static TextureBinary Construct(string inputPath)
	{
		TextureData textureData = TgaParser.Parse(File.ReadAllBytes(inputPath));
		return new(textureData.Width, textureData.Height, textureData.ColorData);
	}
}
