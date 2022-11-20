namespace Warp.NET.RenderImpl.Ui;

public static class RenderImplUiBase
{
	private static RenderImplUiGameBase? _game;

	public static RenderImplUiGameBase Game
	{
		get => _game ?? throw new InvalidOperationException("Game is not initialized.");
		set
		{
			if (_game != null)
				throw new InvalidOperationException("Game is already initialized.");

			_game = value;
		}
	}
}
