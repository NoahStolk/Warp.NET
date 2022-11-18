using Warp.NET.Editor.Components.Styles;
using Warp.NET.Numerics;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.Editor.Components;

public class TextInput : AbstractTextInput
{
	private const float _cursorTimerSwitch = 0.45f;

	private readonly TextInputStyle _textInputStyle;

	public TextInput(
		IBounds bounds,
		bool isNumeric,
		Action<string>? onEnter,
		Action<string>? onDeselect,
		Action<string>? onInput,
		TextInputStyle textInputStyle)
		: base(bounds, isNumeric, onEnter, onDeselect, onInput)
	{
		_textInputStyle = textInputStyle;

		CharWidth = textInputStyle.CharWidth;
		TextRenderingHorizontalOffset = textInputStyle.TextRenderingHorizontalOffset;
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		int padding = (int)MathF.Round((Bounds.Y2 - Bounds.Y1) / 4f);
		Vector2i<int> borderVec = new(_textInputStyle.BorderSize);
		Vector2i<int> scale = new(Bounds.X2 - Bounds.X1, Bounds.Y2 - Bounds.Y1);
		Vector2i<int> center = Bounds.TopLeft + Bounds.Size / 2;

		Game.Self.RectangleRenderer.Schedule(scale, center + parentPosition, Depth, IsSelected ? _textInputStyle.ActiveBorderColor : _textInputStyle.BorderColor);
		Game.Self.RectangleRenderer.Schedule(scale - borderVec, center + parentPosition + borderVec / 2, Depth + 1, Hover ? _textInputStyle.HoverBackgroundColor : _textInputStyle.BackgroundColor);

		bool hasSelection = KeyboardInput.GetSelectionLength() > 0;
		if (KeyboardInput.CursorPositionStart == KeyboardInput.CursorPositionEnd && KeyboardInput.CursorTimer <= _cursorTimerSwitch && IsSelected || hasSelection)
		{
			int selectionStart = Math.Min(KeyboardInput.CursorPositionStart, KeyboardInput.CursorPositionEnd);
			int cursorSelectionStartX = Bounds.X1 + selectionStart * _textInputStyle.CharWidth + padding;

			Vector2i<int> cursorPosition = parentPosition + new Vector2i<int>(cursorSelectionStartX, Bounds.Y1 + borderVec.Y / 2);
			Game.Self.RectangleRenderer.Schedule(new(KeyboardInput.GetSelectionLength() * _textInputStyle.CharWidth + 1, Bounds.Size.Y - borderVec.Y), cursorPosition, Depth + 2, hasSelection ? _textInputStyle.SelectionColor : _textInputStyle.CursorColor);
		}

		Vector2i<int> position = new(Bounds.X1 + padding, Bounds.Y1 + padding);

		Game.Self.MonoSpaceFontRenderer.Schedule(Vector2i<int>.One, parentPosition + position, Depth + 3, _textInputStyle.TextColor, KeyboardInput.Value.ToString(), TextAlign.Left);
	}
}