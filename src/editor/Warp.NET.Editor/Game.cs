using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Debugging;
using Warp.NET.Editor.Layouts;
using Warp.NET.Editor.Rendering;
using Warp.NET.Editor.Rendering.Renderers;
using Warp.NET.Extensions;
using Warp.NET.Numerics;
using Warp.NET.Text;
using Warp.NET.Ui;

namespace Warp.NET.Editor;

[GenerateGame]
public sealed partial class Game
{
	private readonly Matrix4x4 _projectionMatrix;
	private readonly MonoSpaceFontRenderer _monoSpaceFontRenderer = new(new(Textures.Font, @" 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?:()[]{}<>|@^$%#&/\+*`,'=~;.-_  "));
	private readonly SpriteRenderer _spriteRenderer = new();
	private readonly UiRenderer _uiRenderer = new();

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

		RenderBatchCollector.RenderMonoSpaceText(new(2), CoordinateSystem.Get(0.5f, 0.125f), 0, Color.White, "Warp.NET Editor", TextAlign.Middle);
		RenderBatchCollector.RenderMonoSpaceText(new(1), CoordinateSystem.Get(0, 0.8f), 0, Color.Red, DebugStack.GetString(), TextAlign.Left);
		RenderBatchCollector.RenderCircleCenter(ViewportState.MousePosition.RoundToVector2Int32(), 12, 0, Color.Red);

		_mainLayout.NestingContext.Render(default);
	}

	protected override void Render()
	{
		Gl.ClearColor(0, 0, 0, 1);
		Gl.Clear(ClearBufferMask.ColorBufferBit);

		Shaders.Ui.Use();
		Shader.SetMatrix4x4(UiUniforms.Projection, _projectionMatrix);
		_uiRenderer.RenderRectangleTriangles();
		_uiRenderer.RenderCircleLines();

		Shaders.Font.Use();
		Shader.SetMatrix4x4(FontUniforms.Projection, _projectionMatrix);
		_monoSpaceFontRenderer.Render(RenderBatchCollector.MonoSpaceTexts);

		Shaders.Sprite.Use();
		Shader.SetMatrix4x4(SpriteUniforms.Projection, _projectionMatrix);
		_spriteRenderer.Render();

		RenderBatchCollector.Clear();
	}
}
