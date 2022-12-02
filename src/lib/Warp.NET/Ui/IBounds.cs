using Warp.NET.Numerics;

namespace Warp.NET.Ui;

/// <summary>
/// Represents a "box" used for UI positioning. All values are in pixels.
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

	public bool Contains(int xInPixels, int yInPixels)
	{
		return xInPixels >= X1 && xInPixels <= X2 && yInPixels >= Y1 && yInPixels <= Y2;
	}

	public bool Contains(Vector2i<int> positionInPixels)
	{
		return Contains(positionInPixels.X, positionInPixels.Y);
	}

	public bool IntersectsOrContains(IBounds other)
	{
		return IntersectsOrContains(other.X1, other.Y1, other.X2, other.Y2);
	}

	public bool IntersectsOrContains(int x1InPixels, int y1InPixels, int x2Pixels, int y2Pixels)
	{
		Vector2i<int> a = new(x1InPixels, y1InPixels);
		Vector2i<int> b = new(x2Pixels, y1InPixels);
		Vector2i<int> c = new(x1InPixels, y2Pixels);
		Vector2i<int> d = new(x2Pixels, y2Pixels);

		return Contains(a) || Contains(b) || Contains(c) || Contains(d);
	}

	IBounds CreateNested(int xInPixels, int yInPixels, int widthInPixels, int heightInPixels);

	Vector2 CreateNested(int xInPixels, int yInPixels);

	IBounds Move(int xInPixels, int yInPixels);
}
