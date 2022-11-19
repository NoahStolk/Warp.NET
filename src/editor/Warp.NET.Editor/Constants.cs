using Warp.NET.RenderImpl.Ui.Rendering;
using Warp.NET.RenderImpl.Ui.Rendering.Coordinates;

namespace Warp.NET.Editor;

public static class Constants
{
	public const int InitialWindowWidth = 1920;
	public const int InitialWindowHeight = 1080;

	public static Grid GridDefault { get; } = new(InitialWindowWidth, InitialWindowHeight);

	public static Rectangle RectangleFull { get; } = new(0, 0, 1, 1, GridDefault);
}
