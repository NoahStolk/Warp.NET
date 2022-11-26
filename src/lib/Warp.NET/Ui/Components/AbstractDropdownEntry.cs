using Silk.NET.GLFW;
using Warp.NET.Numerics;

namespace Warp.NET.Ui.Components;

public abstract class AbstractDropdownEntry : AbstractComponent
{
	private readonly AbstractDropdown _parent;
	private readonly Action _onClick;

	protected AbstractDropdownEntry(Bounds bounds, AbstractDropdown parent, Action onClick)
		: base(bounds)
	{
		_parent = parent;
		_onClick = onClick;
	}

	protected bool Hover { get; private set; }

	public bool IsDisabled { get; set; }

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		Hover = MouseUiContext.Contains(parentPosition, Bounds);

		if (!Hover || IsDisabled || !Input.IsButtonPressed(MouseButton.Left))
			return;

		_onClick();
		_parent.Toggle(false);
	}
}
