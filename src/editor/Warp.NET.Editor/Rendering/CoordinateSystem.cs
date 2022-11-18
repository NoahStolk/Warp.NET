using System.Numerics;
using Warp.NET.Numerics;

namespace Warp.NET.Editor.Rendering;

public static class CoordinateSystem
{
	public static Vector2i<int> Get(Vector2 position)
	{
		return Get(position.X, position.Y);
	}

	public static Vector2i<int> Get(float x, float y)
	{
		return new((int)(x * Game.Self.InitialWindowWidth), (int)(y * Game.Self.InitialWindowHeight));
	}
}
