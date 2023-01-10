using System.Runtime.Serialization;

namespace Warp.NET.Utils.Parsing;

[Serializable]
public class UnexpectedEndOfStringException : Exception
{
	public UnexpectedEndOfStringException()
	{
	}

	public UnexpectedEndOfStringException(string message)
		: base(message)
	{
	}

	public UnexpectedEndOfStringException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UnexpectedEndOfStringException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
