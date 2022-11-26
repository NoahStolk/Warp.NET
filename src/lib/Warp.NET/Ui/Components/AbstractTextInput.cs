using Silk.NET.GLFW;
using Warp.NET.Numerics;

namespace Warp.NET.Ui.Components;

public abstract class AbstractTextInput : AbstractComponent
{
	private readonly Action<string>? _onDeselect;

	protected AbstractTextInput(
		Bounds bounds,
		bool isNumeric,
		Action<string>? onEnter = null,
		Action<string>? onDeselect = null,
		Action<string>? onInput = null)
		: base(bounds)
	{
		KeyboardInput = new(isNumeric, onEnter, onInput);
		_onDeselect = onDeselect;
	}

	public KeyboardInput KeyboardInput { get; }

	public bool IsSelected { get; protected set; }

	protected bool Hover { get; private set; }

	protected int CharWidth { get; set; }
	protected int TextRenderingHorizontalOffset { get; set; }

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		Hover = MouseUiContext.Contains(parentPosition, Bounds);

		if (Input.IsButtonPressed(MouseButton.Left))
		{
			bool wasSelected = IsSelected;
			IsSelected = Hover;
			if (wasSelected && !IsSelected)
			{
				// Trigger change on component deselect.
				_onDeselect?.Invoke(KeyboardInput.Value.ToString());
				KeyboardInput.CursorPositionStart = 0;
				KeyboardInput.CursorPositionEnd = 0;
			}
			else
			{
				int absoluteMousePositionX = (int)MouseUiContext.MousePosition.X - parentPosition.X - Bounds.X1;
				KeyboardInput.CursorPositionStart = GetIndexFromAbsoluteMousePositionX(absoluteMousePositionX);
				KeyboardInput.CursorPositionEnd = KeyboardInput.CursorPositionStart;
				KeyboardInput.CursorTimer = 0;
			}
		}

		if (!IsSelected)
		{
			KeyboardInput.CursorTimer = 0;
			return;
		}

		if (Input.IsButtonHeld(MouseButton.Left))
		{
			int absoluteMousePositionX = (int)MouseUiContext.MousePosition.X - parentPosition.X - Bounds.X1;
			KeyboardInput.CursorPositionEnd = GetIndexFromAbsoluteMousePositionX(absoluteMousePositionX);
		}

		KeyboardInput.Update();

		int GetIndexFromAbsoluteMousePositionX(int absoluteMousePositionX)
			=> Math.Clamp((int)MathF.Round((absoluteMousePositionX - TextRenderingHorizontalOffset) / (float)CharWidth), 0, KeyboardInput.Value.Length);
	}
}
