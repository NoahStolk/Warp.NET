using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Coordinates;
using Warp.NET.Ui;

namespace Warp.NET.Samples.Ui.Layouts;

public class MainLayout : Layout
{
	public MainLayout()
	{
		Label titleLabel = new(new Rectangle(0.45f, 0.2f, 0.1f, 0.1f), "Main Layout", LabelStyle.Default with { TextColor = Color.Yellow });
		TextButton scrollContentButton = new(new Rectangle(0.1f, 0.3f, 0.1f, 0.1f), () => Game.Self.ActiveLayout = Game.Self.ScrollContentLayout, ButtonStyle.Default, TextButtonStyle.Default, "ScrollContent");

		NestingContext.Add(titleLabel);
		NestingContext.Add(scrollContentButton);
	}
}
