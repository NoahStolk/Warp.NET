using Warp.NET.RenderImpl.Ui.Rendering.Coordinates;

namespace Warp.NET.Editor;

public static class Constants
{
	public const int InitialWindowWidth = 1920;
	public const int InitialWindowHeight = 1080;

	public static Grid GridDefault { get; } = new(InitialWindowWidth, InitialWindowHeight);

	public static Rectangle RectangleFull { get; } = new(Fraction.F00_01, Fraction.F00_01, Fraction.F01_01, Fraction.F01_01, GridDefault);
}
