using Warp.NET.Content.Conversion.Data;
using Warp.NET.Content.Conversion.Parsers;
using Warp.NET.Extensions;

namespace Warp.NET.Content.Conversion.Converters;

public record TextureContentConverter(byte[] Contents) : IContentConverter<TextureContentConverter>
{
	public ContentType ContentType => ContentType.Texture;

	public static TextureContentConverter Construct(string inputPath)
	{
		TextureData textureData = TgaParser.Parse(File.ReadAllBytes(inputPath));
		TextureContentType textureContentType = DetermineTextureContentType(textureData.ColorData);

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write((byte)textureContentType);
		bw.Write(textureData.Width);
		bw.Write(textureData.Height);

		switch (textureContentType)
		{
			case TextureContentType.W1:
				BitArray bitArray = new(textureData.ColorData.Length / 4);
				for (int i = 0; i < textureData.ColorData.Length; i += 4)
				{
					byte r = textureData.ColorData[i];
					byte g = textureData.ColorData[i + 1];
					byte b = textureData.ColorData[i + 2];
					byte a = textureData.ColorData[i + 3];
					bitArray.Set(i / 4, IsOpaqueWhite(r, g, b, a));
				}

				bw.Write(bitArray.ToBytes());
				break;
			case TextureContentType.W8:
				for (int i = 0; i < textureData.ColorData.Length; i += 4)
					bw.Write(textureData.ColorData[i]);

				break;
			case TextureContentType.Wa16:
				for (int i = 0; i < textureData.ColorData.Length; i++)
				{
					if (i % 4 is 0 or 3)
						bw.Write(textureData.ColorData[i]);
				}

				break;
			case TextureContentType.Rgb24:
				for (int i = 0; i < textureData.ColorData.Length; i++)
				{
					if (i % 4 != 3)
						bw.Write(textureData.ColorData[i]);
				}

				break;
			case TextureContentType.Rgba32:
				bw.Write(textureData.ColorData);
				break;
			default:
				throw new NotSupportedException($"{nameof(TextureContentType)} '{textureContentType}' is not supported.");
		}

		return new(ms.ToArray());
	}

	public static Texture Deconstruct(BinaryReader br)
	{
		TextureContentType textureContentType = (TextureContentType)br.ReadByte();
		return textureContentType switch
		{
			TextureContentType.W1 => ConstructTextureW1(br),
			TextureContentType.W8 => ConstructTextureW8(br),
			TextureContentType.Wa16 => ConstructTextureWa16(br),
			TextureContentType.Rgb24 => ConstructTextureRgb24(br),
			TextureContentType.Rgba32 => ConstructTextureRgba32(br),
			_ => throw new NotSupportedException($"{nameof(TextureContentType)} '{textureContentType}' is not supported."),
		};
	}

	private static Texture ConstructTextureW1(BinaryReader br)
	{
		ushort width = br.ReadUInt16();
		ushort height = br.ReadUInt16();

		int colorDataSize = (int)Math.Ceiling(width * height / 8f);
		byte[] colorBuffer = br.ReadBytes(colorDataSize);
		BitArray binaryColors = new(colorBuffer);

		byte[] colors = new byte[width * height * 4];
		for (int i = 0; i < colors.Length / 4; i++)
		{
			byte unifiedValue = binaryColors[i] ? (byte)0xFF : (byte)0x00;
			for (int j = 0; j < 4; j++)
				colors[i * 4 + j] = unifiedValue;
		}

		return new(width, height, colors);
	}

	private static Texture ConstructTextureW8(BinaryReader br)
	{
		ushort width = br.ReadUInt16();
		ushort height = br.ReadUInt16();

		byte[] colors = new byte[width * height * 4];
		for (int i = 0; i < colors.Length; i += 4)
		{
			byte colorComponent = br.ReadByte();
			colors[i] = colorComponent;
			colors[i + 1] = colorComponent;
			colors[i + 2] = colorComponent;
			colors[i + 3] = 0xFF;
		}

		return new(width, height, colors);
	}

	private static Texture ConstructTextureWa16(BinaryReader br)
	{
		ushort width = br.ReadUInt16();
		ushort height = br.ReadUInt16();

		byte[] colors = new byte[width * height * 4];
		for (int i = 0; i < colors.Length; i += 4)
		{
			byte colorComponent = br.ReadByte();
			colors[i] = colorComponent;
			colors[i + 1] = colorComponent;
			colors[i + 2] = colorComponent;
			colors[i + 3] = br.ReadByte();
		}

		return new(width, height, colors);
	}

	private static Texture ConstructTextureRgb24(BinaryReader br)
	{
		ushort width = br.ReadUInt16();
		ushort height = br.ReadUInt16();

		byte[] colors = new byte[width * height * 4];
		for (int i = 0; i < colors.Length; i++)
			colors[i] = i % 4 == 3 ? (byte)0xFF : br.ReadByte();

		return new(width, height, colors);
	}

	private static Texture ConstructTextureRgba32(BinaryReader br)
	{
		ushort width = br.ReadUInt16();
		ushort height = br.ReadUInt16();
		return new(width, height, br.ReadBytes(width * height * 4));
	}

	private static TextureContentType DetermineTextureContentType(byte[] colorData)
	{
		bool isWhiteOrTransparentOnly = true;
		bool isOpaque = true;
		bool isGrayScale = true;
		for (int i = 0; i < colorData.Length; i += 4)
		{
			byte r = colorData[i];
			byte g = colorData[i + 1];
			byte b = colorData[i + 2];
			byte a = colorData[i + 3];

			if (isOpaque && a < byte.MaxValue)
				isOpaque = false;

			if (isWhiteOrTransparentOnly && a != 0 && !IsOpaqueWhite(r, g, b, a))
				isWhiteOrTransparentOnly = false;

			if (isGrayScale && (r != g || r != b || g != b))
				isGrayScale = false;

			if (!isOpaque && !isWhiteOrTransparentOnly && !isGrayScale)
				break;
		}

		if (isWhiteOrTransparentOnly)
			return TextureContentType.W1;

		if (isOpaque)
			return isGrayScale ? TextureContentType.W8 : TextureContentType.Rgb24;

		return isGrayScale ? TextureContentType.Wa16 : TextureContentType.Rgba32;
	}

	private static bool IsOpaqueWhite(byte r, byte g, byte b, byte a)
		=> r == byte.MaxValue && g == byte.MaxValue && b == byte.MaxValue && a == byte.MaxValue;
}
