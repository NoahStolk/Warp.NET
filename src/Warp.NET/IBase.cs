namespace Warp.NET;

public interface IBase<TGame>
	where TGame : IGameBase<TGame>
{
	static abstract TGame Game { get; set; }
}
