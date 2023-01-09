namespace Warp.NET.Content.Conversion.Sounds;

/// <summary>
/// Represents data parsed from a sound format, such as a .wav file.
/// </summary>
internal record SoundData(short Channels, int SampleRate, int ByteRate, short BlockAlign, short BitsPerSample, byte[] Data, int SampleCount, double LengthInSeconds);
