using Warp.NET.Content.Binaries.Data;
using Warp.NET.Content.Binaries.Parsers;

namespace Warp.NET.Content.Binaries.Types;

public record SoundBinary(byte[] Contents) : IBinary<SoundBinary>
{
	public ContentType ContentType => ContentType.Sound;

	public static SoundBinary Construct(string inputPath)
	{
		WaveData waveData = WaveParser.Parse(File.ReadAllBytes(inputPath));

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(waveData.Channels);
		bw.Write(waveData.SampleRate);
		bw.Write(waveData.BitsPerSample);
		bw.Write(waveData.Data.Length);
		bw.Write(waveData.Data);

		return new(ms.ToArray());
	}

	public static Sound Deconstruct(BinaryReader br)
	{
		short channels = br.ReadInt16();
		int sampleRate = br.ReadInt32();
		short bitsPerSample = br.ReadInt16();
		int size = br.ReadInt32();
		byte[] data = br.ReadBytes(size);
		return new(channels, sampleRate, bitsPerSample, data.Length, data);
	}
}
