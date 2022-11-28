using Warp.NET.Numerics;

namespace Warp.NET.Ui;

public sealed record Bounds(float X, float Y, float Width, float Height)
{
	public int X1 => (int)(X * Graphics.CurrentWindowState.Width);
	public int Y1 => (int)(Y * Graphics.CurrentWindowState.Height);
	public int X2 => X1 + (int)(Width * Graphics.CurrentWindowState.Width);
	public int Y2 => Y1 + (int)(Height * Graphics.CurrentWindowState.Height);
	public Vector2i<int> TopLeft => new(X1, Y1);
	public Vector2i<int> Size => new(X2 - X1, Y2 - Y1);

	public bool Contains(int x, int y)
	{
		return x >= X1 && x <= X2 && y >= Y1 && y <= Y2;
	}

	public bool Contains(Vector2i<int> position)
	{
		return Contains(position.X, position.Y);
	}

	public bool IntersectsOrContains(Bounds other)
	{
		return IntersectsOrContains(other.X1, other.Y1, other.X2, other.Y2);
	}

	public bool IntersectsOrContains(int x1, int y1, int x2, int y2)
	{
		Vector2i<int> a = new(x1, y1);
		Vector2i<int> b = new(x2, y1);
		Vector2i<int> c = new(x1, y2);
		Vector2i<int> d = new(x2, y2);

		return Contains(a) || Contains(b) || Contains(c) || Contains(d);
	}

	public Bounds CreateNested(float x, float y, float width, float height)
	{
		float nestedX = x * Width;
		float nestedY = y * Height;
		float nestedWidth = width * Width;
		float nestedHeight = height * Height;
		return new(X + nestedX, Y + nestedY, nestedWidth, nestedHeight);
	}

	public Vector2 CreateNested(float x, float y)
	{
		float nestedX = x * Width;
		float nestedY = y * Height;
		return new(X + nestedX, Y + nestedY);
	}

	public override string ToString()
	{
		return $"{X1},{Y1}..{X2},{Y2}";
	}
}
