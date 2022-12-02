namespace Warp.NET.Ui;

public class Layout : ILayout
{
	public Layout()
	{
		NestingContext = new(new Bounds(0, 0, 1, 1));
	}

	public NestingContext NestingContext { get; }
}
