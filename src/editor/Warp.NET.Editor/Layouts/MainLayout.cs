using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Coordinates;
using Warp.NET.Ui;

namespace Warp.NET.Editor.Layouts;

public class MainLayout : Layout
{
	public MainLayout()
	{
		Button button = new(new Rectangle(Fraction.F01_10, Fraction.F01_10, Fraction.F01_10, Fraction.F01_10), () => {}, ButtonStyle.Default);
		NestingContext.Add(button);
	}
}
