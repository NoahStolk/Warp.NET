using Silk.NET.GLFW;
using Warp.NET.Numerics;

namespace Warp.NET.Ui.Components;

public abstract class AbstractScrollbar : AbstractComponent
{
	private float _holdStartMouseY;
	private float _topPercentage;
	private float _oldTopPercentage;

	private readonly Action<float> _onChange;

	protected AbstractScrollbar(IBounds bounds, Action<float> onChange)
		: base(bounds)
	{
		_onChange = onChange;
	}

	public float ThumbPercentageSize { get; set; }

	public float TopPercentage
	{
		get => _topPercentage;
		set => _topPercentage = Math.Clamp(value, 0, 1 - ThumbPercentageSize);
	}

	protected bool Hold { get; private set; }

	protected bool Hover { get; private set; }

	private float TranslatedMousePosition(Vector2i<int> scrollOffset)
		=> MouseUiContext.MousePosition.Y - Bounds.Y1 - scrollOffset.Y;

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

		Hover = MouseUiContext.Contains(scrollOffset, Bounds);

		if (Hover && Input.IsButtonPressed(MouseButton.Left))
		{
			float mousePos = TranslatedMousePosition(scrollOffset);
			if (mousePos > TopPercentage * Bounds.Size.Y)
			{
				if (mousePos < (TopPercentage + ThumbPercentageSize) * Bounds.Size.Y)
				{
					Hold = true;
					_holdStartMouseY = mousePos;
					_oldTopPercentage = TopPercentage;
				}
				else
				{
					TopPercentage += 0.1f;
					_onChange(TopPercentage);
				}
			}
			else
			{
				TopPercentage -= 0.1f;
				_onChange(TopPercentage);
			}
		}
		else if (Hold)
		{
			UpdateValue();
			_onChange(TopPercentage);
		}

		if (Hold && Input.IsButtonReleased(MouseButton.Left))
		{
			if (Hover)
				UpdateValue();

			_onChange(TopPercentage);
			Hold = false;
			_oldTopPercentage = TopPercentage;
		}

		void UpdateValue()
		{
			float size = Bounds.Size.Y;
			float yDiff = TranslatedMousePosition(scrollOffset) - _holdStartMouseY;
			TopPercentage = (_oldTopPercentage * size + yDiff) / size;
		}
	}
}
