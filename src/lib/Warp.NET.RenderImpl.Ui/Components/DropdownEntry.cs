using Warp.NET.Numerics;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class DropdownEntry : AbstractDropdownEntry
{
	private readonly string _text;
	private readonly TextAlign _textAlign;

	public DropdownEntry(IBounds bounds, AbstractDropdown parent, Action onClick, string text, TextAlign textAlign = TextAlign.Left)
		: base(bounds, parent, onClick)
	{
		_text = text;
		_textAlign = textAlign;
		Depth = 102;
		IsActive = false;
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> borderVec = new(1);
		Vector2i<int> scale = Bounds.Size;
		Vector2i<int> topLeft = Bounds.TopLeft;
		Vector2i<int> center = topLeft + scale / 2;
		RenderImplUiBase.Game.RectangleRenderer.Schedule(Bounds.Size, parentPosition + center, Depth, Color.White);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(Bounds.Size - borderVec * 2, parentPosition + center, Depth + 1, Hover && !IsDisabled ? Color.Gray(0.5f) : Color.Black);

		int padding = (int)MathF.Round(Bounds.Size.Y / 4f);
		Vector2i<int> textPosition = _textAlign switch
		{
			TextAlign.Middle => new Vector2i<int>(Bounds.X1 + Bounds.X2, Bounds.Y1 + Bounds.Y2) / 2,
			TextAlign.Left => new(Bounds.X1 + padding, Bounds.Y1 + padding),
			TextAlign.Right => new(Bounds.X2 - padding, Bounds.Y1 + padding),
			_ => throw new InvalidOperationException("Invalid text align."),
		};
		RenderImplUiBase.Game.MonoSpaceFontRenderer.Schedule(new(1), parentPosition + textPosition, Depth + 2, Color.White, _text, _textAlign);
	}
}
