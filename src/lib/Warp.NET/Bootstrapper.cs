using Warp.NET.Content;
using Warp.NET.Content.Conversion;

namespace Warp.NET;

/// <summary>
/// Used to instantiate game classes.
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
	/// <typeparam name="TGame">The game type which must derive from <see cref="GameBase"/> and implement <see cref="IGameBase{TSelf}"/>.</typeparam>
	/// <typeparam name="TShaderUniformInitializer">The shader uniform initializer type. This type is generated.</typeparam>
	/// <typeparam name="TModelContainer">The type containing the game's models. This type is generated.</typeparam>
	/// <typeparam name="TShaderContainer">The type containing the game's shaders. This type is generated.</typeparam>
	/// <typeparam name="TSoundContainer">The type containing the game's sounds. This type is generated.</typeparam>
	/// <typeparam name="TTextureContainer">The type containing the game's textures. This type is generated.</typeparam>
	/// <returns>The game instance.</returns>
	/// <exception cref="InvalidOperationException">When the file at <paramref name="contentFilePath"/> does not exist.</exception>
	public static TGame CreateGame<TGame, TShaderUniformInitializer, TModelContainer, TShaderContainer, TSoundContainer, TTextureContainer>(string initialWindowTitle, int initialWindowWidth, int initialWindowHeight, bool initialWindowFullScreen, string? contentRootDirectory, string contentFilePath)
		where TGame : GameBase, IGameBase<TGame>
		where TShaderUniformInitializer : IShaderUniformInitializer
		where TModelContainer : IContentContainer<Model>
		where TShaderContainer : IContentContainer<Shader>
		where TSoundContainer : IContentContainer<Sound>
		where TTextureContainer : IContentContainer<Texture>
	{
		if (initialWindowFullScreen)
			Graphics.CreateWindowFull(initialWindowTitle);
		else
			Graphics.CreateWindow(initialWindowTitle, initialWindowWidth, initialWindowHeight);

		if (Directory.Exists(contentRootDirectory))
			ContentFileWriter.GenerateContentFile(contentRootDirectory, contentFilePath);

		if (!File.Exists(contentFilePath))
			throw new InvalidOperationException("The generated content file is missing. Make sure to build in DEBUG mode or copy the file generated in DEBUG mode to the RELEASE output.");

		DecompiledContentFile decompiledContentFile = ContentFileReader.Read(contentFilePath);
		TModelContainer.Initialize(decompiledContentFile.Models);
		TShaderContainer.Initialize(decompiledContentFile.Shaders);
		TSoundContainer.Initialize(decompiledContentFile.Sounds);
		TTextureContainer.Initialize(decompiledContentFile.Textures);

		TShaderUniformInitializer.Initialize();

		TGame game = TGame.Construct(initialWindowTitle, initialWindowWidth, initialWindowHeight, initialWindowFullScreen);
		WarpBase.Game = game;
		TGame.Self = game;
		return game;
	}
}
