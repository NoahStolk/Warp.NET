using Warp.NET;
using Warp.NET.Content;
using Warp.NET.Samples.Text;

CreateWindow(new("2D sample: Text", 1024, 768, false));

Graphics.OnChangeWindowSize = OnChangeWindowSize;

#if DEBUG
const string? contentRootDirectory = @"..\..\..\Content";
#else
const string? contentRootDirectory = null;
#endif

DecompiledContentFile decompiledContentFile = Bootstrapper.GetDecompiledContent(contentRootDirectory, "c");
Charsets.Initialize(decompiledContentFile.Charsets);
Shaders.Initialize(decompiledContentFile.Shaders);
Textures.Initialize(decompiledContentFile.Textures);

ShaderUniformInitializer.Initialize();

Game game = Bootstrapper.CreateGame<Game>();
game.Run();

static void OnChangeWindowSize(int width, int height)
{
	float originalAspectRatio = InitialWindowState.Width / (float)InitialWindowState.Height;
	float adjustedWidth = height * originalAspectRatio;
	float left = (width - adjustedWidth) / 2;
	Gl.Viewport((int)left, 0, (uint)adjustedWidth, (uint)height);
}
