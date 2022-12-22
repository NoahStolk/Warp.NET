using Warp.NET.Content.Conversion.Binaries;

namespace Warp.NET.Content.Conversion.Converters;

public sealed class BlobContentConverter : IContentConverter<BlobBinary>
{
	public static BlobBinary Construct(string inputPath)
	{
		byte[] data = File.ReadAllBytes(inputPath);
		return new(data);
	}
}
