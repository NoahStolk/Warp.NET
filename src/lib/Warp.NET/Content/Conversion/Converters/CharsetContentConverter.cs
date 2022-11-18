using Warp.NET.Content.Conversion.Binaries;

namespace Warp.NET.Content.Conversion.Converters;

public sealed class CharsetContentConverter : IContentConverter<CharsetBinary>
{
	public static CharsetBinary Construct(string inputPath)
	{
		byte[] characters = File.ReadAllBytes(inputPath);
		return new(characters);
	}
}
