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

	public bool ScrollbarHover { get; private set; }
	public bool ScrollbarHold { get; private set; }

	public int ScrollbarStartY { get; private set; }
	public int ScrollbarHeight { get; private set; }

	public IBounds ContentBounds { get; }
	public IBounds ScrollbarBounds { get; }

	private void RecalculateHeight()
	{
		int min = NestingContext.OrderedComponents.Count == 0 ? 0 : NestingContext.OrderedComponents.Min(b => b.Bounds.Y1);
		int max = NestingContext.OrderedComponents.Count == 0 ? 0 : NestingContext.OrderedComponents.Max(b => b.Bounds.Y2);
		_contentHeight = max - min;
		ScrollbarHeight = _contentHeight == 0 ? ScrollbarBounds.Size.Y : (int)(ContentBounds.Size.Y / (float)_contentHeight * ScrollbarBounds.Size.Y);
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		bool wasActive = MouseUiContext.IsActive;

		base.Update(scrollOffset);

		HandleScrollWheel(scrollOffset, wasActive);
		HandleScrollbar(scrollOffset);
	}

	private void UpdateScrollOffset(Vector2i<int> newOffset)
	{
		// Clamp value.
		NestingContext.ScrollOffset = Vector2i<int>.Clamp(newOffset, new(0, -_contentHeight + Bounds.Size.Y), default);

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
			UpdateScrollOffset(newOffset);
		}
	}

	private void HandleScrollbar(Vector2i<int> scrollOffset)
	{
		ScrollbarHover = MouseUiContext.Contains(scrollOffset, ScrollbarBounds);

		if (ScrollbarHover && Input.IsButtonPressed(MouseButton.Left))
		{
			int mousePos = TranslatedMousePosition(scrollOffset.Y);
			if (mousePos > ScrollbarStartY)
			{
				if (mousePos < ScrollbarStartY + ScrollbarHeight)
				{
					ScrollbarHold = true;
					_holdStartMouseY = mousePos;
					_scrollbarStartYOld = ScrollbarStartY;
				}
				else
				{
					UpdateScrollOffset(new(0, NestingContext.ScrollOffset.Y - _scrollAmountInPixels));
				}
			}
			else
			{
				UpdateScrollOffset(new(0, NestingContext.ScrollOffset.Y + _scrollAmountInPixels));
			}
		}
		else if (ScrollbarHold)
		{
			UpdateValue();
		}

		if (ScrollbarHold && Input.IsButtonReleased(MouseButton.Left))
		{
			if (ScrollbarHover)
				UpdateValue();

			ScrollbarHold = false;
			_scrollbarStartYOld = ScrollbarStartY;
		}

		void UpdateValue()
		{
			int yDiff = TranslatedMousePosition(scrollOffset.Y) - _holdStartMouseY;
			float multiplier = _contentHeight / (float)ScrollbarBounds.Size.Y;
			UpdateScrollOffset(new(0, (int)((-_scrollbarStartYOld - yDiff) * multiplier)));
		}

		int TranslatedMousePosition(int scrollOffsetY)
			=> (int)MouseUiContext.MousePosition.Y - Bounds.Y1 - scrollOffsetY;
	}
}
