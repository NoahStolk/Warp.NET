using Warp.NET.Ui;

namespace Warp.NET.RenderImpl.Ui.Rendering.Coordinates;

public sealed record Rectangle : IBounds
{
	private readonly float _x;
	private readonly float _y;
	private readonly float _width;
	private readonly float _height;

	public Rectangle(Fraction x, Fraction y, Fraction width, Fraction height)
		: this(x.ToFloat(), y.ToFloat(), width.ToFloat(), height.ToFloat())
	{
	}

	public Rectangle(float x, float y, float width, float height)
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

	public int X1 => (int)(_x * CurrentWindowState.Width);
	public int Y1 => (int)(_y * CurrentWindowState.Height);
	public int X2 => X1 + (int)(_width * CurrentWindowState.Width);
	public int Y2 => Y1 + (int)(_height * CurrentWindowState.Height);

	public override string ToString()
	{
		return $"{X1},{Y1}..{X2},{Y2}";
	}
}
