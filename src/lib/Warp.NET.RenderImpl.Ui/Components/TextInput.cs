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

	public TextInput(
		IBounds bounds,
		bool isNumeric,
		Action<string>? onEnter,
		Action<string>? onDeselect,
		Action<string>? onInput,
		TextInputStyle textInputStyle)
		: base(bounds, isNumeric, onEnter, onDeselect, onInput)
	{
		TextInputStyle = textInputStyle;
	}

	public TextInputStyle TextInputStyle { get; set; }

	protected override int CharWidth => RenderImplUiBase.Game.GetFontRenderer(TextInputStyle.FontSize).Font.CharWidth;

	protected override int TextRenderingHorizontalOffset => TextInputStyle.TextRenderingHorizontalOffset;

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		int padding = (int)MathF.Round((Bounds.Y2 - Bounds.Y1) / 4f);
		Vector2i<int> borderVec = new(TextInputStyle.BorderSize);

		RenderImplUiBase.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth, IsSelected ? TextInputStyle.ActiveBorderColor : TextInputStyle.BorderColor);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(Bounds.Size - borderVec, Bounds.Center + scrollOffset, Depth + 1, Hover ? TextInputStyle.HoverBackgroundColor : TextInputStyle.BackgroundColor);

		MonoSpaceFontRenderer fontRenderer = RenderImplUiBase.Game.GetFontRenderer(TextInputStyle.FontSize);

		bool hasSelection = KeyboardInput.GetSelectionLength() > 0;
		if (KeyboardInput.CursorPositionStart == KeyboardInput.CursorPositionEnd && KeyboardInput.CursorTimer <= _cursorTimerSwitch && IsSelected || hasSelection)
		{
			int selectionStart = Math.Min(KeyboardInput.CursorPositionStart, KeyboardInput.CursorPositionEnd);
			int cursorSelectionStartX = Bounds.X1 + selectionStart * fontRenderer.Font.CharWidth + padding;

			int cursorWidth = KeyboardInput.GetSelectionLength() * fontRenderer.Font.CharWidth + 1;
			Vector2i<int> cursorPosition = scrollOffset + new Vector2i<int>(cursorSelectionStartX + cursorWidth / 2, Bounds.Center.Y);
			RenderImplUiBase.Game.RectangleRenderer.Schedule(new(cursorWidth, Bounds.Size.Y - borderVec.Y), cursorPosition, Depth + 2, hasSelection ? TextInputStyle.SelectionColor : TextInputStyle.CursorColor);
		}

		Vector2i<int> position = new(Bounds.X1 + padding, Bounds.Y1 + padding);

		fontRenderer.Schedule(Vector2i<int>.One, scrollOffset + position, Depth + 3, TextInputStyle.TextColor, KeyboardInput.Value.ToString(), TextAlign.Left);
	}
}
