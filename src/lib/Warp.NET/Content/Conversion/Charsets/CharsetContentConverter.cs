namespace Warp.NET.Content.Conversion.Charsets;

public sealed class CharsetContentConverter : IContentConverter<CharsetBinary>
{
	public static CharsetBinary Construct(string inputPath)
	{
		byte[] characters = File.ReadAllBytes(inputPath);
		return new(characters);
	}
}
