using Warp.NET.Numerics;

namespace Warp.NET.RenderImpl.Ui.Components.Styles;

public readonly record struct TextInputStyle(Color BackgroundColor, Color BorderColor, Color HoverBackgroundColor, Color TextColor, Color ActiveBorderColor, Color CursorColor, Color SelectionColor, int BorderSize, int TextRenderingHorizontalOffset)
{
	public int CharWidth => 8;
}
