using Warp.NET.Editor.Rendering;
using Warp.NET.Numerics;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.Editor.Components;

public abstract class ScrollContent<TSelf, TParent> : AbstractScrollContent<TSelf, TParent>
	where TSelf : AbstractScrollContent<TSelf, TParent>
	where TParent : AbstractScrollViewer<TParent, TSelf>
{
	protected ScrollContent(IBounds bounds, AbstractScrollViewer<TParent, TSelf> parent)
		: base(bounds, parent)
	{
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		ScissorScheduler.SetScissor(Scissor.Create(Bounds, parentPosition, ViewportState.Offset, ViewportState.Scale));

		base.Render(parentPosition);

		Game.Self.RectangleRenderer.Schedule(Bounds.Size, parentPosition + Bounds.TopLeft + Bounds.Size / 2, Depth, Color.Black);

		ScissorScheduler.UnsetScissor();
	}
}
