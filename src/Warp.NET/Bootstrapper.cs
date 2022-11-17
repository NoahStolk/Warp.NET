using Warp.NET.Content.Binaries;

namespace Warp.NET;

/// <summary>
/// Used to initiate game classes.
/// </summary>
public static class Bootstrapper
{
	/// <summary>
	/// Instantiates the game class.
	/// </summary>
	/// <param name="initialWindowTitle">The initial window title.</param>
	/// <param name="initialWindowWidth">The initial window width.</param>
	/// <param name="initialWindowHeight">The initial window height.</param>
	/// <param name="initialWindowFullScreen">Whether full screen should be enabled when the game starts.</param>
	/// <param name="contentRootDirectory">The content root directory to generate a content file from. If the directory does not exist, or is <see langword="null" />, the file will not be generated.</param>
	/// <param name="contentFilePath">The generated content file path required to bootstrap the game.</param>
	/// <typeparam name="TGame">The game type which must implement <see cref="IGameBase{TSelf}"/>.</typeparam>
	/// <typeparam name="TBase">The base type which is used to access the game instance.</typeparam>
	/// <typeparam name="TShaderUniformInitializer">The shader uniform initializer type. This type is generated.</typeparam>
	/// <returns>The game instance.</returns>
	/// <exception cref="InvalidOperationException">When the file at <paramref name="contentFilePath"/> does not exist, or when the entry assembly could not be retrieved.</exception>
	public static TGame CreateGame<TGame, TBase, TShaderUniformInitializer>(string initialWindowTitle, int initialWindowWidth, int initialWindowHeight, bool initialWindowFullScreen, string? contentRootDirectory, string contentFilePath)
		where TGame : GameBase, IGameBase<TGame>
		where TBase : IBase<TGame>
		where TShaderUniformInitializer : IShaderUniformInitializer, new()
	{
		if (initialWindowFullScreen)
			Graphics.CreateWindowFull(initialWindowTitle);
		else
			Graphics.CreateWindow(initialWindowTitle, initialWindowWidth, initialWindowHeight);

		if (Directory.Exists(contentRootDirectory))
			ContentFileWriter.GenerateContentFile(contentRootDirectory, contentFilePath);

		if (!File.Exists(contentFilePath))
			throw new InvalidOperationException("The generated content file is missing. Make sure to build in DEBUG mode or copy the file generated in DEBUG mode to the RELEASE output.");

		// TODO: Assign the values from the decompiled content file.
		DecompiledContentFile decompiledContentFile = ContentFileReader.Read(contentFilePath);

		TShaderUniformInitializer.Initialize();

		TGame game = TGame.Construct(initialWindowTitle, initialWindowWidth, initialWindowHeight, initialWindowFullScreen);
		WarpBase.Game = game;
		TBase.Game = game;
		return game;
	}
}
