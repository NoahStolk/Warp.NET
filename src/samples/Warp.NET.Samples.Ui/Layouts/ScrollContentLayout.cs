using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace Warp.NET.Samples.Ui.Layouts;

public class ScrollContentLayout : Layout
{
	public ScrollContentLayout()
	{
		Label titleLabel = new(new PixelBounds(896, 256, 128, 128), "Scroll Content Layout", LabelStyle.Default with { TextColor = Color.Green });
		TextButton backButton = new(new PixelBounds(256, 320, 256, 128), () => Game.Self.ActiveLayout = Game.Self.MainLayout, ButtonStyle.Default, TextButtonStyle.Default, "Back");

		const int scrollbarWidth = 32;
		ScrollArea scrollViewer = new(new PixelBounds(704, 384, 512 + scrollbarWidth, 512));

		for (int i = 0; i < 20; i++)
		{
			const int buttonHeight = 64;
			TextButton button = new(scrollViewer.Bounds.CreateNested(0, i * buttonHeight, 512, buttonHeight), () => { }, ButtonStyle.Default, TextButtonStyle.Default, $"Button {i}") { Depth = scrollViewer.Depth + 1 };
			scrollViewer.NestingContext.Add(button);
		}

		NestingContext.Add(titleLabel);
		NestingContext.Add(backButton);
		NestingContext.Add(scrollViewer);
	}
}
