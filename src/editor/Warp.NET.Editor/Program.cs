using Warp.NET;
using Warp.NET.Editor;
using Warp.NET.Editor.Rendering;

OnChangeWindowSize = ViewportState.OnChangeWindowSize;

#if DEBUG
const string? contentRootDirectory = @"..\..\..\Content";
#else
const string? contentRootDirectory = null;
#endif

GameParameters gameParameters = new("Warp.NET Editor", Constants.InitialWindowWidth, Constants.InitialWindowHeight, false);
Game game = Bootstrapper.CreateGame<Game, ShaderUniformInitializer, Models, Shaders, Sounds, Textures>(gameParameters, contentRootDirectory, "c");
game.Run();
