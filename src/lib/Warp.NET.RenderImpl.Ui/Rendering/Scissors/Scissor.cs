using Warp.NET.Extensions;
using Warp.NET.Numerics;
using Warp.NET.Ui;

namespace Warp.NET.RenderImpl.Ui.Rendering.Scissors;

public record Scissor(int X, int Y, uint Width, uint Height)
{
	public static Scissor Create(IBounds bounds, Vector2i<int> scrollOffset)
	{
		return Create(bounds.X1, bounds.Y1, bounds.Size.X, bounds.Size.Y, scrollOffset);
	}

	public static Scissor Create(int x, int y, int w, int h, Vector2i<int> scrollOffset)
	{
		Vector2i<int> topLeft = new(x, y);
		Vector2i<int> size = new(w, h);
		Vector2i<int> scaledSize = size.ToVector2().RoundToVector2Int32();
		Vector2i<int> scaledTopLeft = topLeft.ToVector2().RoundToVector2Int32();
		Vector2i<int> scaledScrollOffset = scrollOffset.ToVector2().RoundToVector2Int32();
		return new(
			scaledTopLeft.X + scaledScrollOffset.X,
			CurrentWindowState.Height - (scaledSize.Y + scaledScrollOffset.Y) - scaledTopLeft.Y,
			(uint)scaledSize.X,
			(uint)scaledSize.Y);
	}
}
