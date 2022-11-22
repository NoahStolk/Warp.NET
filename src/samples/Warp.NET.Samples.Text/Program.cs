using Warp.NET;
using Warp.NET.Content.Conversion;
using Warp.NET.Samples.Text;

const int initialWindowWidth = 1024;
const int initialWindowHeight = 768;

Graphics.OnChangeWindowSize = OnChangeWindowSize;

#if DEBUG
const string? contentRootDirectory = @"..\..\..\Content";
#else
const string? contentRootDirectory = null;
#endif

GameParameters gameParameters = new("2D sample: Text", initialWindowWidth, initialWindowHeight, InitialWindowFullScreen: false);

Bootstrapper.CreateWindow(gameParameters);

DecompiledContentFile decompiledContentFile = Bootstrapper.GetDecompiledContent(contentRootDirectory, "c");
Charsets.Initialize(decompiledContentFile.Charsets);
Shaders.Initialize(decompiledContentFile.Shaders);
Textures.Initialize(decompiledContentFile.Textures);

ShaderUniformInitializer.Initialize();

Game game = Bootstrapper.CreateGame<Game>(gameParameters);
game.Run();

static void OnChangeWindowSize(int width, int height)
{
	const float originalAspectRatio = initialWindowWidth / (float)initialWindowHeight;
	float adjustedWidth = height * originalAspectRatio;
	float left = (width - adjustedWidth) / 2;
	Gl.Viewport((int)left, 0, (uint)adjustedWidth, (uint)height);
}
