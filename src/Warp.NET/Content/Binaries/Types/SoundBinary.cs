using Warp.NET.Content.Parsers;
using Warp.NET.Content.Parsers.Data;

namespace Warp.NET.Content.Binaries.Types;

public class SoundBinary : IBinary
{
	private WaveData? _waveData;

	public ContentType ReadFromPath(string path)
	{
		_waveData = WaveParser.Parse(File.ReadAllBytes(path));
		return ContentType.Sound;
	}

	public byte[] ToBytes(ContentType contentType)
	{
		if (contentType != ContentType.Sound)
			throw new NotSupportedException($"Calling {nameof(SoundBinary)}.{nameof(ToBytes)} with {nameof(ContentType)} '{contentType}' is not supported.");

		if (_waveData == null)
			throw new InvalidOperationException("Binary is not initialized.");

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(_waveData.Channels);
		bw.Write(_waveData.SampleRate);
		bw.Write(_waveData.BitsPerSample);
		bw.Write(_waveData.Data.Length);
		bw.Write(_waveData.Data);
		return ms.ToArray();
	}
}
