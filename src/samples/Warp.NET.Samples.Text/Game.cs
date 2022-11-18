using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Numerics;
using Warp.NET.Samples.Text.Renderers;

namespace Warp.NET.Samples.Text;

[GenerateGame]
public sealed partial class Game
{
	private readonly Matrix4x4 _projectionMatrix;
	private readonly MonoSpaceFontRenderer _monoSpaceFontRenderer = new(new(Textures.Font, @" 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?:()[]{}<>|@^$%#&/\+*`,'=~;.-_  "));

	private Game(GameParameters gameParameters)
		: base(gameParameters)
	{
		_projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0, InitialWindowWidth, InitialWindowHeight, 0, 0, 1);
	}

	protected override void Render()
	{
		Gl.ClearColor(0, 0, 0, 1);
		Gl.Clear(ClearBufferMask.ColorBufferBit);

		Shaders.Font.Use();
		Shader.SetMatrix4x4(FontUniforms.Projection, _projectionMatrix);

		const int x = 512;
		float y = 128;
		_monoSpaceFontRenderer.Render(Vector2i<int>.One * 2, new(x, y += 64), "Big text");
		_monoSpaceFontRenderer.Render(Vector2i<int>.One, new(x, y += 64), "Left align");
		_monoSpaceFontRenderer.Render(Vector2i<int>.One, new(x, y += 64), $"{x} x {y}");
		_monoSpaceFontRenderer.Render(Vector2i<int>.One, new(x, y += 64), $"{x} x {y}");
		_monoSpaceFontRenderer.Render(Vector2i<int>.One, new(x, y += 64), $"{x} x {y}");
		_monoSpaceFontRenderer.Render(Vector2i<int>.One, new(x, y += 64), "Center align", true);
		_monoSpaceFontRenderer.Render(Vector2i<int>.One, new(x, y += 64), $"{x} x {y}", true);
		_monoSpaceFontRenderer.Render(Vector2i<int>.One, new(x, y += 64), $"{x} x {y}", true);
		_monoSpaceFontRenderer.Render(Vector2i<int>.One, new(x, y += 64), $"{x} x {y}", true);
		_monoSpaceFontRenderer.Render(Vector2i<int>.One, new(64, 64), "Text with...\n...line breaks");
		_monoSpaceFontRenderer.Render(Vector2i<int>.One, new(512, 64), "Centered text with...\n...line breaks", true);
	}
}
