using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering;
using Warp.NET.RenderImpl.Ui.Rendering.Scissors;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class ScrollArea : AbstractScrollArea
{
	public ScrollArea(IBounds bounds, int scrollAmountInPixels, int scrollbarWidth, ScrollAreaStyle scrollAreaStyle)
		: base(bounds, scrollAmountInPixels, scrollbarWidth)
	{
		ScrollAreaStyle = scrollAreaStyle;
	}

	public ScrollAreaStyle ScrollAreaStyle { get; set; }

	public override void Render(Vector2i<int> scrollOffset)
	{
		// Render content.
		ScissorScheduler.SetScissor(Scissor.Create(ContentBounds, scrollOffset, ViewportState.Offset, ViewportState.Scale));

		base.Render(scrollOffset);

		ScissorScheduler.UnsetScissor();

		// Render scrollbar.
		Vector2i<int> borderVec = new(ScrollAreaStyle.BorderSize * 2);
		Vector2i<int> scale = ScrollbarBounds.Size;

		RenderImplUiBase.Game.RectangleRenderer.Schedule(scale, scrollOffset + ScrollbarBounds.Center, Depth, ScrollAreaStyle.BorderColor);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(scale - borderVec, scrollOffset + ScrollbarBounds.Center, Depth + 1, IsDraggingScrollbar ? ScrollAreaStyle.DraggingBackgroundColor : IsScrollbarHovering ? ScrollAreaStyle.HoveringBackgroundColor : ScrollAreaStyle.BackgroundColor);

		int thumbPadding = (ScrollAreaStyle.BorderSize + ScrollAreaStyle.ThumbPadding) * 2;
		Vector2i<int> thumbScale = new(scale.X - thumbPadding, ScrollbarHeight - thumbPadding + 1); // + 1 is necessary for some reason.
		Vector2i<int> thumbOffset = new(0, ScrollbarStartY);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(thumbScale, scrollOffset + new Vector2i<int>(ScrollbarBounds.Center.X, ScrollbarBounds.TopLeft.Y + thumbScale.Y / 2 + thumbPadding / 2) + thumbOffset, Depth + 2, ScrollAreaStyle.ThumbColor);
	}
}
