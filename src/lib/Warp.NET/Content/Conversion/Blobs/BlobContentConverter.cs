namespace Warp.NET.Content.Conversion.Blobs;

public sealed class BlobContentConverter : IContentConverter<BlobBinary>
{
	public static BlobBinary Construct(string inputPath)
	{
		byte[] data = File.ReadAllBytes(inputPath);
		return new(data);
	}
}
