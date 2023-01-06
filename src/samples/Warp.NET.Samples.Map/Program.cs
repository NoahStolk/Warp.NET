using Warp.NET;
using Warp.NET.Content.Conversion;
using Warp.NET.Samples.Map;

CreateWindow(new("3D sample: Map", 1920, 1080, false));

Graphics.OnChangeWindowSize = OnChangeWindowSize;

#if DEBUG
const string? contentRootDirectory = @"..\..\..\Content";
#else
const string? contentRootDirectory = null;
#endif

DecompiledContentFile decompiledContentFile = Bootstrapper.GetDecompiledContent(contentRootDirectory, "c");
Charsets.Initialize(decompiledContentFile.Charsets);
Maps.Initialize(decompiledContentFile.Maps);
Shaders.Initialize(decompiledContentFile.Shaders);
Textures.Initialize(decompiledContentFile.Textures);

ShaderUniformInitializer.Initialize();

Game game = Bootstrapper.CreateGame<Game>();
game.Initialize();
game.Run();

static void OnChangeWindowSize(int width, int height)
{
	float originalAspectRatio = InitialWindowState.Width / (float)InitialWindowState.Height;
	float adjustedWidth = height * originalAspectRatio;
	float left = (width - adjustedWidth) / 2;
	Gl.Viewport((int)left, 0, (uint)adjustedWidth, (uint)height);
}
