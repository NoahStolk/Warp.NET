using Warp.NET;
using Warp.NET.Samples.Window;

Graphics.CreateWindow(new("Warp.NET.Samples.Window", 1024, 768, false));
Game game = Bootstrapper.CreateGame<Game>();
game.Run();
