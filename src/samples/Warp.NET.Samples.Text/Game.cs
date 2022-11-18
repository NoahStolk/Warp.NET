using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Samples.Text.Renderers;
using Warp.NET.Text;

namespace Warp.NET.Samples.Text;

[GenerateGame]
public sealed partial class Game
{
	private readonly Matrix4x4 _projectionMatrix;
	private readonly MonoSpaceFontRenderer spleen6x12Renderer = new(new(Textures.Spleen6x12, Charsets.Ascii_32_126));
	private readonly MonoSpaceFontRenderer spleen16x32Renderer = new(new(Textures.Spleen16x32, Charsets.Ascii_32_126));

	private Game(GameParameters gameParameters)
		: base(gameParameters)
	{
		_projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0, InitialWindowWidth, InitialWindowHeight, 0, 0, 1);
	}

	protected override void PrepareRender()
	{
		base.PrepareRender();

		const int x = 512;
		int y = 128;
		spleen6x12Renderer.Render(new(1), new(x, y), "Tiny text");
		spleen16x32Renderer.Schedule(new(2), new(x, y += 64), "Big text", TextAlign.Left);
		spleen16x32Renderer.Schedule(new(1), new(x, y += 64), "Left align", TextAlign.Left);
		spleen16x32Renderer.Schedule(new(1), new(x, y += 64), $"{x} x {y}", TextAlign.Left);
		spleen16x32Renderer.Schedule(new(1), new(x, y += 64), $"{x} x {y}", TextAlign.Left);
		spleen16x32Renderer.Schedule(new(1), new(x, y += 64), $"{x} x {y}", TextAlign.Left);
		spleen16x32Renderer.Schedule(new(1), new(x, y += 64), "Center align", TextAlign.Middle);
		spleen16x32Renderer.Schedule(new(1), new(x, y += 64), $"{x} x {y}", TextAlign.Middle);
		spleen16x32Renderer.Schedule(new(1), new(x, y += 64), $"{x} x {y}", TextAlign.Middle);
		spleen16x32Renderer.Schedule(new(1), new(x, y += 64), $"{x} x {y}", TextAlign.Middle);
		spleen16x32Renderer.Schedule(new(1), new(64, 64), "Text with...\n...line breaks", TextAlign.Left);
		spleen16x32Renderer.Schedule(new(1), new(512, 64), "Centered text with...\n...line breaks", TextAlign.Middle);
	}

	protected override void Render()
	{
		Gl.ClearColor(0, 0, 0, 1);
		Gl.Clear(ClearBufferMask.ColorBufferBit);

		Shaders.Font.Use();
		Shader.SetMatrix4x4(FontUniforms.Projection, _projectionMatrix);

		spleen6x12Renderer.Render();
		spleen16x32Renderer.Render();
	}
}
