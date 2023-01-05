namespace Warp.NET.Content.Conversion.Sounds;

public static class WaveParser
{
	private const string _riffHeader = "RIFF";
	private const string _formatHeader = "WAVE";
	private const string _fmtHeader = "fmt ";
	private const string _dataHeader = "data";
	private const int _fmtMinimumSize = 16;
	private const int _audioFormat = 1;

	public static SoundData Parse(byte[] fileContents)
	{
		using MemoryStream ms = new(fileContents);
		using BinaryReader br = new(ms);
		string riffHeader = Encoding.Default.GetString(br.ReadBytes(4));
		if (riffHeader != _riffHeader)
			throw new WaveParseException($"Expected '{_riffHeader}' header (got '{riffHeader}').");

		_ = br.ReadInt32(); // Amount of bytes remaining at this point (after these 4).

		string format = Encoding.Default.GetString(br.ReadBytes(4));
		if (format != _formatHeader)
			throw new WaveParseException($"Expected '{_formatHeader}' header (got '{format}').");

		string fmtHeader = Encoding.Default.GetString(br.ReadBytes(4));
		if (fmtHeader != _fmtHeader)
			throw new WaveParseException($"Expected '{_fmtHeader}' header (got '{fmtHeader}').");

		int fmtSize = br.ReadInt32();
		if (fmtSize < _fmtMinimumSize)
			throw new WaveParseException($"Expected FMT data chunk size to be at least {_fmtMinimumSize} (got {fmtSize}).");

		short audioFormat = br.ReadInt16();
		if (audioFormat != _audioFormat)
			throw new WaveParseException($"Expected audio format to be {_audioFormat} (got {audioFormat}).");

		short channels = br.ReadInt16();
		int sampleRate = br.ReadInt32();
		int byteRate = br.ReadInt32();
		short blockAlign = br.ReadInt16();
		short bitsPerSample = br.ReadInt16();

		br.BaseStream.Seek(_fmtMinimumSize - fmtSize, SeekOrigin.Current);

		int expectedByteRate = sampleRate * channels * bitsPerSample / 8;
		int expectedBlockAlign = channels * bitsPerSample / 8;
		if (byteRate != expectedByteRate)
			throw new WaveParseException($"Expected byte rate to be {expectedByteRate} (got {byteRate}).");
		if (blockAlign != expectedBlockAlign)
			throw new WaveParseException($"Expected block align to be {expectedBlockAlign} (got {blockAlign}).");

		for (long i = br.BaseStream.Position; i < br.BaseStream.Length - (_dataHeader.Length + sizeof(int)); i += 4)
		{
			string dataHeader = Encoding.Default.GetString(br.ReadBytes(4));
			if (dataHeader != _dataHeader)
				continue;

			int dataSize = br.ReadInt32();
			byte[] data = br.ReadBytes(dataSize);

			int sampleCount = dataSize / (bitsPerSample / 8) / channels;
			double lengthInSeconds = sampleCount / (double)sampleRate;
			return new(channels, sampleRate, byteRate, blockAlign, bitsPerSample, data, sampleCount, lengthInSeconds);
		}

		throw new WaveParseException($"Could not find '{_dataHeader}' header.");
	}
}
