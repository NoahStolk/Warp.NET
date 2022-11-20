using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class Button : AbstractButton
{
	public Button(IBounds bounds, Action onClick, ButtonStyle buttonStyle)
		: base(bounds, onClick)
	{
		ButtonStyle = buttonStyle;
	}

	public ButtonStyle ButtonStyle { get; set; }

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> borderVec = new(ButtonStyle.BorderSize);
		Vector2i<int> scale = Bounds.Size;
		Vector2i<int> topLeft = Bounds.TopLeft;
		Vector2i<int> center = topLeft + scale / 2;

		RenderImplUiBase.Game.RectangleRenderer.Schedule(scale, parentPosition + center, Depth, ButtonStyle.BorderColor);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(scale - borderVec * 2, parentPosition + center, Depth + 1, Hover && !IsDisabled ? ButtonStyle.HoverBackgroundColor : ButtonStyle.BackgroundColor);
	}
}