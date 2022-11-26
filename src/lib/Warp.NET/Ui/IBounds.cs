using Warp.NET.Numerics;

namespace Warp.NET.Ui;

public interface IBounds
{
	int X1 { get; }

	int Y1 { get; }

	int X2 { get; }

	int Y2 { get; }

	public Vector2i<int> TopLeft => new(X1, Y1);

	public Vector2i<int> Size => new(X2 - X1, Y2 - Y1);
}

public static class BoundsExtensions
{
	public static bool Contains(this IBounds bounds, int x, int y)
	{
		return x >= bounds.X1 && x <= bounds.X2 && y >= bounds.Y1 && y <= bounds.Y2;
	}

	public static bool Contains(this IBounds bounds, Vector2i<int> position)
	{
		return Contains(bounds, position.X, position.Y);
	}

	public static bool IntersectsOrContains(this IBounds other)
	{
		return IntersectsOrContains(other, other.X1, other.Y1, other.X2, other.Y2);
	}

	public static bool IntersectsOrContains(this IBounds other, int x1, int y1, int x2, int y2)
	{
		Vector2i<int> a = new(x1, y1);
		Vector2i<int> b = new(x2, y1);
		Vector2i<int> c = new(x1, y2);
		Vector2i<int> d = new(x2, y2);

		return Contains(other, a) || Contains(other, b) || Contains(other, c) || Contains(other, d);
	}
}
