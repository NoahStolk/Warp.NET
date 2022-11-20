using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Debugging;
using Warp.NET.Editor.Layouts;
using Warp.NET.Extensions;
using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui;
using Warp.NET.RenderImpl.Ui.Rendering;
using Warp.NET.RenderImpl.Ui.Rendering.Coordinates;
using Warp.NET.Text;
using Warp.NET.Ui;

namespace Warp.NET.Editor;

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

		MonoSpaceFontRenderer.Schedule(new(2), Constants.GridDefault.Get(Fraction.F01_02, Fraction.F01_08), 0, Color.White, "Warp.NET Editor", TextAlign.Middle);
		MonoSpaceFontRenderer.Schedule(new(1), Constants.GridDefault.Get(Fraction.F00_01, Fraction.F04_05), 0, Color.Red, DebugStack.GetString(), TextAlign.Left);
		CircleRenderer.Schedule(ViewportState.MousePosition.RoundToVector2Int32(), 12, 0, Color.Red);

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
		MonoSpaceFontRenderer.Render();

		RenderImplUiShaders.Sprite.Use();
		Shader.SetMatrix4x4(SpriteUniforms.Projection, _projectionMatrix);
		SpriteRenderer.Render();
	}
}
