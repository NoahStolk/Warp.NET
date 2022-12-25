using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Debugging;
using Warp.NET.Extensions;
using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui;
using Warp.NET.RenderImpl.Ui.Rendering;
using Warp.NET.Samples.Ui.Layouts;
using Warp.NET.Text;
using Warp.NET.Ui;

namespace Warp.NET.Samples.Ui;

[GenerateGame]
public sealed partial class Game : RenderImplUiGameBase
{
	private readonly Matrix4x4 _projectionMatrix;

	private Game()
	{
		ActiveLayout = MainLayout;

		_projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0, InitialWindowState.Width, InitialWindowState.Height, 0, -1024, 1024);

		Gl.Enable(EnableCap.Blend);
		Gl.Enable(EnableCap.CullFace);
		Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
	}

	public ILayout ActiveLayout { get; set; }
	public MainLayout MainLayout { get; } = new();
	public ScrollContentLayout ScrollContentLayout { get; } = new();

	protected override void Update()
	{
		base.Update();

		MouseUiContext.Reset(ViewportState.MousePosition);

		ActiveLayout.NestingContext.Update(default);
	}

	protected override void PrepareRender()
	{
		base.PrepareRender();

		MonoSpaceFontRenderer32.Schedule(new(2), new(960, 128), 0, Color.White, "Warp.NET.Samples.Ui", TextAlign.Middle);
		MonoSpaceFontRenderer12.Schedule(new(1), new(960, 192), 0, Color.White, "This text is scheduled by the game itself, and is not part of the UI components.", TextAlign.Middle);
		MonoSpaceFontRenderer12.Schedule(new(1), new(64, 768), 0, Color.Red, DebugStack.GetString(), TextAlign.Left);
		CircleRenderer.Schedule(ViewportState.MousePosition.RoundToVector2Int32(), 12, 0, Color.Red);

		ActiveLayout.NestingContext.Render(default);
	}

	protected override void Render()
	{
		Gl.ClearColor(0, 0, 0, 1);
		Gl.Clear(ClearBufferMask.ColorBufferBit);

		RenderImplUiShaders.Ui.Use();
		Shader.SetMatrix4x4(UiUniforms.Projection, _projectionMatrix);
		RectangleRenderer.Render();
		CircleRenderer.Render();

		RenderImplUiShaders.Font.Use();
		Shader.SetMatrix4x4(FontUniforms.Projection, _projectionMatrix);
		MonoSpaceFontRenderer8.Render();
		MonoSpaceFontRenderer12.Render();
		MonoSpaceFontRenderer16.Render();
		MonoSpaceFontRenderer24.Render();
		MonoSpaceFontRenderer32.Render();
		MonoSpaceFontRenderer64.Render();

		RenderImplUiShaders.Sprite.Use();
		Shader.SetMatrix4x4(SpriteUniforms.Projection, _projectionMatrix);
		SpriteRenderer.Render();
	}
}
