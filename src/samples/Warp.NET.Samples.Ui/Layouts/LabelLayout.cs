using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;
using Warp.NET.Ui;

namespace Warp.NET.Samples.Ui.Layouts;

public class LabelLayout : Layout
{
	public LabelLayout()
	{
		LabelStyle labelStyle = new(Color.Green, TextAlign.Middle, FontSize.H16);
		ButtonStyle buttonStyle = new(Color.Black, Color.White, Color.Gray(0.5f), 2);
		TextButtonStyle textButtonStyle = new(Color.White, TextAlign.Middle, FontSize.H24);

		Label titleLabel = new(new PixelBounds(960 - 64, 256, 128, 128), "Label Layout", labelStyle);
		TextButton backButton = new(new PixelBounds(256, 320, 256, 128), () => Game.Self.ActiveLayout = Game.Self.MainLayout, buttonStyle, textButtonStyle, "Back");

		NestingContext.Add(titleLabel);
		NestingContext.Add(backButton);

		Label cutOffLabel = new(new PixelBounds(960 - 128, 512, 256, 128), "This is a very long label that will be cut off.", labelStyle)
		{
			RenderOverflow = false,
		};
		NestingContext.Add(cutOffLabel);

		Label normalLabel = new(new PixelBounds(960 - 128, 576, 256, 128), "This is a very long label that will not be cut off.", labelStyle);
		NestingContext.Add(normalLabel);
	}
}
