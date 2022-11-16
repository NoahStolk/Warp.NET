namespace Warp.NET.Content.Binaries.Types;

public interface IBinary
{
	/// <summary>
	/// Used for creating an object from the content file.
	/// </summary>
	ContentType ReadFromPath(string path);

	/// <summary>
	/// Used for writing the object to the binary.
	/// </summary>
	byte[] ToBytes(ContentType contentType);
}
