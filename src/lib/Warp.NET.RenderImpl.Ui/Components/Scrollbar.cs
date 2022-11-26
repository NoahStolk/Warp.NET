using Warp.NET.Numerics;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class Scrollbar : AbstractScrollbar
{
	public Scrollbar(Bounds bounds, Action<float> onChange)
		: base(bounds, onChange)
	{
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		const int border = 4;

		Vector2i<int> borderVec = new(border);
		Vector2i<int> scale = Bounds.Size;
		Vector2i<int> center = Bounds.TopLeft + Bounds.Size / 2;

		Color thumbColor = Color.Gray(0.75f);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(scale, parentPosition + center, Depth, thumbColor);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(scale - borderVec, parentPosition + center, Depth + 1, Hold ? Color.Gray(0.5f) : Hover ? Color.Gray(0.25f) : Color.Black);

		// TODO: Refactor when easily testable using sample.
		const int thumbPadding = 4;
		Vector2i<int> thumbScale = new(scale.X - thumbPadding, (int)MathF.Ceiling(scale.Y * ThumbPercentageSize) - thumbPadding + 1); // + 1 needed for scaled UI for some reason.
		float percentageForRendering = Math.Clamp(TopPercentage, 0, 1 - ThumbPercentageSize) - 0.5f + ThumbPercentageSize / 2;

		RenderImplUiBase.Game.RectangleRenderer.Schedule(thumbScale, parentPosition + center + new Vector2i<int>(0, (int)MathF.Round(percentageForRendering * scale.Y)), Depth + 2, thumbColor);
	}
}
