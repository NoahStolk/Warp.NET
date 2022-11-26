namespace Warp.NET.Ui;

public class Layout : ILayout
{
	public Layout()
	{
		NestingContext = new(new ScreenBounds());
	}

	public NestingContext NestingContext { get; }
}
