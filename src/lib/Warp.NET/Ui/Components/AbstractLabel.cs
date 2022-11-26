namespace Warp.NET.Ui.Components;

public abstract class AbstractLabel : AbstractComponent
{
	protected AbstractLabel(Bounds bounds, string text)
		: base(bounds)
	{
		Text = text;
	}

	public string Text { get; set; }
}
