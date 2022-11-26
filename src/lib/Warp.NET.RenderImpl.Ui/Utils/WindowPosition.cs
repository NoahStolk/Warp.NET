using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Rendering.Coordinates;

namespace Warp.NET.RenderImpl.Ui.Utils;

public static class WindowPosition
{
	public static Vector2i<int> Get(Fraction x, Fraction y)
	{
		return Get(x.ToFloat(), y.ToFloat());
	}

	public static Vector2i<int> Get(float x, float y)
	{
		return new((int)(x * CurrentWindowState.Width), (int)(y * CurrentWindowState.Height));
	}
}
