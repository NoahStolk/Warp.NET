using Silk.NET.GLFW;
using Warp.NET.Numerics;

namespace Warp.NET.Ui.Components;

public abstract class AbstractSlider : AbstractComponent
{
	private readonly Action<float> _onChange;
	private readonly bool _applyInstantly;

	protected AbstractSlider(IBounds bounds, Action<float> onChange, bool applyInstantly, float min, float max, float step, float defaultValue)
		: base(bounds)
	{
		_onChange = onChange;
		_applyInstantly = applyInstantly;
		Min = min;
		Max = max;
		Step = step;

		CurrentValue = defaultValue;
	}

	public float Min { get; set; }
	public float Max { get; set; }
	public float Step { get; set; }

	public float CurrentValue { get; set; }

	protected bool Hold { get; private set; }

	protected bool Hover { get; private set; }

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		Hover = MouseUiContext.Contains(parentPosition, Bounds);

		if (Hover && Input.IsButtonPressed(MouseButton.Left))
		{
			Hold = true;
		}
		else if (Hold)
		{
			UpdateValue();

			if (_applyInstantly)
				_onChange(CurrentValue);
		}

		if (Hold && Input.IsButtonReleased(MouseButton.Left))
		{
			if (Hover)
				UpdateValue();

			_onChange(CurrentValue);
			Hold = false;
		}

		void UpdateValue()
		{
			float percentage = (MouseUiContext.MousePosition.X - parentPosition.X - Bounds.X1) / (Bounds.X2 - Bounds.X1);
			float realValue = Math.Clamp(percentage * (Max - Min) + Min, Min, Max);
			CurrentValue = MathF.Round(realValue / Step) * Step;
		}
	}
}
