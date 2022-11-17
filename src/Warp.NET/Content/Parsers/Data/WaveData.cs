namespace Warp.NET.Content.Parsers.Data;

public record WaveData(short Channels, int SampleRate, int ByteRate, short BlockAlign, short BitsPerSample, byte[] Data, int SampleCount, double LengthInSeconds);
