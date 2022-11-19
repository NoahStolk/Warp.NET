using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering;
using Warp.NET.Ui;

namespace Warp.NET.Editor.Layouts;

public class MainLayout : Layout
{
	public MainLayout()
		: base(Constants.RectangleFull)
	{
		Button button = new(new Rectangle(0.1f, 0.1f, 0.1f, 0.1f, Constants.GridDefault), () => {}, ButtonStyle.Default);
		NestingContext.Add(button);
	}
}
