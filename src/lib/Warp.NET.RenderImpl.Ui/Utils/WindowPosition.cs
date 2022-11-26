using Warp.NET.Numerics;

namespace Warp.NET.RenderImpl.Ui.Utils;

public static class WindowPosition
{
	public static Vector2i<int> Get(float x, float y)
	{
		return new((int)(x * CurrentWindowState.Width), (int)(y * CurrentWindowState.Height));
	}
}
