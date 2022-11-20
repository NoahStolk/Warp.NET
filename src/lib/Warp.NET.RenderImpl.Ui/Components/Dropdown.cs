using Warp.NET.Numerics;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class Dropdown : AbstractDropdown
{
	private readonly string _text;

	public Dropdown(IBounds bounds, string text)
		: base(bounds)
	{
		_text = text;
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> borderVec = new(1);
		Vector2i<int> scale = Bounds.Size;
		Vector2i<int> topLeft = Bounds.TopLeft;
		Vector2i<int> center = topLeft + scale / 2;
		RenderImplUiBase.Game.RectangleRenderer.Schedule(Bounds.Size, parentPosition + center, Depth, Color.White);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(Bounds.Size - borderVec * 2, parentPosition + center, Depth + 1, Hover ? Color.Gray(0.5f) : Color.Black);

		Vector2i<int> textPosition = new Vector2i<int>(Bounds.X1 + Bounds.X2, Bounds.Y1 + Bounds.Y2) / 2;
		RenderImplUiBase.Game.MonoSpaceFontRenderer.Schedule(new(1), parentPosition + textPosition, Depth + 2, Color.White, _text, TextAlign.Middle);
	}
}
