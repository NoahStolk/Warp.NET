using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public interface IScrollContent<out TSelf, in TParent>
	where TSelf : AbstractScrollContent<TSelf, TParent>, IScrollContent<TSelf, TParent>
	where TParent : AbstractScrollViewer<TParent, TSelf>
{
	static abstract TSelf Construct(Bounds bounds, TParent parent);
}
