using Warp.NET.Content.Binaries.Data;
using Warp.NET.Content.Binaries.Parsers;

namespace Warp.NET.Content.Binaries.ContentConverters;

public record SoundContentConverter(byte[] Contents) : IContentConverter<SoundContentConverter>
{
	public ContentType ContentType => ContentType.Sound;

	public static SoundContentConverter Construct(string inputPath)
	{
		SoundData soundData = WaveParser.Parse(File.ReadAllBytes(inputPath));

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(soundData.Channels);
		bw.Write(soundData.SampleRate);
		bw.Write(soundData.BitsPerSample);
		bw.Write(soundData.Data.Length);
		bw.Write(soundData.Data);

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
