using Warp.NET.Content.Conversion.GameDefinitions.Properties;
using Warp.NET.Utils.Parsing;

namespace Warp.NET.Content.Conversion.GameDefinitions;

internal class GameDefinitionParser
{
	public static GameDefinitionData Parse(byte[] fileContents)
	{
		try
		{
			List<GameClass> classes = new();

			StringIterator stringIterator = new(Encoding.UTF8.GetString(fileContents));

			do
			{
				if (stringIterator.IsNext("@SolidClass"))
					classes.Add(ParseGameClass(stringIterator, ClassType.Solid));
				else if (stringIterator.IsNext("@PointClass"))
					classes.Add(ParseGameClass(stringIterator, ClassType.Point));
				else if (stringIterator.IsNext("@BaseClass"))
					classes.Add(ParseGameClass(stringIterator, ClassType.Base));
			}
			while (stringIterator.Advance());

			return new(classes);
		}
		catch (UnexpectedEndOfStringException ex)
		{
			throw new GameDefinitionParseException("Unexpected end of file. Game definition could not be parsed.", ex);
		}
	}

	private static GameClass ParseGameClass(StringIterator stringIterator, ClassType classType)
	{
		List<string> baseClassNames = new();
		string? className = null;

		do
		{
			if (stringIterator.IsNext("base("))
			{
				string commaSeparatedBaseClassNames = stringIterator.ReadBetween("(", ")", true);
				baseClassNames = commaSeparatedBaseClassNames.Split(",").ToList();
			}
			else if (stringIterator.IsNext("="))
			{
				stringIterator.Advance();

				className = stringIterator.ReadUntil("[", true).Trim();

				if (className.Contains(':'))
					className = className.Split(':')[0].Trim();

				break;
			}
		}
		while (stringIterator.Advance());

		if (className == null)
			throw new GameDefinitionParseException("Class name not found. Game definition could not be parsed.");

		string classBody = stringIterator.ReadUntil("]", true);

		(List<ChoicesProperty> choicesProperties, List<FlagsProperty> flagsProperties, List<IntegerProperty> integerProperties, List<StringProperty> stringProperties) = ParseProperties(classBody);

		return new(classType, baseClassNames, className, choicesProperties, flagsProperties, integerProperties, stringProperties);
	}

	private static (List<ChoicesProperty> ChoicesProperties, List<FlagsProperty> FlagsProperties, List<IntegerProperty> IntegerProperties, List<StringProperty> StringProperties) ParseProperties(string classBody)
	{
		List<ChoicesProperty> choicesProperties = new();
		List<FlagsProperty> flagsProperties = new();
		List<IntegerProperty> integerProperties = new();
		List<StringProperty> stringProperties = new();

		string[] properties = classBody.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

		foreach (string property in properties)
		{
			StringIterator stringIterator = new(property);
			string propertyName = stringIterator.ReadUntil("(", false).Trim();

			if (stringIterator.IsNext("(choices)"))
				choicesProperties.Add(ParseChoicesProperty(stringIterator, propertyName));
			else if (stringIterator.IsNext("(flags)"))
				flagsProperties.Add(ParseFlagsProperty(stringIterator, propertyName));
			else if (stringIterator.IsNext("(integer)"))
				integerProperties.Add(ParseIntegerProperty(stringIterator, propertyName));
			else if (stringIterator.IsNext("(string)"))
				stringProperties.Add(ParseStringProperty(stringIterator, propertyName));
		}

		return (choicesProperties, flagsProperties, integerProperties, stringProperties);
	}

	private static ChoicesProperty ParseChoicesProperty(StringIterator stringIterator, string propertyName)
	{
		// Skip "In-Editor Name".
		stringIterator.ReadBetween("\"", "\"", true);

		string defaultValue = stringIterator.ReadBetween(":", ":", true);
		if (!int.TryParse(defaultValue, out int defaultValueParsed))
			throw new GameDefinitionParseException($"'{defaultValue}' could not be parsed to an integer.");

		// Skip "In-Editor Help".
		stringIterator.ReadBetween("\"", "\"", true);

		string[] choices = stringIterator.ReadBetween("[", "]", true).Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
		if (choices.Length == 0)
			throw new GameDefinitionParseException("Choices property must have at least one choice.");

		Dictionary<int, string> choicesDictionary = choices
			.Select(s =>
			{
				string[] split = s.Split(":");
				if (split.Length != 2)
					throw new GameDefinitionParseException("Choices property must have a value and a name separated by a colon.");

				if (!int.TryParse(split[0], out int value))
					throw new GameDefinitionParseException("Choices property value must be an integer.");

				return (Value: value, Name: split[1].Trim().TrimStart('\"').TrimEnd('\"'));
			})
			.ToDictionary(kvp => kvp.Value, kvp => kvp.Name);

		return new(propertyName, defaultValueParsed, choicesDictionary);
	}

	private static FlagsProperty ParseFlagsProperty(StringIterator stringIterator, string propertyName)
	{
		// TODO: Implement.
		return new(propertyName, new List<Flag>());
	}

	private static IntegerProperty ParseIntegerProperty(StringIterator stringIterator, string propertyName)
	{
		// Skip "In-Editor Name".
		stringIterator.ReadBetween("\"", "\"", true);

		string defaultValue = stringIterator.ReadBetween(":", ":", true);

		int? defaultValueFinal;
		if (string.IsNullOrWhiteSpace(defaultValue))
			defaultValueFinal = null;
		else if (int.TryParse(defaultValue, out int defaultValueParsed))
			defaultValueFinal = defaultValueParsed;
		else
			throw new GameDefinitionParseException($"'{defaultValue}' could not be parsed to an integer.");

		// Skip "In-Editor Help".
		stringIterator.ReadBetween("\"", "\"", true);

		return new(propertyName, defaultValueFinal);
	}

	private static StringProperty ParseStringProperty(StringIterator stringIterator, string propertyName)
	{
		// Skip "In-Editor Name".
		stringIterator.ReadBetween("\"", "\"", true);

		string defaultValue = stringIterator.ReadBetween(":", ":", true);
		string? defaultValueParsed = string.IsNullOrWhiteSpace(defaultValue) ? null : defaultValue.Trim().TrimStart('\"').TrimEnd('\"');

		// Skip "In-Editor Help".
		stringIterator.ReadBetween("\"", "\"", true);

		return new(propertyName, defaultValueParsed);
	}
}
