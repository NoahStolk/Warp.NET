using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Rendering;
using Warp.NET.RenderImpl.Ui.Rendering.Scissors;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class ScrollArea : AbstractScrollArea
{
	public ScrollArea(IBounds bounds, int scrollAmountInPixels, int scrollbarWidth)
		: base(bounds, scrollAmountInPixels, scrollbarWidth)
	{
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		// Render content.
		ScissorScheduler.SetScissor(Scissor.Create(ContentBounds, scrollOffset, ViewportState.Offset, ViewportState.Scale));

		base.Render(scrollOffset);

		ScissorScheduler.UnsetScissor();

		// Render scrollbar.
		Vector2i<int> borderVec = new(4);
		Vector2i<int> scale = ScrollbarBounds.Size;

		RenderImplUiBase.Game.RectangleRenderer.Schedule(scale, scrollOffset + ScrollbarBounds.Center, Depth, Color.White);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(scale - borderVec, scrollOffset + ScrollbarBounds.Center, Depth + 1, IsDraggingScrollbar ? Color.Gray(0.5f) : IsScrollbarHovering ? Color.Gray(0.25f) : Color.Black);

		const int thumbPadding = 8;
		Vector2i<int> thumbScale = new(scale.X - thumbPadding, ScrollbarHeight - thumbPadding + 1); // + 1 is necessary for some reason.
		Vector2i<int> thumbOffset = new(0, ScrollbarStartY);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(thumbScale, scrollOffset + new Vector2i<int>(ScrollbarBounds.Center.X, ScrollbarBounds.TopLeft.Y + thumbScale.Y / 2 + thumbPadding / 2) + thumbOffset, Depth + 2, Color.Gray(0.75f));
	}
}
