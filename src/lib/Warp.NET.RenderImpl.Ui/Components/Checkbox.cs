using Warp.NET.Numerics;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class Checkbox : AbstractCheckbox
{
	public Checkbox(NormalizedBounds bounds, Action<bool> onClick)
		: base(bounds, onClick)
	{
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		const int margin = 8;
		const int border = 4;
		const int borderTick = 16;

		Vector2i<int> marginVec = new(margin);
		Vector2i<int> borderVec = new(border);
		Vector2i<int> borderTickVec = new(borderTick);
		Vector2i<int> fullScale = new(Bounds.X2 - Bounds.X1, Bounds.Y2 - Bounds.Y1);
		Vector2i<int> topLeft = new(Bounds.X1, Bounds.Y1);
		Vector2i<int> center = topLeft + fullScale / 2;
		Vector2i<int> scale = fullScale - marginVec;

		RenderImplUiBase.Game.RectangleRenderer.Schedule(scale + borderVec, scrollOffset + center, Depth, Color.White);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(scale, scrollOffset + center, Depth + 1, Hover ? Color.Gray(0.25f) : Color.Black);

		if (CurrentValue)
			RenderImplUiBase.Game.RectangleRenderer.Schedule(scale - borderTickVec, scrollOffset + center, Depth + 2, Color.Gray(0.75f));
	}
}
