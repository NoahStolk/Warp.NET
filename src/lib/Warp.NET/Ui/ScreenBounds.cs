namespace Warp.NET.Ui;

public class ScreenBounds : IBounds
{
	public int X1 => 0;
	public int Y1 => 0;
	public int X2 => Graphics.CurrentWindowState.Width;
	public int Y2 => Graphics.CurrentWindowState.Height;
}
