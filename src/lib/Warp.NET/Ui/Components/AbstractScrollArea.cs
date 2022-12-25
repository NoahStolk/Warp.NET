using Silk.NET.GLFW;
using Warp.NET.Extensions;
using Warp.NET.Numerics;

namespace Warp.NET.Ui.Components;

public abstract class AbstractScrollArea : AbstractComponent
{
	private readonly int _scrollAmountInPixels;

	private int _holdStartMouseY;
	private int _scrollbarStartYOld;

	private int _contentHeight;

	protected AbstractScrollArea(IBounds bounds, int scrollAmountInPixels, int scrollbarWidth)
		: base(bounds)
	{
		_scrollAmountInPixels = scrollAmountInPixels;

		ContentBounds = Bounds.CreateNested(0, 0, bounds.Size.X - scrollbarWidth, bounds.Size.Y);
		ScrollbarBounds = Bounds.CreateNested(bounds.Size.X - scrollbarWidth, 0, scrollbarWidth, bounds.Size.Y);

		NestingContext.OnUpdateQueue = RecalculateHeight;
	}

	public bool IsScrollbarHovering { get; private set; }
	public bool IsDraggingScrollbar { get; private set; }

	public int ScrollbarStartY { get; private set; }
	public int ScrollbarHeight { get; private set; }

	public IBounds ContentBounds { get; }
	public IBounds ScrollbarBounds { get; }

	public void RecalculateHeight()
	{
		int min = NestingContext.OrderedComponents.Count == 0 ? 0 : NestingContext.OrderedComponents.Min(b => b.Bounds.Y1);
		int max = NestingContext.OrderedComponents.Count == 0 ? 0 : NestingContext.OrderedComponents.Max(b => b.Bounds.Y2);
		_contentHeight = max - min;
		ScrollbarHeight = _contentHeight == 0 ? ScrollbarBounds.Size.Y : Math.Min((int)(ContentBounds.Size.Y / (float)_contentHeight * ScrollbarBounds.Size.Y), ContentBounds.Size.Y);

		UpdateScrollOffsetAndScrollbarPosition(NestingContext.ScrollOffset);
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		bool wasActive = MouseUiContext.IsActive;

		base.Update(scrollOffset);

		HandleScrollWheel(scrollOffset, wasActive);
		HandleScrollbar(scrollOffset);
	}

	public void UpdateScrollOffsetAndScrollbarPosition(Vector2i<int> newScrollOffset)
	{
		// Update and clamp scroll offset.
		NestingContext.ScrollOffset = Vector2i<int>.Clamp(newScrollOffset, new(0, -_contentHeight + Bounds.Size.Y), default);

		// Update scrollbar position.
		ScrollbarStartY = (int)(NestingContext.ScrollOffset.Y / (float)-_contentHeight * ScrollbarBounds.Size.Y);
	}

	private void HandleScrollWheel(Vector2i<int> scrollOffset, bool wasActive)
	{
		bool hoverWithoutBlock = wasActive && ContentBounds.Contains(MouseUiContext.MousePosition.RoundToVector2Int32() - scrollOffset);
		if (!hoverWithoutBlock)
			return;

		int scroll = Input.GetScroll();
		if (scroll != 0)
		{
			Vector2i<int> newOffset = new(0, NestingContext.ScrollOffset.Y + scroll * _scrollAmountInPixels);
			UpdateScrollOffsetAndScrollbarPosition(newOffset);
		}
	}

	private void HandleScrollbar(Vector2i<int> scrollOffset)
	{
		IsScrollbarHovering = MouseUiContext.Contains(scrollOffset, ScrollbarBounds);

		if (IsScrollbarHovering && Input.IsButtonPressed(MouseButton.Left))
		{
			int mousePos = TranslatedMousePosition(scrollOffset.Y);
			if (mousePos > ScrollbarStartY)
			{
				if (mousePos < ScrollbarStartY + ScrollbarHeight)
				{
					IsDraggingScrollbar = true;
					_holdStartMouseY = mousePos;
					_scrollbarStartYOld = ScrollbarStartY;
				}
				else
				{
					UpdateScrollOffsetAndScrollbarPosition(new(0, NestingContext.ScrollOffset.Y - _scrollAmountInPixels));
				}
			}
			else
			{
				UpdateScrollOffsetAndScrollbarPosition(new(0, NestingContext.ScrollOffset.Y + _scrollAmountInPixels));
			}
		}
		else if (IsDraggingScrollbar)
		{
			UpdateScrollOffsetFromDrag();
		}

		if (IsDraggingScrollbar && Input.IsButtonReleased(MouseButton.Left))
		{
			if (IsScrollbarHovering)
				UpdateScrollOffsetFromDrag();

			IsDraggingScrollbar = false;
			_scrollbarStartYOld = ScrollbarStartY;
		}

		void UpdateScrollOffsetFromDrag()
		{
			int mouseDifferenceWithStartDrag = TranslatedMousePosition(scrollOffset.Y) - _holdStartMouseY;
			float multiplier = _contentHeight / (float)ScrollbarBounds.Size.Y;
			UpdateScrollOffsetAndScrollbarPosition(new(0, (int)((-_scrollbarStartYOld - mouseDifferenceWithStartDrag) * multiplier)));
		}

		int TranslatedMousePosition(int scrollOffsetY)
			=> (int)MouseUiContext.MousePosition.Y - Bounds.Y1 - scrollOffsetY;
	}
}
