namespace Warp.NET.Ui.Components;

public abstract class AbstractScrollViewer<TSelf, TContent> : AbstractComponent
	where TSelf : AbstractScrollViewer<TSelf, TContent>
	where TContent : AbstractScrollContent<TContent, TSelf>
{
	protected AbstractScrollViewer(IBounds bounds)
		: base(bounds)
	{
	}

	public abstract AbstractScrollbar Scrollbar { get; }
	public abstract TContent Content { get; }

	public virtual void InitializeContent()
	{
		Content.SetContent();
		SetThumbPercentageSize();
		SetScrollPercentage(0);
	}

	public void SetThumbPercentageSize()
	{
		Scrollbar.ThumbPercentageSize = Content.ContentHeightInPixels == 0 ? 1 : Math.Clamp(Content.Bounds.Size.Y / (float)Content.ContentHeightInPixels, 0, 1);
	}

	public void SetScrollPercentage(float percentage)
	{
		Content.SetScrollOffset(new(0, (int)MathF.Round(percentage * -Content.ContentHeightInPixels)));
		Scrollbar.TopPercentage = Content.ContentHeightInPixels == 0 ? 1 : -Content.NestingContext.ScrollOffset.Y / (float)Content.ContentHeightInPixels;
	}
}
