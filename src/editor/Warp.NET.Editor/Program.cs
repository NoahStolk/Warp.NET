using Warp.NET;
using Warp.NET.Content.Conversion;
using Warp.NET.Editor;
using Warp.NET.RenderImpl.Ui;

Graphics.OnChangeWindowSize = OnChangeWindowSize;
CreateWindow(new("Warp.NET Editor", 1920, 1080, false));

#if DEBUG
const string? contentRootDirectory = @"..\..\..\..\..\lib\Warp.NET.RenderImpl.Ui\Content";
#else
const string? contentRootDirectory = null;
#endif

DecompiledContentFile decompiledContentFile = Bootstrapper.GetDecompiledContent(contentRootDirectory, "render-impl-ui-content");
RenderImplUiCharsets.Initialize(decompiledContentFile.Charsets);
RenderImplUiShaders.Initialize(decompiledContentFile.Shaders);
RenderImplUiTextures.Initialize(decompiledContentFile.Textures);

RenderImplUiShaderUniformInitializer.Initialize();

Game game = Bootstrapper.CreateGame<Game>();
RenderImplUiBase.Game = game;
game.Run();

static void OnChangeWindowSize(int width, int height)
{
	Gl.Viewport(0, 0, (uint)width, (uint)height);
}
