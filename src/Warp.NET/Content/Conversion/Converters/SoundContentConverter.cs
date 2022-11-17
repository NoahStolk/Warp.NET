using Warp.NET.Content.Conversion.Binaries;
using Warp.NET.Content.Conversion.Data;
using Warp.NET.Content.Conversion.Parsers;

namespace Warp.NET.Content.Conversion.Converters;

public sealed class SoundContentConverter : IContentConverter<SoundBinary>
{
	public static SoundBinary Construct(string inputPath)
	{
		SoundData soundData = WaveParser.Parse(File.ReadAllBytes(inputPath));
		return new(soundData.Channels, soundData.SampleRate, soundData.BitsPerSample, soundData.Data);
	}
}
