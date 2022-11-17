using Warp.NET.Content.Binaries.Data;
using Warp.NET.Content.Binaries.Parsers;

namespace Warp.NET.Content.Binaries.Types;

public record SoundBinary(ContentType ContentType, byte[] Contents) : IBinary<SoundBinary>
{
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

		return new(ContentType.Sound, ms.ToArray());
	}
}
