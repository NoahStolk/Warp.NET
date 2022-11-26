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

	public NestingContext(Bounds bounds)
	{
		Bounds = bounds;
	}

	public IReadOnlyList<AbstractComponent> ToAdd => _toAdd;

	public IReadOnlyList<AbstractComponent> ToRemove => _toRemove;

	public IReadOnlyList<AbstractComponent> OrderedComponents => _orderedComponents;

	public Vector2i<int> ScrollOffset { get; set; }

	public Bounds Bounds { get; }

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

		Bounds boundsWithScrollOffset = component.Bounds with
		{
			X = component.Bounds.X + ScrollOffset.X / (float)Graphics.CurrentWindowState.Height,
			Y = component.Bounds.Y + ScrollOffset.Y / (float)Graphics.CurrentWindowState.Height,
		};

		return Bounds.IntersectsOrContains(boundsWithScrollOffset);
	}
}
