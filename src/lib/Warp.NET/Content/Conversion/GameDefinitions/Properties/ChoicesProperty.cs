namespace Warp.NET.Content.Conversion.GameDefinitions.Properties;

internal record ChoicesProperty
{
	public ChoicesProperty(string name, int defaultValue, IReadOnlyDictionary<int, string> choices)
	{
		if (!choices.ContainsKey(defaultValue))
			throw new ArgumentException($"The default value '{defaultValue}' is not present in the choices.");

		Name = name;
		DefaultValue = defaultValue;
		Choices = choices;
	}

	public string Name { get; }
	public int DefaultValue { get; }
	public IReadOnlyDictionary<int, string> Choices { get; }
}
