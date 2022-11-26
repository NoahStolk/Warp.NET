using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Debugging;
using Warp.NET.Editor.Layouts;
using Warp.NET.Extensions;
using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui;
using Warp.NET.RenderImpl.Ui.Rendering.Coordinates;
using Warp.NET.RenderImpl.Ui.Utils;
using Warp.NET.Text;
using Warp.NET.Ui;

namespace Warp.NET.Editor;

[GenerateGame]
public sealed partial class Game : RenderImplUiGameBase
{
	private readonly MainLayout _mainLayout = new();

	private Game()
	{
		DebugStack.DisplaySetting = DebugStackDisplaySetting.Simple;
	}

	protected override void Update()
	{
		base.Update();

		MouseUiContext.Reset(Input.GetMousePosition());

		_mainLayout.NestingContext.Update(default);
	}

	protected override void PrepareRender()
	{
		base.PrepareRender();

		MonoSpaceFontRenderer32.Schedule(new(2), WindowPosition.Get(Fraction.F01_02, Fraction.F01_08), 0, Color.White, "Warp.NET Editor", TextAlign.Middle);
		MonoSpaceFontRenderer12.Schedule(new(1), WindowPosition.Get(Fraction.F00_01, Fraction.F04_05), 0, Color.Red, DebugStack.GetString(), TextAlign.Left);
		CircleRenderer.Schedule(Input.GetMousePosition().RoundToVector2Int32(), 12, 0, Color.Red);

		_mainLayout.NestingContext.Render(default);
	}

	protected override void Render()
	{
		Gl.ClearColor(0, 0, 0, 1);
		Gl.Clear(ClearBufferMask.ColorBufferBit);

		Matrix4x4 projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0, CurrentWindowState.Width, CurrentWindowState.Height, 0, 0, 1);

		RenderImplUiShaders.Ui.Use();
		Shader.SetMatrix4x4(UiUniforms.Projection, projectionMatrix);
		RectangleRenderer.Render();
		CircleRenderer.Render();

		RenderImplUiShaders.Font.Use();
		Shader.SetMatrix4x4(FontUniforms.Projection, projectionMatrix);
		MonoSpaceFontRenderer8.Render();
		MonoSpaceFontRenderer12.Render();
		MonoSpaceFontRenderer16.Render();
		MonoSpaceFontRenderer24.Render();
		MonoSpaceFontRenderer32.Render();
		MonoSpaceFontRenderer64.Render();

		RenderImplUiShaders.Sprite.Use();
		Shader.SetMatrix4x4(SpriteUniforms.Projection, projectionMatrix);
		SpriteRenderer.Render();
	}
}
