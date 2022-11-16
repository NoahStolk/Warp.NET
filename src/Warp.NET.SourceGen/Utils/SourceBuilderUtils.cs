using Microsoft.CodeAnalysis.Text;

namespace Warp.NET.SourceGen.Utils;

public static class SourceBuilderUtils
{
	private const string _fileHeader = """
		// <auto-generated>
		// This code was generated by Warp.NET.SourceGen.
		// </auto-generated>
		""";

	private static readonly string[] _suppressions =
	{
		"CS0105",
		"CS1591",
		"CS8618",
		"HAA0301",
		"HAA0302",
		"S1128",
		"S1185",
		"S3973",
		"SA1001",
		"SA1027",
		"SA1028",
		"SA1101",
		"SA1122",
		"SA1137",
		"SA1200",
		"SA1201",
		"SA1202",
		"SA1208",
		"SA1210",
		"SA1309",
		"SA1311",
		"SA1413",
		"SA1503",
		"SA1505",
		"SA1507",
		"SA1508",
		"SA1516",
		"SA1600",
		"SA1601",
		"SA1602",
		"SA1623",
		"SA1649",
	};

	private static readonly string _warningSuppressionCodes = string.Join(", ", _suppressions);

	public static SourceText Build(string sourceCode)
	{
		return SourceText.From($"{_fileHeader}{Constants.NewLine}{Constants.NewLine}#pragma warning disable {_warningSuppressionCodes}{Constants.NewLine}#nullable enable{Constants.NewLine}{Constants.NewLine}{sourceCode}{Constants.NewLine}", Encoding.UTF8);
	}

	public static SourceText GenerateAttribute(AttributeTargets attributeTargets, string attributeName)
	{
		List<AttributeTargets> targets = Enum.GetValues(typeof(AttributeTargets)).Cast<AttributeTargets>().Where(attr => attributeTargets.HasFlag(attr)).ToList();
		string attributeTargetsString = string.Join(" | ", targets.Select(s => $"System.AttributeTargets.{s}"));

		return Build($$"""
			namespace Warp;

			[System.AttributeUsage({{attributeTargetsString}})]
			public class {{attributeName}} : System.Attribute
			{
			}
			""");
	}
}
