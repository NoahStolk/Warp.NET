using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class ScrollViewer<TContent> : AbstractScrollViewer<ScrollViewer<TContent>, TContent>
	where TContent : AbstractScrollContent<TContent, ScrollViewer<TContent>>, IScrollContent<TContent, ScrollViewer<TContent>>
{
	public ScrollViewer(IBounds bounds, IBounds contentBounds, IBounds scrollbarBounds)
		: base(bounds)
	{
		Content = TContent.Construct(contentBounds, this);
		Scrollbar = new Scrollbar(scrollbarBounds, SetScrollPercentage);

		NestingContext.Add(Content);
		NestingContext.Add(Scrollbar);
	}

	public override AbstractScrollbar Scrollbar { get; }
	public override TContent Content { get; }
}
