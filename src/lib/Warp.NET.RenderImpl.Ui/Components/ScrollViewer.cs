using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class ScrollViewer<TContent> : AbstractScrollViewer<ScrollViewer<TContent>, TContent>
	where TContent : AbstractScrollContent<TContent, ScrollViewer<TContent>>, IScrollContent<TContent, ScrollViewer<TContent>>
{
	public ScrollViewer(Bounds bounds)
		: base(bounds)
	{
		Content = TContent.Construct(bounds.CreateNested(0, 0, 0.95f, 1), this);
		Scrollbar = new Scrollbar(Bounds.CreateNested(0.95f, 0, 0.05f, 1), SetScrollPercentage);

		NestingContext.Add(Content);
		NestingContext.Add(Scrollbar);
	}

	public override AbstractScrollbar Scrollbar { get; }
	public override TContent Content { get; }
}
