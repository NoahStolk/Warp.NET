using Warp.NET;
using Warp.NET.Content.Conversion;
using Warp.NET.Editor;
using Warp.NET.RenderImpl.Ui;
using Warp.NET.RenderImpl.Ui.Rendering;

GameParameters gameParameters = new("Warp.NET Editor", Constants.InitialWindowWidth, Constants.InitialWindowHeight, false);

OnChangeWindowSize = (w, h) => ViewportState.OnChangeWindowSize(w, h, gameParameters);
Bootstrapper.CreateWindow(gameParameters);

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

Game game = Bootstrapper.CreateGame<Game>(gameParameters);
RenderImplUiBase.Game = game;
game.Run();
