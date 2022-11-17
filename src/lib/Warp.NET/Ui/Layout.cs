namespace Warp.NET.Ui;

public class Layout : ILayout
{
	public Layout(IBounds bounds)
	{
		NestingContext = new(bounds);
	}

	public NestingContext NestingContext { get; }
}
