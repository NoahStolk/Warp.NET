using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;
using Warp.NET.Ui;

namespace Warp.NET.Samples.Ui.Layouts;

public class MainLayout : Layout
{
	public MainLayout()
	{
		LabelStyle labelStyle = new(Color.Yellow, TextAlign.Middle, FontSize.H16);
		ButtonStyle buttonStyle = new(Color.Black, Color.White, Color.Gray(0.5f), 2);
		TextButtonStyle textButtonStyle = new(Color.White, TextAlign.Middle, FontSize.H24);

		Label titleLabel = new(new PixelBounds(960 - 64, 256, 128, 128), "Main Layout", labelStyle);
		TextButton scrollContentButton = new(new PixelBounds(256, 320, 256, 128), () => Game.Self.ActiveLayout = Game.Self.ScrollContentLayout, buttonStyle, textButtonStyle, "ScrollContent");
		TextButton labelButton = new(new PixelBounds(512, 320, 256, 128), () => Game.Self.ActiveLayout = Game.Self.LabelLayout, buttonStyle, textButtonStyle, "Label");

		NestingContext.Add(titleLabel);
		NestingContext.Add(scrollContentButton);
		NestingContext.Add(labelButton);
	}
}
