using Warp.NET;
using Warp.NET.Samples.Window;

GameParameters gameParameters = new("Warp.NET.Samples.Window", 1024, 768, false);
Bootstrapper.CreateWindow(gameParameters);
Game game = Bootstrapper.CreateGame<Game>(gameParameters);
game.Run();
