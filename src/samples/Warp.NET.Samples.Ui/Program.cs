using Warp.NET;
using Warp.NET.Content;
using Warp.NET.Debugging;
using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui;
using Warp.NET.RenderImpl.Ui.Rendering;
using Warp.NET.Samples.Ui;

const int nativeWidth = 1920;
const int nativeHeight = 1080;

DebugStack.DisplaySetting = DebugStackDisplaySetting.Simple;
Graphics.OnChangeWindowSize = OnChangeWindowSize;
CreateWindow(new("Warp.NET.Samples.Ui", nativeWidth, nativeHeight, false));
SetWindowSizeLimits(nativeWidth, nativeHeight, -1, -1);

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
	const float originalAspectRatio = nativeWidth / (float)nativeHeight;
	const float adjustedWidth = nativeHeight * originalAspectRatio; // Adjusted for aspect ratio

	int leftOffset = (int)((width - adjustedWidth) / 2);
	int bottomOffset = (height - nativeHeight) / 2;
	ViewportState.Offset = new(leftOffset, bottomOffset);
	ViewportState.Viewport = new(leftOffset, bottomOffset, (int)adjustedWidth, nativeHeight); // Fix viewport to maintain aspect ratio
	ViewportState.Scale = new(ViewportState.Viewport.Width / (float)nativeWidth, ViewportState.Viewport.Height / (float)nativeHeight);

	ActivateViewport(ViewportState.Viewport);

	static void ActivateViewport(Viewport viewport)
	{
		Gl.Viewport(viewport.X, viewport.Y, (uint)viewport.Width, (uint)viewport.Height);
	}
}
