using NoahStolk.WaveParser;

namespace Warp.NET.Content.Binaries.Types;

public class SoundBinary : IBinary
{
	private short _channels;
	private int _sampleRate;
	private short _bitsPerSample;
	private byte[]? _data;

	public ContentType ReadFromPath(string path)
	{
		WaveData waveData = new(File.ReadAllBytes(path));
		_channels = waveData.Channels;
		_sampleRate = waveData.SampleRate;
		_bitsPerSample = waveData.BitsPerSample;
		_data = waveData.Data;
		return ContentType.Sound;
	}

	public byte[] ToBytes(ContentType contentType)
	{
		if (contentType != ContentType.Sound)
			throw new NotSupportedException($"Calling {nameof(SoundBinary)}.{nameof(ToBytes)} with {nameof(ContentType)} '{contentType}' is not supported.");

		if (_data == null)
			throw new InvalidOperationException("Binary is not initialized.");

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(_channels);
		bw.Write(_sampleRate);
		bw.Write(_bitsPerSample);
		bw.Write(_data.Length);
		bw.Write(_data);
		return ms.ToArray();
	}
}
