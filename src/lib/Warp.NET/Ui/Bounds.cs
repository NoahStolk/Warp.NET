using Warp.NET.Numerics;

namespace Warp.NET.Ui;

public sealed record Bounds
{
	private readonly float _x;
	private readonly float _y;
	private readonly float _width;
	private readonly float _height;

	public Bounds(float x, float y, float width, float height)
	{
		// if (x < 0 || y < 0 || width < 0 || height < 0 || x > 1 || y > 1 || width > 1 || height > 1)
		// 	throw new ArgumentException("All parameters must be between 0 and 1.");
		//
		// if (x + width > 1 || y + height > 1)
		// 	throw new ArgumentException("Rectangle is out of bounds.");

		_x = x;
		_y = y;
		_width = width;
		_height = height;
	}

	public int X1 => (int)(_x * Graphics.CurrentWindowState.Width);
	public int Y1 => (int)(_y * Graphics.CurrentWindowState.Height);
	public int X2 => X1 + (int)(_width * Graphics.CurrentWindowState.Width);
	public int Y2 => Y1 + (int)(_height * Graphics.CurrentWindowState.Height);
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
		float nestedX = x * _width;
		float nestedY = y * _height;
		float nestedWidth = width * _width;
		float nestedHeight = height * _height;
		return new(_x + nestedX, _y + nestedY, nestedWidth, nestedHeight);
	}

	public override string ToString()
	{
		return $"{X1},{Y1}..{X2},{Y2}";
	}
}
