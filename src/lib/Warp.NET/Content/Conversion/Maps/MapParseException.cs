using System.Runtime.Serialization;

namespace Warp.NET.Content.Conversion.Maps;

[Serializable]
public class MapParseException : Exception
{
	public MapParseException()
	{
	}

	public MapParseException(string? message)
		: base(message)
	{
	}

	public MapParseException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected MapParseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
