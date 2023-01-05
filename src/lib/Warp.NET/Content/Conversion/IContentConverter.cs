namespace Warp.NET.Content.Conversion;

public interface IContentConverter<out TBinary>
	where TBinary : IBinary<TBinary>
{
	/// <summary>
	/// Constructs the binary from the file located at <paramref name="inputPath"/>.
	/// </summary>
	static abstract TBinary Construct(string inputPath);
}
