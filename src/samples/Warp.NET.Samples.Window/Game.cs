using Silk.NET.OpenGL;

namespace Warp.NET.Samples.Window;

[GenerateGame]
public sealed partial class Game
{
	protected override void Render()
	{
		Graphics.Gl.ClearColor(MathF.Sin(Tt), MathF.Sin(Tt / 2f), MathF.Sin(Tt / 3f), 1);
		Graphics.Gl.Clear(ClearBufferMask.ColorBufferBit);
	}
}
