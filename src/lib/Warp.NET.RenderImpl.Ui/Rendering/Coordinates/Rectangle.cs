using Warp.NET.Ui;

namespace Warp.NET.RenderImpl.Ui.Rendering.Coordinates;

public sealed record Rectangle : IBounds
{
	private readonly Fraction _x;
	private readonly Fraction _y;
	private readonly Fraction _width;
	private readonly Fraction _height;

	public Rectangle(Fraction x, Fraction y, Fraction width, Fraction height)
	{
		_x = x;
		_y = y;
		_width = width;
		_height = height;
	}

	public int X1 => (int)(_x.ToFloat() * CurrentWindowState.Width);
	public int Y1 => (int)(_y.ToFloat() * CurrentWindowState.Height);
	public int X2 => X1 + (int)(_width.ToFloat() * CurrentWindowState.Width);
	public int Y2 => Y1 + (int)(_height.ToFloat() * CurrentWindowState.Height);

	public override string ToString()
	{
		return $"{X1},{Y1}..{X2},{Y2}";
	}
}
