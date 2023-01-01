using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering;
using Warp.NET.RenderImpl.Ui.Rendering.Renderers;
using Warp.NET.RenderImpl.Ui.Rendering.Scissors;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;
using Warp.NET.Ui;

namespace Warp.NET.Samples.Ui.Layouts;

public class ScissorLayout : Layout
{
	public ScissorLayout()
	{
		LabelStyle labelStyle = new(Color.Green, TextAlign.Middle, FontSize.H16);
		ButtonStyle buttonStyle = new(Color.Black, Color.White, Color.Gray(0.5f), 2);
		TextButtonStyle textButtonStyle = new(Color.White, TextAlign.Middle, FontSize.H24);

		Label titleLabel = new(new PixelBounds(960 - 64, 192, 128, 128), "Scissor Layout", labelStyle);
		TextButton backButton = new(new PixelBounds(256, 320, 256, 128), () => Game.Self.ActiveLayout = Game.Self.MainLayout, buttonStyle, textButtonStyle, "Back");

		NestingContext.Add(titleLabel);
		NestingContext.Add(backButton);
	}

	public void Render()
	{
		RenderSquare(Color.Red);

		ScissorScheduler.PushScissor(Scissor.Create(new PixelBounds(640, 640, 384, 384), default, ViewportState.Offset, ViewportState.Scale));
		RenderSquare(Color.Yellow);

		ScissorScheduler.PushScissor(Scissor.Create(new PixelBounds(640, 640, 64, 64), default, ViewportState.Offset, ViewportState.Scale));
		RenderSquare(Color.Purple);

		ScissorScheduler.PopScissor();
		ScissorScheduler.PushScissor(Scissor.Create(new PixelBounds(704, 704, 128, 128), default, ViewportState.Offset, ViewportState.Scale));
		RenderSquare(Color.Green);

		ScissorScheduler.PopScissor();
		ScissorScheduler.PopScissor();

		void RenderSquare(Color color)
		{
			Game.Self.RectangleRenderer.Schedule(new(384, 384), new(768, 768), 0, color);
		}
	}
}
