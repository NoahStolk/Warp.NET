using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Renderers;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class TextInput : AbstractTextInput
{
	private const float _cursorTimerSwitch = 0.45f;

	private TextInputStyle _textInputStyle;

	public TextInput(
		Bounds bounds,
		bool isNumeric,
		Action<string>? onEnter,
		Action<string>? onDeselect,
		Action<string>? onInput,
		TextInputStyle textInputStyle)
		: base(bounds, isNumeric, onEnter, onDeselect, onInput)
	{
		_textInputStyle = textInputStyle;
	}

	public TextInputStyle TextInputStyle
	{
		get => _textInputStyle;
		set
		{
			CharWidth = RenderImplUiBase.Game.GetFontRenderer(value.FontSize).Font.CharWidth;
			TextRenderingHorizontalOffset = value.TextRenderingHorizontalOffset;
			_textInputStyle = value;
		}
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		int padding = (int)MathF.Round((Bounds.Y2 - Bounds.Y1) / 4f);
		Vector2i<int> borderVec = new(TextInputStyle.BorderSize);
		Vector2i<int> center = Bounds.TopLeft + Bounds.Size / 2;

		RenderImplUiBase.Game.RectangleRenderer.Schedule(Bounds.Size, center + parentPosition, Depth, IsSelected ? TextInputStyle.ActiveBorderColor : TextInputStyle.BorderColor);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(Bounds.Size - borderVec, center + parentPosition, Depth + 1, Hover ? TextInputStyle.HoverBackgroundColor : TextInputStyle.BackgroundColor);

		MonoSpaceFontRenderer fontRenderer = RenderImplUiBase.Game.GetFontRenderer(TextInputStyle.FontSize);

		bool hasSelection = KeyboardInput.GetSelectionLength() > 0;
		if (KeyboardInput.CursorPositionStart == KeyboardInput.CursorPositionEnd && KeyboardInput.CursorTimer <= _cursorTimerSwitch && IsSelected || hasSelection)
		{
			int selectionStart = Math.Min(KeyboardInput.CursorPositionStart, KeyboardInput.CursorPositionEnd);
			int cursorSelectionStartX = Bounds.X1 + selectionStart * fontRenderer.Font.CharWidth + padding;

			int cursorWidth = KeyboardInput.GetSelectionLength() * fontRenderer.Font.CharWidth + 1;
			Vector2i<int> cursorPosition = parentPosition + new Vector2i<int>(cursorSelectionStartX + cursorWidth / 2, center.Y);
			RenderImplUiBase.Game.RectangleRenderer.Schedule(new(cursorWidth, Bounds.Size.Y - borderVec.Y), cursorPosition, Depth + 2, hasSelection ? TextInputStyle.SelectionColor : TextInputStyle.CursorColor);
		}

		Vector2i<int> position = new(Bounds.X1 + padding, Bounds.Y1 + padding);

		fontRenderer.Schedule(Vector2i<int>.One, parentPosition + position, Depth + 3, TextInputStyle.TextColor, KeyboardInput.Value.ToString(), TextAlign.Left);
	}
}
