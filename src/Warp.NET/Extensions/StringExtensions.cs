namespace Warp.NET.Extensions;

public static class StringExtensions
{
	public static string TrimSpacesAround(this string str, params char[] chars)
	{
		foreach (char c in chars)
		{
			str = str.Replace($" {c}", c.ToString());
			str = str.Replace($"{c} ", c.ToString());
		}

		return str;
	}

	public static string TrimSpacesAround(this string str, params string[] substrings)
	{
		foreach (string substring in substrings)
		{
			str = str.Replace($" {substring}", substring);
			str = str.Replace($"{substring} ", substring);
		}

		return str;
	}

	public static string RemoveDuplicatesOf(this string str, params string[] substrings)
	{
		foreach (string substring in substrings)
		{
			string substringDuplicate = string.Concat(Enumerable.Repeat(substring, 2));

			while (str.Contains(substringDuplicate))
				str = str.Replace(substringDuplicate, substring);
		}

		return str;
	}
}
