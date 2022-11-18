using Warp.NET;
using Warp.NET.Samples.Window;

#if DEBUG
const string? contentRootDirectory = @"..\..\..\Content";
#else
const string? contentRootDirectory = null;
#endif

GameParameters gameParameters = new("Warp.NET.Samples.Window", 1024, 768, false);
Game game = Bootstrapper.CreateGame<Game, ShaderUniformInitializer, Charsets, Models, Shaders, Sounds, Textures>(gameParameters, contentRootDirectory, "c");
game.Run();
