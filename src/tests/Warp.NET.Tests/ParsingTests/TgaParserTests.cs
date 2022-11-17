using Warp.NET.Content.Conversion.Data;
using Warp.NET.Content.Conversion.Parsers;
using Warp.NET.Tests.Utils;

namespace Warp.NET.Tests.ParsingTests;

[TestClass]
public class TgaParserTests
{
	[DataTestMethod]
	[DataRow("Font.tga", 1152, 12)]
	[DataRow("Placeholder.tga", 16, 16)]
	public void ParseTgaBytes(string fileName, int expectedWidth, int expectedHeight)
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath(fileName));
		TextureData texture = TgaParser.Parse(bytes);

		Assert.AreEqual(expectedWidth, texture.Width);
		Assert.AreEqual(expectedHeight, texture.Height);
	}
}
