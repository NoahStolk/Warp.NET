namespace Warp.NET.RenderImpl.Ui.Rendering;

public static class ScissorScheduler
{
	public static Scissor? CurrentScissor { get; private set; }

	/// <summary>
	/// Sets the scissor test for future render calls. Future render calls will be batched with this scissor test.
	/// </summary>
	public static void SetScissor(Scissor scissor)
	{
		CurrentScissor = scissor;
	}

	/// <summary>
	/// Unsets the scissor test for future render calls. Future render calls will not be batched with a scissor test.
	/// </summary>
	public static void UnsetScissor()
	{
		CurrentScissor = null;
	}
}
