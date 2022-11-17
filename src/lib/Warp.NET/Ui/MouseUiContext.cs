using Warp.NET.Extensions;
using Warp.NET.Numerics;

namespace Warp.NET.Ui;

public static class MouseUiContext
{
	/// <summary>
	/// Determines whether the context is still active.
	/// This is used to prevent hovering issues with overlapping components.
	/// </summary>
	public static bool IsActive { get; private set; } = true;

	public static Vector2 MousePosition { get; private set; }

	private static void Deactivate()
	{
		IsActive = false;
	}

	public static bool Contains(Vector2i<int> parentPosition, IBounds bounds)
	{
		if (!IsActive || !bounds.Contains(MousePosition.RoundToVector2Int32() - parentPosition))
			return false;

		Deactivate();
		return true;
	}

	public static void Reset(Vector2 mousePosition)
	{
		IsActive = true;
		MousePosition = mousePosition;
	}
}
