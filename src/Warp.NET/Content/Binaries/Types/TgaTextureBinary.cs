using Warp.NET.Utils;

namespace Warp.NET.Content.Binaries.Types;

public class TgaTextureBinary : IBinary
{
	private byte[]? _colorData; // Rows, top to bottom, left to right

	public ushort Width { get; private set; }
	public ushort Height { get; private set; }

	public ContentType ReadFromPath(string path)
	{
		using FileStream fs = new(path, FileMode.Open);
		using BinaryReader br = new(fs);

		// Header
		byte idLength = br.ReadByte();
		byte colorMapType = br.ReadByte();
		if (colorMapType != 0)
			throw new NotSupportedException($"TGA with color map type {colorMapType} is not supported.");

		byte imageType = br.ReadByte();
		if (imageType != 2)
			throw new NotSupportedException($"TGA with image type {imageType} is not supported.");

		// Color map spec
		_ = br.ReadUInt16(); // Index of first entry
		ushort colorMapLength = br.ReadUInt16();
		_ = br.ReadByte(); // Entry size

		// Image spec
		_ = br.ReadUInt16(); // Origin X
		_ = br.ReadUInt16(); // Origin Y
		Width = br.ReadUInt16();
		Height = br.ReadUInt16();
		byte pixelDepth = br.ReadByte();
		byte imageDescriptor = br.ReadByte();
		bool rightToLeft = BitUtils.IsBitSet(imageDescriptor, 4);
		bool topToBottom = BitUtils.IsBitSet(imageDescriptor, 5);

		// Skip image ID and color map.
		br.BaseStream.Seek(idLength, SeekOrigin.Current);
		br.BaseStream.Seek(colorMapLength, SeekOrigin.Current);

		// Image data
		_colorData = pixelDepth switch
		{
			32 => ReadRgba(br, rightToLeft, topToBottom),
			24 => ReadRgb(br, rightToLeft, topToBottom),
			_ => throw new NotSupportedException($"TGA with pixel depth {pixelDepth} is not supported."),
		};

		return ColorDataUtils.DetermineTextureContentType(_colorData);
	}

	private byte[] ReadRgba(BinaryReader br, bool rightToLeft, bool topToBottom)
	{
		byte[] encodedPixels = br.ReadBytes(Width * Height * 4);

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		for (int i = topToBottom ? 0 : Height - 1; topToBottom ? i < Height : i >= 0; i += topToBottom ? 1 : -1)
		{
			for (int j = rightToLeft ? Width - 1 : 0; rightToLeft ? j >= 0 : j < Width; j += rightToLeft ? -1 : 1)
			{
				int pixelIndex = (i * Width + j) * 4;
				bw.Write(encodedPixels[pixelIndex + 2]); // R
				bw.Write(encodedPixels[pixelIndex + 1]); // G
				bw.Write(encodedPixels[pixelIndex + 0]); // B
				bw.Write(encodedPixels[pixelIndex + 3]); // A
			}
		}

		return ms.ToArray();
	}

	private byte[] ReadRgb(BinaryReader br, bool rightToLeft, bool topToBottom)
	{
		byte[] encodedPixels = br.ReadBytes(Width * Height * 3);

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		for (int i = topToBottom ? 0 : Height - 1; topToBottom ? i < Height : i >= 0; i += topToBottom ? 1 : -1)
		{
			for (int j = rightToLeft ? Width - 1 : 0; rightToLeft ? j >= 0 : j < Width; j += rightToLeft ? -1 : 1)
			{
				int pixelIndex = (i * Width + j) * 3;
				bw.Write(encodedPixels[pixelIndex + 2]); // R
				bw.Write(encodedPixels[pixelIndex + 1]); // G
				bw.Write(encodedPixels[pixelIndex + 0]); // B
				bw.Write((byte)0xFF);
			}
		}

		return ms.ToArray();
	}

	public byte[] ToBytes(ContentType contentType)
	{
		if (_colorData == null)
			throw new InvalidOperationException("Binary is not initialized.");

		return ColorDataUtils.WriteTextureBinary(Width, Height, _colorData, contentType);
	}
}
