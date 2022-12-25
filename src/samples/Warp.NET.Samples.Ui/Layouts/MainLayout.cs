using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace Warp.NET.Samples.Ui.Layouts;

public class MainLayout : Layout
{
	public MainLayout()
	{
		Label titleLabel = new(new PixelBounds(960, 256, 128, 128), "Main Layout", LabelStyle.Default with { TextColor = Color.Yellow });
		TextButton scrollContentButton = new(new PixelBounds(256, 320, 256, 128), () => Game.Self.ActiveLayout = Game.Self.ScrollContentLayout, ButtonStyle.Default, TextButtonStyle.Default, "ScrollContent");

		NestingContext.Add(titleLabel);
		NestingContext.Add(scrollContentButton);
	}
}
