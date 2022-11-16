using Warp.NET.Content.Binaries;
using Warp.NET.Extensions;

namespace Warp.NET.Utils;

public static class ColorDataUtils
{
	private static bool IsOpaqueWhite(byte r, byte g, byte b, byte a)
		=> r == byte.MaxValue && g == byte.MaxValue && b == byte.MaxValue && a == byte.MaxValue;

	public static ContentType DetermineTextureContentType(byte[] colorData)
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
			return ContentType.TextureW1;

		if (isOpaque)
			return isGrayScale ? ContentType.TextureW8 : ContentType.TextureRgb24;

		return isGrayScale ? ContentType.TextureWa16 : ContentType.TextureRgba32;
	}

	public static byte[] WriteTextureBinary(ushort width, ushort height, byte[] colorData, ContentType contentType)
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(width);
		bw.Write(height);

		switch (contentType)
		{
			case ContentType.TextureW1:
				BitArray bitArray = new(colorData.Length / 4);
				for (int i = 0; i < colorData.Length; i += 4)
				{
					byte r = colorData[i];
					byte g = colorData[i + 1];
					byte b = colorData[i + 2];
					byte a = colorData[i + 3];
					bitArray.Set(i / 4, IsOpaqueWhite(r, g, b, a));
				}

				bw.Write(bitArray.ToBytes());
				break;
			case ContentType.TextureW8:
				for (int i = 0; i < colorData.Length; i += 4)
					bw.Write(colorData[i]);

				break;
			case ContentType.TextureWa16:
				for (int i = 0; i < colorData.Length; i++)
				{
					if (i % 4 is 0 or 3)
						bw.Write(colorData[i]);
				}

				break;
			case ContentType.TextureRgb24:
				for (int i = 0; i < colorData.Length; i++)
				{
					if (i % 4 != 3)
						bw.Write(colorData[i]);
				}

				break;
			case ContentType.TextureRgba32:
				bw.Write(colorData);
				break;
			default:
				throw new NotSupportedException($"Calling {nameof(WriteTextureBinary)} with {nameof(ContentType)} '{contentType}' is not supported.");
		}

		return ms.ToArray();
	}
}
