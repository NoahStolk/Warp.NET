namespace Warp.NET.Content.Conversion.Maps;

[DebuggerDisplay("{_index}, {_str[_index]}")]
internal class StringIterator
{
	private readonly string _str;
	private int _index;

	public StringIterator(string str)
	{
		_str = str;
	}

	/// <summary>
	/// Advances the iterator to the next character. Returns <see langword="false" /> if the end of the string was reached.
	/// </summary>
	public bool Advance()
	{
		if (_index >= _str.Length)
			return false;

		_index++;
		return true;
	}

	/// <summary>
	/// Returns whether the characters in <paramref name="chars"/> come next in the string.
	/// </summary>
	public bool IsNext(ReadOnlySpan<char> chars)
	{
		if (_index + chars.Length > _str.Length)
			return false;

		for (int i = 0; i < chars.Length; i++)
		{
			if (_str[_index + i] != chars[i])
				return false;
		}

		return true;
	}

	/// <summary>
	/// Reads from the string until the characters in <paramref name="end"/> are found.
	/// The end characters are not included in the result.
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when the end of the string was reached before finding the characters in <paramref name="end"/>.</exception>
	public string ReadUntil(ReadOnlySpan<char> end, bool advancePastEnd)
	{
		int startIndex = _index;
		while (!IsNext(end))
		{
			if (!Advance())
				throw new InvalidOperationException($"End of string before reaching end string: {end}");
		}

		string str = _str.Substring(startIndex, _index - startIndex);

		if (advancePastEnd)
			_index += end.Length;

		return str;
	}

	/// <summary>
	/// Reads from the string between the characters in <paramref name="start"/> and <paramref name="end"/>.
	/// Then advances the iterator past the end.
	/// The start and end characters are not included in the result.
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when the end of the string was reached before finding the characters in <paramref name="start"/> or <paramref name="end"/>.</exception>
	public string ReadBetween(ReadOnlySpan<char> start, ReadOnlySpan<char> end, bool advancePastEnd)
	{
		while (!IsNext(start))
		{
			if (!Advance())
				throw new InvalidOperationException($"End of string before reaching start string: {start}");
		}

		_index += start.Length;

		int startIndex = _index;

		while (!IsNext(end))
		{
			if (!Advance())
				throw new InvalidOperationException($"End of string before reaching end string: {end}");
		}

		string str = _str.Substring(startIndex, _index - startIndex);

		if (advancePastEnd)
			_index += end.Length;

		return str;
	}
}
