using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace Warp.NET.Editor.Layouts;

public class MainLayout : Layout
{
	public MainLayout()
	{
		Button button = new(new(0.1f, 0.1f, 0.1f, 0.1f), () => {}, ButtonStyle.Default);
		NestingContext.Add(button);
	}
}
