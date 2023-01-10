using Warp.NET.Content.Conversion.GameDefinitions.Properties;

namespace Warp.NET.Content.Conversion.GameDefinitions;

internal record GameClass(
	ClassType Type,
	IReadOnlyList<string> BaseClassNames,
	string Name,
	IReadOnlyList<ChoicesProperty> ChoicesProperties,
	IReadOnlyList<FlagsProperty> FlagsProperties,
	IReadOnlyList<IntegerProperty> IntegerProperties,
	IReadOnlyList<StringProperty> StringProperties);
