using Warp.NET.Numerics;

namespace Warp.NET.Ui.Components;

public abstract class AbstractComponent
{
	protected AbstractComponent(Bounds bounds)
	{
		Bounds = bounds;
		NestingContext = new(bounds);
	}

	public Bounds Bounds { get; set; }
	public bool IsActive { get; set; } = true;
	public int Depth { get; set; }
	public NestingContext NestingContext { get; }

	public virtual void Update(Vector2i<int> parentPosition)
	{
		NestingContext.Update(parentPosition + Bounds.TopLeft);
	}

	public virtual void Render(Vector2i<int> parentPosition)
	{
		NestingContext.Render(parentPosition + Bounds.TopLeft);
	}
}
