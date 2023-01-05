using Warp.NET.Content.Conversion.Sounds;
using Warp.NET.Tests.Utils;

namespace Warp.NET.Tests.ContentTests;

[TestClass]
public class SoundBinaryTests
{
	[DataTestMethod]
	[DataRow("Sample1.wav", (short)1, 11000, (short)8, 2208)]
	[DataRow("Sample2.wav", (short)1, 11025, (short)16, 61544)]
	public void TestSoundBinary(string fileName, short expectedChannels, int expectedSampleRate, short expectedBitsPerSample, int expectedDataSize)
	{
		SoundBinary soundBinaryFromFile = SoundContentConverter.Construct(ResourceUtils.GetResourcePath(fileName));
		Assert.AreEqual(expectedChannels, soundBinaryFromFile.Channels);
		Assert.AreEqual(expectedSampleRate, soundBinaryFromFile.SampleRate);
		Assert.AreEqual(expectedBitsPerSample, soundBinaryFromFile.BitsPerSample);
		Assert.AreEqual(expectedDataSize, soundBinaryFromFile.Data.Length);

		byte[] soundBytes = soundBinaryFromFile.ToBytes();

		using MemoryStream ms = new(soundBytes);
		using BinaryReader br = new(ms);

		SoundBinary soundBinaryConverted = SoundBinary.FromStream(br);
		Assert.AreEqual(expectedChannels, soundBinaryConverted.Channels);
		Assert.AreEqual(expectedSampleRate, soundBinaryConverted.SampleRate);
		Assert.AreEqual(expectedBitsPerSample, soundBinaryConverted.BitsPerSample);
		Assert.AreEqual(expectedDataSize, soundBinaryConverted.Data.Length);

		CollectionAssert.AreEqual(soundBinaryFromFile.Data, soundBinaryConverted.Data);
	}
}
