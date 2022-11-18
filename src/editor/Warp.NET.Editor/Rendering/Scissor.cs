using System.Numerics;
using Warp.NET.Extensions;
using Warp.NET.Numerics;
using Warp.NET.Ui;

namespace Warp.NET.Editor.Rendering;

public record Scissor(int X, int Y, uint Width, uint Height)
{
	public static Scissor Create(IBounds bounds, Vector2i<int> parentPosition, Vector2 viewportOffset, Vector2 uiScale)
	{
		return Create(bounds.X1, bounds.Y1, bounds.Size.X, bounds.Size.Y, parentPosition, viewportOffset, uiScale);
	}

	public static Scissor Create(int x, int y, int w, int h, Vector2i<int> parentPosition, Vector2 viewportOffset, Vector2 uiScale)
	{
		Vector2i<int> topLeft = new(x, y);
		Vector2i<int> size = new(w, h);
		Vector2i<int> scaledSize = (size.ToVector2() * uiScale).RoundToVector2Int32();
		Vector2i<int> scaledTopLeft = (topLeft.ToVector2() * uiScale).RoundToVector2Int32();
		Vector2i<int> scaledParentPosition = (parentPosition.ToVector2() * uiScale).RoundToVector2Int32();
		return new(
			scaledTopLeft.X + (int)viewportOffset.X + scaledParentPosition.X,
			WindowHeight - (scaledSize.Y + scaledParentPosition.Y) - (int)viewportOffset.Y - scaledTopLeft.Y,
			(uint)scaledSize.X,
			(uint)scaledSize.Y);
	}
}
