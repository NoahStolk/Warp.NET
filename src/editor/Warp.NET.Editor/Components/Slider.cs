using Warp.NET.Numerics;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.Editor.Components;

public class Slider : AbstractSlider
{
	private readonly Color _textColor;

	public Slider(IBounds bounds, Action<float> onChange, bool applyInstantly, float min, float max, float step, float defaultValue, int border, Color textColor)
		: base(bounds, onChange, applyInstantly, min, max, step, defaultValue)
	{
		Border = border;
		_textColor = textColor;
	}

	protected int Border { get; }

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> borderVec = new(Border);
		Vector2i<int> scale = new(Bounds.X2 - Bounds.X1, Bounds.Y2 - Bounds.Y1);
		Vector2i<int> topLeft = new(Bounds.X1, Bounds.Y1);
		Vector2i<int> center = topLeft + scale / 2;

		Game.Self.RectangleRenderer.Schedule(scale, parentPosition + center, Depth, Color.White);
		Game.Self.RectangleRenderer.Schedule(scale - borderVec, parentPosition + center, Depth + 1, Hold ? Color.Gray(0.5f) : Hover ? Color.Gray(0.25f) : Color.Black);

		Vector2i<int> centerPosition = new Vector2i<int>(Bounds.X1 + Bounds.X2, Bounds.Y1 + Bounds.Y2) / 2;
		Game.Self.MonoSpaceFontRenderer.Schedule(Vector2i<int>.One, parentPosition + centerPosition, Depth + 3, _textColor, CurrentValue.ToString("0.00"), TextAlign.Middle);
	}
}