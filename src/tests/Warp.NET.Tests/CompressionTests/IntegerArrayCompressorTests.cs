using Warp.NET.Utils;

namespace Warp.NET.Tests.CompressionTests;

[TestClass]
public class IntegerArrayCompressorTests
{
	[DataTestMethod]
	[DataRow(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
	[DataRow(new byte[] { 1, 2, 3, 4, 5 })]
	[DataRow(new byte[] { 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1 })]
	[DataRow(new byte[] { 0, 0, 0, 0 })]
	[DataRow(new byte[] { 0 })]
	[DataRow(new byte[] { 1 })]
	[DataRow(new byte[] { })]
	[DataRow(new byte[] { 127, 127, 127 })]
	[DataRow(new byte[] { 255, 255, 255, 255 })]
	public void CompressAndDecompress(byte[] input)
	{
		int length = input.Length;

		byte[] compressed = IntegerArrayCompressor.CompressData(input);
		byte[] decompressed = IntegerArrayCompressor.ExtractData(compressed);

		if (decompressed.Length == 0)
		{
			for (int i = 0; i < length; i++)
				Assert.AreEqual(0, input[i]);
		}
		else
		{
			Assert.IsTrue(input.SequenceEqual(decompressed[..length]));
		}
	}
}
