namespace Warp.NET.Ui;

/// <summary>
/// An implementation of <see cref="IBounds"/> which uses pixel coordinates. This results in pixel-perfect rendering.
/// </summary>
/// <param name="X">The X coordinate in pixels.</param>
/// <param name="Y">The Y coordinate in pixels.</param>
/// <param name="Width">The width in pixels.</param>
/// <param name="Height">The height in pixels.</param>
public sealed record PixelBounds(int X, int Y, int Width, int Height) : IBounds
{
	public int X1 => X;
	public int Y1 => Y;
	public int X2 => X + Width;
	public int Y2 => Y + Height;

	public IBounds CreateNested(int x, int y, int width, int height)
	{
		return new PixelBounds(X + x, Y + y, width, height);
	}

	public Vector2 CreateNested(int x, int y)
	{
		return new(X + x, Y + y);
	}

	public IBounds Move(int x, int y)
	{
		return this with
		{
			X = X + x,
			Y = Y + y,
		};
	}
}
