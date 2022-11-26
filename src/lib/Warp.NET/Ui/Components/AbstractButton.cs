using Silk.NET.GLFW;
using Warp.NET.Numerics;

namespace Warp.NET.Ui.Components;

public abstract class AbstractButton : AbstractComponent
{
	private readonly Action _onClick;

	protected AbstractButton(Bounds bounds, Action onClick)
		: base(bounds)
	{
		_onClick = onClick;
	}

	protected bool Hover { get; private set; }

	public bool IsDisabled { get; set; }

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		Hover = MouseUiContext.Contains(parentPosition, Bounds);

		if (Hover && !IsDisabled && Input.IsButtonPressed(MouseButton.Left))
			_onClick();
	}
}
