using System.Runtime.Serialization;

namespace Warp.NET.Content.Conversion.Textures;

[Serializable]
public class TgaParseException : Exception
{
	public TgaParseException()
	{
	}

	public TgaParseException(string? message)
		: base(message)
	{
	}

	public TgaParseException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected TgaParseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
