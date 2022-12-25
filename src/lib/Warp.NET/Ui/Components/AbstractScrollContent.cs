using Warp.NET.Extensions;
using Warp.NET.Numerics;

namespace Warp.NET.Ui.Components;

public abstract class AbstractScrollContent<TSelf, TParent> : AbstractComponent
	where TSelf : AbstractScrollContent<TSelf, TParent>
	where TParent : AbstractScrollViewer<TParent, TSelf>
{
	private readonly AbstractScrollViewer<TParent, TSelf> _parent;

	protected AbstractScrollContent(IBounds bounds, AbstractScrollViewer<TParent, TSelf> parent)
		: base(bounds)
	{
		_parent = parent;
	}

	public abstract int ContentHeightInPixels { get; protected set; }

	protected virtual float ScrollPercentageMultiplier => 0.05f;

	public abstract void SetContent();

	public void SetScrollOffset(Vector2i<int> scrollOffset)
	{
		NestingContext.ScrollOffset = Vector2i<int>.Clamp(scrollOffset, new(0, -ContentHeightInPixels + Bounds.Size.Y), default);
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		bool wasActive = MouseUiContext.IsActive;

		base.Update(scrollOffset);

		bool hoverWithoutBlock = wasActive && Bounds.Contains(MouseUiContext.MousePosition.RoundToVector2Int32() - scrollOffset);
		if (!hoverWithoutBlock)
			return;

		int scroll = Input.GetScroll();
		if (scroll != 0)
			_parent.SetScrollPercentage(_parent.Scrollbar.TopPercentage - scroll * ScrollPercentageMultiplier);
	}
}
