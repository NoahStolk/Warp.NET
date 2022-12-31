using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace Warp.NET.Editor.Layouts;

public class MainLayout : Layout
{
	public MainLayout()
	{
		ButtonStyle buttonStyle = new(Color.Black, Color.White, Color.Gray(0.5f), 2);
		Button button = new(new NormalizedBounds(0.1f, 0.1f, 0.1f, 0.1f), () => { }, buttonStyle);
		NestingContext.Add(button);
	}
}
