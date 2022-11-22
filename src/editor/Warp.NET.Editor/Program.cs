using System.Numerics;
using Warp.NET;
using Warp.NET.Content.Conversion;
using Warp.NET.Editor;
using Warp.NET.RenderImpl.Ui;
using Warp.NET.RenderImpl.Ui.Rendering;

GameParameters gameParameters = new("Warp.NET Editor", Constants.InitialWindowWidth, Constants.InitialWindowHeight, false);

Graphics.OnChangeWindowSize = (w, h) => OnChangeWindowSize(w, h, gameParameters);
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

static void OnChangeWindowSize(int width, int height, GameParameters gameParameters)
{
	float originalAspectRatio = gameParameters.InitialWindowWidth / (float)gameParameters.InitialWindowHeight;
	float adjustedWidth = height * originalAspectRatio; // Adjusted for aspect ratio
	ViewportState.Offset = new((width - adjustedWidth) / 2, 0);
	Vector2 size = new(adjustedWidth, height); // Fix viewport to maintain aspect ratio
	ViewportState.Scale = size / new Vector2(gameParameters.InitialWindowWidth, gameParameters.InitialWindowHeight);

	ViewportState.Viewport = new((int)ViewportState.Offset.X, (int)ViewportState.Offset.Y, (int)size.X, (int)size.Y);
	Gl.Viewport(ViewportState.Viewport.X, ViewportState.Viewport.Y, (uint)ViewportState.Viewport.Width, (uint)ViewportState.Viewport.Height);
}
