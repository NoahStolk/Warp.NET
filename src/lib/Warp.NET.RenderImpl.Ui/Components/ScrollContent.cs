using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Rendering;
using Warp.NET.RenderImpl.Ui.Rendering.Scissors;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public abstract class ScrollContent<TSelf, TParent> : AbstractScrollContent<TSelf, TParent>
	where TSelf : AbstractScrollContent<TSelf, TParent>, IScrollContent<TSelf, TParent>
	where TParent : AbstractScrollViewer<TParent, TSelf>
{
	protected ScrollContent(IBounds bounds, TParent parent)
		: base(bounds, parent)
	{
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		ScissorScheduler.SetScissor(Scissor.Create(Bounds, scrollOffset, ViewportState.Offset, ViewportState.Scale));

		base.Render(scrollOffset);

		ScissorScheduler.UnsetScissor();
	}
}
