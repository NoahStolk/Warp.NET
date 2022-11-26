using Warp.NET;
using Warp.NET.Content.Conversion;
using Warp.NET.Debugging;
using Warp.NET.RenderImpl.Ui;
using Warp.NET.Samples.Ui;

DebugStack.DisplaySetting = DebugStackDisplaySetting.Simple;
Graphics.OnChangeWindowSize = OnChangeWindowSize;
CreateWindow(new("Warp.NET.Samples.Ui", 1920, 1080, false));

#if DEBUG
const string? contentRootDirectory = @"..\..\..\..\..\lib\Warp.NET.RenderImpl.Ui\Content";
#else
const string? contentRootDirectory = null;
#endif

DecompiledContentFile renderImplUiContent = Bootstrapper.GetDecompiledContent(contentRootDirectory, "render-impl-ui");
RenderImplUiCharsets.Initialize(renderImplUiContent.Charsets);
RenderImplUiShaders.Initialize(renderImplUiContent.Shaders);
RenderImplUiTextures.Initialize(renderImplUiContent.Textures);

RenderImplUiShaderUniformInitializer.Initialize();

Game game = Bootstrapper.CreateGame<Game>();
RenderImplUiBase.Game = game;
game.Run();

static void OnChangeWindowSize(int width, int height)
{
	Gl.Viewport(0, 0, (uint)width, (uint)height);
}
