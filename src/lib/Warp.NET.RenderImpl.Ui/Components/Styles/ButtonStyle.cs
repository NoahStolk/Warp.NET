using Warp.NET.Numerics;

namespace Warp.NET.RenderImpl.Ui.Components.Styles;

public record ButtonStyle(Color BackgroundColor, Color BorderColor, Color HoverBackgroundColor, int BorderSize)
{
	public static ButtonStyle Default { get; } = new(Color.Black, Color.White, Color.Gray(0.5f), 2);
}
