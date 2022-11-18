using Warp.NET;
using Warp.NET.Numerics;
using Warp.NET.Samples.Text;

const int w = 1024;
const int h = 768;

Graphics.OnChangeWindowSize = OnChangeWindowSize;

#if DEBUG
const string? contentRootDirectory = @"..\..\..\Content";
#else
const string? contentRootDirectory = null;
#endif

Game game = Bootstrapper.CreateGame<Game, ShaderUniformInitializer, Models, Shaders, Sounds, Textures>("2D sample: Text", w, h, false, contentRootDirectory, "c");
game.Run();

static void OnChangeWindowSize(int width, int height)
{
	const float originalAspectRatio = w / (float)h;
	float adjustedWidth = height * originalAspectRatio; // Adjusted for aspect ratio
	float left = (width - adjustedWidth) / 2;
	SetViewport(left, 0, adjustedWidth, height); // Fix viewport to maintain aspect ratio

	void SetViewport(float x, float y, float w, float h)
	{
		Viewport viewport = new((int)x, (int)y, (int)w, (int)h);
		Gl.Viewport(viewport.X, viewport.Y, (uint)viewport.Width, (uint)viewport.Height);
	}
}
