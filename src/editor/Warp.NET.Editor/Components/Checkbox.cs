using Warp.NET.Numerics;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.Editor.Components;

public class Checkbox : AbstractCheckbox
{
	public Checkbox(IBounds bounds, Action<bool> onClick)
		: base(bounds, onClick)
	{
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

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

		Game.Self.RectangleRenderer.Schedule(scale + borderVec, parentPosition + center, Depth, Color.White);
		Game.Self.RectangleRenderer.Schedule(scale, parentPosition + center, Depth + 1, Hover ? Color.Gray(0.25f) : Color.Black);

		if (CurrentValue)
			Game.Self.RectangleRenderer.Schedule(scale - borderTickVec, parentPosition + center, Depth + 2, Color.Gray(0.75f));
	}
}
