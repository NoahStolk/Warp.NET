using System.Runtime.Serialization;

namespace Warp.NET.Content.Conversion.GameDefinitions;

[Serializable]
public class GameDefinitionParseException : Exception
{
	public GameDefinitionParseException()
	{
	}

	public GameDefinitionParseException(string? message)
		: base(message)
	{
	}

	public GameDefinitionParseException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected GameDefinitionParseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
