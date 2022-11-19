using System.Numerics;
using Warp.NET.Numerics;
using Warp.NET.Ui;

namespace Warp.NET.RenderImpl.Ui.Rendering.Coordinates;

public sealed record Rectangle : IBounds
{
	public Rectangle(Vector2 position, Vector2 size, Grid grid)
		: this(position.X, position.Y, size.X, size.Y, grid)
	{
	}

	public Rectangle(float x, float y, float width, float height, Grid grid)
	{
		X1 = (int)(x * grid.Width);
		Y1 = (int)(y * grid.Height);
		X2 = X1 + (int)(width * grid.Width);
		Y2 = Y1 + (int)(height * grid.Height);
	}

	public int X1 { get; }
	public int Y1 { get; }
	public int X2 { get; }
	public int Y2 { get; }

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
}
