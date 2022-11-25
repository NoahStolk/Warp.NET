using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Debugging;
using Warp.NET.RenderImpl.Ui;
using Warp.NET.RenderImpl.Ui.Rendering;
using Warp.NET.Samples.Ui.Layouts;
using Warp.NET.Ui;

namespace Warp.NET.Samples.Ui;

[GenerateGame]
public sealed partial class Game : RenderImplUiGameBase
{
	private readonly Matrix4x4 _projectionMatrix;
	private readonly MainLayout _mainLayout = new();

	private Game(GameParameters gameParameters)
		: base(gameParameters)
	{
		_projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0, InitialWindowWidth, InitialWindowHeight, 0, 0, 1);

		DebugStack.DisplaySetting = DebugStackDisplaySetting.Simple;
	}

	protected override void Update()
	{
		base.Update();

		MouseUiContext.Reset(ViewportState.MousePosition);

		_mainLayout.NestingContext.Update(default);
	}

	protected override void PrepareRender()
	{
		base.PrepareRender();

		_mainLayout.NestingContext.Render(default);
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
