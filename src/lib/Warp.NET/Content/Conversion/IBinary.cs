namespace Warp.NET.Content.Conversion;

public interface IBinary<out TSelf>
	where TSelf : IBinary<TSelf>
{
	ContentType ContentType { get; }

	byte[] ToBytes();

	static abstract TSelf FromStream(BinaryReader br);
}
