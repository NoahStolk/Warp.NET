using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class Label : AbstractLabel
{
	public Label(IBounds bounds, string text, LabelStyle labelStyle)
		: base(bounds, text)
	{
		LabelStyle = labelStyle;
	}

	public LabelStyle LabelStyle { get; set; }

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		int padding = (int)MathF.Round((Bounds.Y2 - Bounds.Y1) / 4f);
		Vector2i<int> textPosition = LabelStyle.TextAlign switch
		{
			TextAlign.Middle => new Vector2i<int>(Bounds.X1 + Bounds.X2, Bounds.Y1 + Bounds.Y2) / 2,
			TextAlign.Left => new(Bounds.X1 + padding, Bounds.Y1 + padding),
			TextAlign.Right => new(Bounds.X2 - padding, Bounds.Y1 + padding),
			_ => throw new InvalidOperationException("Invalid text align."),
		};

		RenderImplUiBase.Game.GetFontRenderer(LabelStyle.FontSize).Schedule(Vector2i<int>.One, parentPosition + textPosition, Depth + 2, LabelStyle.TextColor, Text, LabelStyle.TextAlign);
	}
}
