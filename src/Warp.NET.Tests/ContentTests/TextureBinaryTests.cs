using Warp.NET.Content.Conversion.Binaries;
using Warp.NET.Content.Conversion.Converters;
using Warp.NET.Tests.Utils;

namespace Warp.NET.Tests.ContentTests;

[TestClass]
public class TextureBinaryTests
{
	[DataTestMethod]
	[DataRow("Font.tga", 1152, 12)]
	[DataRow("Placeholder.tga", 16, 16)]
	public void TestTextureBinary(string fileName, int expectedWidth, int expectedHeight)
	{
		TextureBinary textureBinaryFromFile = TextureContentConverter.Construct(ResourceUtils.GetResourcePath(fileName));
		Assert.AreEqual(expectedWidth, textureBinaryFromFile.Width);
		Assert.AreEqual(expectedHeight, textureBinaryFromFile.Height);

		byte[] textureBytes = textureBinaryFromFile.ToBytes();

		using MemoryStream ms = new(textureBytes);
		using BinaryReader br = new(ms);

		TextureBinary textureBinaryConverted = TextureBinary.FromStream(br);
		Assert.AreEqual(expectedWidth, textureBinaryConverted.Width);
		Assert.AreEqual(expectedHeight, textureBinaryConverted.Height);

		CollectionAssert.AreEqual(textureBinaryFromFile.ColorData, textureBinaryConverted.ColorData);
	}
}
