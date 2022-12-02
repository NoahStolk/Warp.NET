namespace Warp.NET.Ui;

/// <summary>
/// An implementation of <see cref="IBounds"/> which uses normalized coordinates. This results in rendering stretched across the entire window.
/// </summary>
/// <param name="X">The normalized X coordinate, between 0 and 1.</param>
/// <param name="Y">The normalized Y coordinate, between 0 and 1.</param>
/// <param name="Width">The normalized width, between 0 and 1.</param>
/// <param name="Height">The normalized height, between 0 and 1.</param>
// TODO: Rename to NormalizedBounds.
public sealed record Bounds(float X, float Y, float Width, float Height) : IBounds
{
	public int X1 => (int)(X * Graphics.CurrentWindowState.Width);
	public int Y1 => (int)(Y * Graphics.CurrentWindowState.Height);
	public int X2 => X1 + (int)(Width * Graphics.CurrentWindowState.Width);
	public int Y2 => Y1 + (int)(Height * Graphics.CurrentWindowState.Height);

	public IBounds CreateNested(int x, int y, int width, int height)
	{
		float nestedX = (int)(x / (float)Graphics.CurrentWindowState.Width) * Width;
		float nestedY = (int)(y / (float)Graphics.CurrentWindowState.Height) * Height;
		float nestedWidth = (int)(width / (float)Graphics.CurrentWindowState.Width) * Width;
		float nestedHeight = (int)(height / (float)Graphics.CurrentWindowState.Height) * Height;
		return new Bounds(X + nestedX, Y + nestedY, nestedWidth, nestedHeight);
	}

	public Vector2 CreateNested(int x, int y)
	{
		float nestedX = (int)(x / (float)Graphics.CurrentWindowState.Width) * Width;
		float nestedY = (int)(y / (float)Graphics.CurrentWindowState.Height) * Height;
		return new(X + nestedX, Y + nestedY);
	}

	public IBounds Move(int x, int y)
	{
		return this with
		{
			X = X + (int)(x / (float)Graphics.CurrentWindowState.Width),
			Y = Y + (int)(y / (float)Graphics.CurrentWindowState.Height),
		};
	}
}
