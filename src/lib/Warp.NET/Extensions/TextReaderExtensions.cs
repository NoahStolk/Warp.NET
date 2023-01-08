namespace Warp.NET.Extensions;

public static class TextReaderExtensions
{
	public static string ReadBetween(this TextReader textReader, char beginChar, char endChar)
	{
		StringBuilder builder = new();
		bool begin = false;
		while (true)
		{
			int c = textReader.Read();
			if (c == -1)
				throw new EndOfStreamException("Unexpected end of file.");

			if (begin)
			{
				if (c == endChar)
					return builder.ToString();

				builder.Append((char)c);
			}

			if (c == beginChar)
				begin = true;
		}
	}

	public static string ReadUntilNewline(this TextReader textReader)
	{
		StringBuilder builder = new();
		while (true)
		{
			int c = textReader.Read();
			if (c is -1 or '\n' or '\r')
				return builder.ToString();

			builder.Append((char)c);
		}
	}

	public static string ReadUntil(this TextReader textReader, char endChar)
	{
		StringBuilder builder = new();
		while (true)
		{
			int c = textReader.Read();
			if (c == -1)
				throw new EndOfStreamException("Unexpected end of file.");

			if (c == endChar)
				return builder.ToString();

			builder.Append((char)c);
		}
	}
}
