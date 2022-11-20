using Warp.NET.Numerics;

namespace Warp.NET.RenderImpl.Ui.Rendering.Coordinates;

public record Grid(int PixelWidth, int PixelHeight)
{
	public Vector2i<int> Get(Fraction x, Fraction y)
	{
		return new((int)(x.ToFloat() * PixelWidth), (int)(y.ToFloat() * PixelHeight));
	}
}
