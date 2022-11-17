using System.Collections.Immutable;
using Warp.NET.Content;
using Warp.NET.Numerics;

namespace Warp.NET.Text;

public class MonoSpaceFont
{
	private readonly string _charset;
	private readonly char _lineBreak;

	public MonoSpaceFont(Texture texture, string charset, char lineBreak = '\n')
	{
		Texture = texture;
		_charset = charset;
		_lineBreak = lineBreak;

		CharAmount = charset.Length;
		CharVertexWidth = 1.0f / CharAmount;

		Vertices = ImmutableArray.Create(stackalloc float[24]
		{
			-0.5f, +0.5f, 0.0f, 1.0f, // top left
			+0.5f, +0.5f, CharVertexWidth, 1.0f, // top right
			-0.5f, -0.5f, 0.0f, 0.0f, // bottom left

			+0.5f, +0.5f, CharVertexWidth, 1.0f, // top right
			+0.5f, -0.5f, CharVertexWidth, 0.0f, // bottom right
			-0.5f, -0.5f, 0.0f, 0.0f, // bottom left
		});
	}

	public Texture Texture { get; }
	public int CharAmount { get; }
	public float CharVertexWidth { get; }

	public ImmutableArray<float> Vertices { get; }

	public float GetTextureOffset(char c) => _charset.IndexOf(c) / (float)CharAmount;

	public Vector2i<int> MeasureText(string text)
	{
		int width = 0;
		int maxWidth = 0;
		int height = 1;
		foreach (char c in text)
		{
			if (c == _lineBreak)
			{
				width = 0;
				height++;
			}
			else
			{
				width++;
				if (maxWidth < width)
					maxWidth = width;
			}
		}

		int charWidth = Texture.Width / CharAmount;
		return new Vector2i<int>(maxWidth, height) * new Vector2i<int>(charWidth, Texture.Height);
	}

	public void AdvancePosition(char c, ref Vector2i<int> relativePosition, int xOrigin, int charWidth, int charHeight)
	{
		if (c == _lineBreak)
		{
			relativePosition.X = xOrigin;
			relativePosition.Y += charHeight;
		}
		else
		{
			relativePosition.X += charWidth;
		}
	}
}
