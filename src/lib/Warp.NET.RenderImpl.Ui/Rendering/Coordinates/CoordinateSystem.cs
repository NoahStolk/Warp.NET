using System.Numerics;
using Warp.NET.Numerics;

namespace Warp.NET.RenderImpl.Ui.Rendering.Coordinates;

public static class CoordinateSystem
{
	public static Vector2i<int> Get(Vector2 position)
	{
		return Get(position.X, position.Y);
	}

	public static Vector2i<int> Get(float x, float y)
	{
		return new((int)(x * WarpBase.Game.InitialWindowWidth), (int)(y * WarpBase.Game.InitialWindowHeight));
	}
}
