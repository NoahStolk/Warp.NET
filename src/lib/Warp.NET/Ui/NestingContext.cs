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

	private readonly HashSet<QueuedComponent> _temporaryProcessingQueue = new();
	private readonly List<QueuedComponent> _queuedComponents = new();

	public NestingContext(IBounds bounds)
	{
		Bounds = bounds;
	}

	public IReadOnlyList<AbstractComponent> OrderedComponents => _orderedComponents;

	public Vector2i<int> ScrollOffset { get; set; }

	public IBounds Bounds { get; }

	public Action? OnUpdateQueue { get; set; }

	public void Add(AbstractComponent component)
	{
		_queuedComponents.Add(new(component, true));
	}

	public void Remove(AbstractComponent component)
	{
		_queuedComponents.Add(new(component, false));
	}

	private void UpdateQueue()
	{
		if (_queuedComponents.Count == 0)
			return;

		for (int i = _queuedComponents.Count - 1; i >= 0; i--)
		{
			QueuedComponent component = _queuedComponents[i];

			// Skip this component if we already processed it.
			// This entry will be processed next time.
			// This way we can add and remove the same component multiple times during a single update, without breaking the UI.
			if (_temporaryProcessingQueue.Contains(component))
				continue;

			if (component.Add)
			{
				// Only add the component if it doesn't already exist.
				// Otherwise we would get corrupted state.
				if (!_orderedComponents.Contains(component.Component))
				{
					_orderedComponents.Add(component.Component);
					_temporaryProcessingQueue.Add(component);
				}
			}
			else
			{
				_orderedComponents.Remove(component.Component);
				_temporaryProcessingQueue.Add(component);
			}

			// Always drop this entry, even if we didn't process it because the component already exists.
			_queuedComponents.Remove(component);
		}

		_temporaryProcessingQueue.Clear();

		_orderedComponents = _orderedComponents.OrderBy(c => c.Depth).ToList();

		OnUpdateQueue?.Invoke();
	}

	public void Update(Vector2i<int> scrollOffset)
	{
		UpdateQueue();

		for (int i = _orderedComponents.Count - 1; i >= 0; i--)
		{
			AbstractComponent component = _orderedComponents[i];
			if (IsInParent(component))
				component.Update(scrollOffset + ScrollOffset);
		}
	}

	public void Render(Vector2i<int> scrollOffset)
	{
		for (int i = 0; i < _orderedComponents.Count; i++)
		{
			AbstractComponent component = _orderedComponents[i];
			if (IsInParent(component))
				component.Render(scrollOffset + ScrollOffset);
		}
	}

	private bool IsInParent(AbstractComponent component)
	{
		if (!component.IsActive)
			return false;

		IBounds boundsWithScrollOffset = component.Bounds.Move(ScrollOffset.X, ScrollOffset.Y);
		return Bounds.IntersectsOrContains(boundsWithScrollOffset);
	}

	private sealed class QueuedComponent
	{
		public QueuedComponent(AbstractComponent component, bool add)
		{
			Component = component;
			Add = add;
		}

		public AbstractComponent Component { get; }

		public bool Add { get; }
	}
}
