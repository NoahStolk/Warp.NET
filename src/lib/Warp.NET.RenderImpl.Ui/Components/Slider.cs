using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.RenderImpl.Ui.Components;

public class Slider : AbstractSlider
{
	public Slider(IBounds bounds, Action<float> onChange, bool applyInstantly, float min, float max, float step, float defaultValue, SliderStyle sliderStyle)
		: base(bounds, onChange, applyInstantly, min, max, step, defaultValue)
	{
		SliderStyle = sliderStyle;
	}

	public SliderStyle SliderStyle { get; set; }

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Vector2i<int> borderVec = new(SliderStyle.BorderSize);

		RenderImplUiBase.Game.RectangleRenderer.Schedule(Bounds.Size, scrollOffset + Bounds.Center, Depth, Color.White);
		RenderImplUiBase.Game.RectangleRenderer.Schedule(Bounds.Size - borderVec, scrollOffset + Bounds.Center, Depth + 1, Hold ? Color.Gray(0.5f) : Hover ? Color.Gray(0.25f) : Color.Black);

		RenderImplUiBase.Game.GetFontRenderer(SliderStyle.FontSize).Schedule(Vector2i<int>.One, scrollOffset + Bounds.Center, Depth + 3, SliderStyle.TextColor, CurrentValue.ToString("0.00"), SliderStyle.TextAlign);
	}
}
