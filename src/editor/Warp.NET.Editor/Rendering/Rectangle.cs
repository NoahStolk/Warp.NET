using System.Numerics;
using Warp.NET.Numerics;
using Warp.NET.Ui;

namespace Warp.NET.Editor.Rendering;

public sealed record Rectangle : IBounds
{
	public Rectangle(Vector2 position, Vector2 size)
		: this(position.X, position.Y, size.X, size.Y)
	{
	}

	public Rectangle(float x, float y, float width, float height)
	{
		X1 = (int)(x * Constants.InitialWindowWidth);
		Y1 = (int)(y * Constants.InitialWindowHeight);
		X2 = X1 + (int)(width * Constants.InitialWindowWidth);
		Y2 = Y1 + (int)(height * Constants.InitialWindowHeight);
	}

	public int X1 { get; }
	public int Y1 { get; }
	public int X2 { get; }
	public int Y2 { get; }

	public Vector2i<int> TopLeft => new(X1, Y1);

	public Vector2i<int> Size => new(X2 - X1, Y2 - Y1);

	public static Rectangle Full { get; } = new(Vector2.Zero, Vector2.One);

	public bool Contains(int x, int y)
	{
		return x >= X1 && x <= X2 && y >= Y1 && y <= Y2;
	}

	public bool Contains(Vector2i<int> position)
	{
		return Contains(position.X, position.Y);
	}

	public bool IntersectsOrContains(IBounds other)
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

	public override string ToString()
	{
		return $"{X1},{Y1}..{X2},{Y2}";
	}

	public static IBounds Move(IBounds original, Vector2i<int> offset)
	{
		return new Rectangle(original.X1 + offset.X, original.Y1 + offset.Y, original.X2 + offset.X, original.Y2 + offset.Y);
	}
}