namespace Warp.NET;

public interface IGameBase<out TSelf>
	where TSelf : IGameBase<TSelf>
{
	static abstract TSelf Construct(string initialWindowTitle, int initialWindowWidth, int initialWindowHeight, bool initialWindowFullScreen);
}
