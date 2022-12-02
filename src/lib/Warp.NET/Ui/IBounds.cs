using Warp.NET.Numerics;

namespace Warp.NET.Ui;

/// <summary>
/// Represents a "box" used for UI positioning. All values are in pixels. // TODO: Rename parameters to "xInPixels" etc.
/// </summary>
public interface IBounds
{
	int X1 { get; }
	int Y1 { get; }
	int X2 { get; }
	int Y2 { get; }

	public Vector2i<int> TopLeft => new(X1, Y1);
	public Vector2i<int> Size => new(X2 - X1, Y2 - Y1);
	public Vector2i<int> Center => new(X1 + (X2 - X1) / 2, Y1 + (Y2 - Y1) / 2);

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

	IBounds CreateNested(int x, int y, int width, int height);

	Vector2 CreateNested(int x, int y);

	IBounds Move(int x, int y);
}
