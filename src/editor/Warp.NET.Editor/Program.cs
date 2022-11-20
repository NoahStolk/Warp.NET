using Warp.NET;
using Warp.NET.Editor;
using Warp.NET.RenderImpl.Ui;
using Warp.NET.RenderImpl.Ui.Rendering;
using Models = Warp.NET.Editor.Models;
using Sounds = Warp.NET.Editor.Sounds;

#if DEBUG
const string? contentRootDirectory = @"..\..\..\Content";
#else
const string? contentRootDirectory = null;
#endif

GameParameters gameParameters = new("Warp.NET Editor", Constants.InitialWindowWidth, Constants.InitialWindowHeight, false);

OnChangeWindowSize = (w, h) => ViewportState.OnChangeWindowSize(w, h, gameParameters);

Game game = Bootstrapper.CreateGame<Game, RenderImplUiShaderUniformInitializer, RenderImplUiCharsets, Models, RenderImplUiShaders, Sounds, RenderImplUiTextures>(gameParameters, contentRootDirectory, "c");
RenderImplUiBase.Game = game;
game.Run();
