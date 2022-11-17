namespace Warp.NET.Content.Binaries.ContentConverters;

public interface IContentConverter<out TSelf>
	where TSelf : IContentConverter<TSelf>
{
	/// <summary>
	/// The content type determined from the input file.
	/// </summary>
	ContentType ContentType { get; }

	/// <summary>
	/// The binary contents which will be written to the content file.
	/// </summary>
	byte[] Contents { get; }

	/// <summary>
	/// Constructs the binary from the file located at <paramref name="inputPath"/>.
	/// </summary>
	static abstract TSelf Construct(string inputPath);
}
