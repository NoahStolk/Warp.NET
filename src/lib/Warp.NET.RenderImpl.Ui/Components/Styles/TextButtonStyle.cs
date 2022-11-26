using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;

namespace Warp.NET.RenderImpl.Ui.Components.Styles;

public record TextButtonStyle(Color TextColor, TextAlign TextAlign, FontSize FontSize)
{
	public static TextButtonStyle Default { get; } = new(Color.White, TextAlign.Middle, FontSize.H24);
}
