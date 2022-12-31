using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;
using Warp.NET.Ui;

namespace Warp.NET.Samples.Ui.Layouts;

public class ScrollContentLayout : Layout
{
	public ScrollContentLayout()
	{
		LabelStyle labelStyle = new(Color.Green, TextAlign.Left, FontSize.H16);
		ButtonStyle buttonStyle = new(Color.Black, Color.White, Color.Gray(0.5f), 2);
		TextButtonStyle textButtonStyle = new(Color.White, TextAlign.Middle, FontSize.H24);

		Label titleLabel = new(new PixelBounds(896, 256, 128, 128), "Scroll Content Layout", labelStyle);
		TextButton backButton = new(new PixelBounds(256, 320, 256, 128), () => Game.Self.ActiveLayout = Game.Self.MainLayout, buttonStyle, textButtonStyle, "Back");

		const int scrollbarWidth = 32;
		ScrollAreaStyle style = new(2, 2, Color.White, Color.Gray(0.75f), Color.Black, Color.Gray(0.5f), Color.Gray(0.25f));
		ScrollArea scrollViewer = new(new PixelBounds(704, 384, 512 + scrollbarWidth, 512), 96, 32, style);

		for (int i = 0; i < 20; i++)
		{
			const int buttonHeight = 64;
			TextButton button = new(scrollViewer.Bounds.CreateNested(0, i * buttonHeight, 512, buttonHeight), () => { }, buttonStyle, textButtonStyle, $"Button {i}") { Depth = scrollViewer.Depth + 1 };
			scrollViewer.NestingContext.Add(button);
		}

		NestingContext.Add(titleLabel);
		NestingContext.Add(backButton);
		NestingContext.Add(scrollViewer);
	}
}
