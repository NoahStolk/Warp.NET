using Warp.NET.Numerics;
using Warp.NET.Ui.Components;

namespace Warp.NET.Ui;

public class NestingContext
{
	/// <summary>
	/// Components are ordered by depth (ascending).
	/// We update them in reverse order, so components in front of other components are updated first.
	/// We render them in normal order, so components in front of other components are rendered last.
	/// </summary>
	private List<AbstractComponent> _orderedComponents = new();

	private readonly List<AbstractComponent> _toAdd = new();
	private readonly List<AbstractComponent> _toRemove = new();

	public NestingContext(IBounds bounds)
	{
		Bounds = bounds;
	}

	public IReadOnlyList<AbstractComponent> ToAdd => _toAdd;

	public IReadOnlyList<AbstractComponent> ToRemove => _toRemove;

	public IReadOnlyList<AbstractComponent> OrderedComponents => _orderedComponents;

	public Vector2i<int> ScrollOffset { get; set; }

	public IBounds Bounds { get; }

	public void Add(AbstractComponent component)
	{
		_toAdd.Add(component);
	}

	public void Remove(AbstractComponent component)
	{
		_toRemove.Add(component);
	}

	private void UpdateQueue()
	{
		if (_toAdd.Count == 0 && _toRemove.Count == 0)
			return;

		foreach (AbstractComponent component in _toAdd)
		{
			if (_orderedComponents.Contains(component))
				throw new InvalidOperationException("Attempting to add an already existing component.");

			_orderedComponents.Add(component);
		}

		foreach (AbstractComponent component in _toRemove)
		{
			if (_toAdd.Contains(component))
				throw new InvalidOperationException("Attempting to add and remove the same component at the same time.");

			_orderedComponents.Remove(component);
		}

		_toAdd.Clear();
		_toRemove.Clear();

		_orderedComponents = _orderedComponents.OrderBy(c => c.Depth).ToList();
	}

	public void Update(Vector2i<int> parentPosition)
	{
		UpdateQueue();

		for (int i = _orderedComponents.Count - 1; i >= 0; i--)
		{
			AbstractComponent component = _orderedComponents[i];
			if (IsInParent(component))
				component.Update(parentPosition + ScrollOffset);
		}
	}

	public void Render(Vector2i<int> parentPosition)
	{
		for (int i = 0; i < _orderedComponents.Count; i++)
		{
			AbstractComponent component = _orderedComponents[i];
			if (IsInParent(component))
				component.Render(parentPosition + ScrollOffset);
		}
	}

	private bool IsInParent(AbstractComponent component)
	{
		if (!component.IsActive)
			return false;

		Vector2i<int> parentTopLeft = new Vector2i<int>(Bounds.X1, Bounds.Y1) + ScrollOffset;
		return Bounds.IntersectsOrContains(component.Bounds.X1 + parentTopLeft.X, component.Bounds.Y1 + parentTopLeft.Y, component.Bounds.X2 + parentTopLeft.X, component.Bounds.Y2 + parentTopLeft.Y);
	}
}