using Warp.NET.Editor.Components.Styles;
using Warp.NET.Editor.Rendering;
using Warp.NET.Numerics;
using Warp.NET.Text;
using Warp.NET.Ui;

namespace Warp.NET.Editor.Components;

public class TextButton : Button
{
	public TextButton(IBounds bounds, Action onClick, ButtonStyle buttonStyle, TextButtonStyle textButtonStyle, string text)
		: base(bounds, onClick, buttonStyle)
	{
		TextButtonStyle = textButtonStyle;
		Text = text;
	}

	public string Text { get; set; }
	public TextButtonStyle TextButtonStyle { get; set; }

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		if (Text.Length == 0)
			return;

		int padding = (int)MathF.Round((Bounds.Y2 - Bounds.Y1) / 4f);
		Vector2i<int> textPosition = TextButtonStyle.TextAlign switch
		{
			TextAlign.Middle => new Vector2i<int>(Bounds.X1 + Bounds.X2, Bounds.Y1 + Bounds.Y2) / 2,
			TextAlign.Left => new(Bounds.X1 + padding, Bounds.Y1 + padding),
			TextAlign.Right => new(Bounds.X2 - padding, Bounds.Y1 + padding),
			_ => throw new InvalidOperationException("Invalid text align."),
		};

		RenderBatchCollector.RenderMonoSpaceText(Vector2i<int>.One, parentPosition + textPosition, Depth + 2, TextButtonStyle.TextColor, Text, TextButtonStyle.TextAlign);
	}
}
