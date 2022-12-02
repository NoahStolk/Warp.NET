using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class ScrollViewer<TContent> : AbstractScrollViewer<ScrollViewer<TContent>, TContent>
	where TContent : AbstractScrollContent<TContent, ScrollViewer<TContent>>, IScrollContent<TContent, ScrollViewer<TContent>>
{
	public ScrollViewer(IBounds bounds, int scrollbarWidth)
		: base(bounds)
	{
		int division = bounds.Size.X - scrollbarWidth;
		Content = TContent.Construct(bounds.CreateNested(0, 0, division, bounds.Size.Y), this);
		Scrollbar = new Scrollbar(bounds.CreateNested(division, 0, scrollbarWidth, bounds.Size.Y), SetScrollPercentage);

		NestingContext.Add(Content);
		NestingContext.Add(Scrollbar);
	}

	public override AbstractScrollbar Scrollbar { get; }
	public override TContent Content { get; }
}
