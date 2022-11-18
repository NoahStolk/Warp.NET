using Silk.NET.OpenGL;

namespace Warp.NET.Editor.Rendering;

/// <summary>
/// Responsible for setting the scissor test during rendering.
/// </summary>
public static class ScissorActivator
{
	private static Scissor? _cached;

	/// <summary>
	/// Sets the scissor test for rendering.
	/// </summary>
	public static void SetScissor(Scissor? scissor)
	{
		if (_cached == scissor)
			return;

		if (scissor != null)
		{
			Gl.Enable(EnableCap.ScissorTest);
			Gl.Scissor(scissor.X, scissor.Y, scissor.Width, scissor.Height);
		}
		else
		{
			Gl.Disable(EnableCap.ScissorTest);
		}

		_cached = scissor;
	}
}
