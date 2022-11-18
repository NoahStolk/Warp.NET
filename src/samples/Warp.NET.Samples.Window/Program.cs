using Warp.NET;
using Warp.NET.Samples.Window;

#if DEBUG
const string? contentRootDirectory = @"..\..\..\Content";
#else
const string? contentRootDirectory = null;
#endif

Game game = Bootstrapper.CreateGame<Game, ShaderUniformInitializer, Models, Shaders, Sounds, Textures>("Warp.NET.Samples.Window", 1024, 768, false, contentRootDirectory, "c");
game.Run();
