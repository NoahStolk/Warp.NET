using System.Numerics;
using Warp.NET.Numerics;

namespace Warp.NET.RenderImpl.Ui.Rendering;

public static class ViewportState
{
	public static Viewport Viewport { get; private set; }

	public static Vector2 Offset { get; private set; }

	public static Vector2 Scale { get; private set; }

	public static Vector2 MousePosition => (Input.GetMousePosition() - Offset) / Scale;

	public static void OnChangeWindowSize(int width, int height, GameParameters gameParameters)
	{
		float originalAspectRatio = gameParameters.InitialWindowWidth / (float)gameParameters.InitialWindowHeight;
		float adjustedWidth = height * originalAspectRatio; // Adjusted for aspect ratio
		Offset = new((width - adjustedWidth) / 2, 0);
		Vector2 size = new(adjustedWidth, height); // Fix viewport to maintain aspect ratio
		Scale = size / new Vector2(gameParameters.InitialWindowWidth, gameParameters.InitialWindowHeight);

		Viewport = new((int)Offset.X, (int)Offset.Y, (int)size.X, (int)size.Y);
		Gl.Viewport(Viewport.X, Viewport.Y, (uint)Viewport.Width, (uint)Viewport.Height);
	}
}
