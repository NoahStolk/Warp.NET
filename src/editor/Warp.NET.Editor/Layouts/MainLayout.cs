using Warp.NET.Editor.Components;
using Warp.NET.Editor.Components.Styles;
using Warp.NET.Editor.Rendering;
using Warp.NET.Ui;

namespace Warp.NET.Editor.Layouts;

public class MainLayout : Layout
{
	public MainLayout()
		: base(Rectangle.Full)
	{
		Button button = new(new Rectangle(0.1f, 0.1f, 0.1f, 0.1f), () => {}, ButtonStyle.Default);
		NestingContext.Add(button);
	}
}
