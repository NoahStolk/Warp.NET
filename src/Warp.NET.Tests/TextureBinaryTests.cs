using Warp.NET.Content.Conversion.Binaries;
using Warp.NET.Content.Conversion.Converters;
using Warp.NET.Tests.Utils;

namespace Warp.NET.Tests;

[TestClass]
public class TextureBinaryTests
{
	[DataTestMethod]
	[DataRow("Font.tga", 1152, 12)]
	[DataRow("Placeholder.tga", 16, 16)]
	public void TestTextureBinary(string fileName, int width, int height)
	{
		TextureBinary textureBinaryFromFile = TextureContentConverter.Construct(ResourceUtils.GetResourcePath(fileName));
		Assert.AreEqual(width, textureBinaryFromFile.Width);
		Assert.AreEqual(height, textureBinaryFromFile.Height);

		byte[] textureBytes = textureBinaryFromFile.ToBytes();

		using MemoryStream ms = new(textureBytes);
		using BinaryReader br = new(ms);

		TextureBinary textureBinaryConverted = TextureBinary.FromStream(br);
		Assert.AreEqual(width, textureBinaryConverted.Width);
		Assert.AreEqual(height, textureBinaryConverted.Height);

		CollectionAssert.AreEqual(textureBinaryFromFile.ColorData, textureBinaryConverted.ColorData);
	}
}
