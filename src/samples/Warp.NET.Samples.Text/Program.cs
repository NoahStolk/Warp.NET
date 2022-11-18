using Warp.NET;
using Warp.NET.Samples.Text;

const int initialWindowWidth = 1024;
const int initialWindowHeight = 768;

Graphics.OnChangeWindowSize = OnChangeWindowSize;

#if DEBUG
const string? contentRootDirectory = @"..\..\..\Content";
#else
const string? contentRootDirectory = null;
#endif

GameParameters gameParameters = new("2D sample: Text", initialWindowWidth, initialWindowHeight, false);
Game game = Bootstrapper.CreateGame<Game, ShaderUniformInitializer, Charsets, Models, Shaders, Sounds, Textures>(gameParameters, contentRootDirectory, "c");
game.Run();

static void OnChangeWindowSize(int width, int height)
{
	const float originalAspectRatio = initialWindowWidth / (float)initialWindowHeight;
	float adjustedWidth = height * originalAspectRatio;
	float left = (width - adjustedWidth) / 2;
	Gl.Viewport((int)left, 0, (uint)adjustedWidth, (uint)height);
}
