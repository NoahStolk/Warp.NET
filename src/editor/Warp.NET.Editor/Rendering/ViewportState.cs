using System.Numerics;

namespace Warp.NET.Editor.Rendering;

public static class ViewportState
{
	public static Vector2 Offset { get; private set; }

	public static Vector2 Scale { get; private set; }

	public static Vector2 MousePosition => (Input.GetMousePosition() - Offset) / Scale;

	public static void OnChangeWindowSize(int width, int height)
	{
		const float originalAspectRatio = Constants.InitialWindowWidth / (float)Constants.InitialWindowHeight;
		float adjustedWidth = height * originalAspectRatio; // Adjusted for aspect ratio
		Offset = new((width - adjustedWidth) / 2, 0);
		Vector2 size = new(adjustedWidth, height); // Fix viewport to maintain aspect ratio
		Scale = size / new Vector2(Constants.InitialWindowWidth, Constants.InitialWindowHeight);

		Gl.Viewport((int)Offset.X, (int)Offset.Y, (uint)size.X, (uint)size.Y);
	}
}
