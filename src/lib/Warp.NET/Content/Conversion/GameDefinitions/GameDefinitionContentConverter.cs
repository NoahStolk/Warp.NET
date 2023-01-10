namespace Warp.NET.Content.Conversion.GameDefinitions;

internal sealed class GameDefinitionContentConverter : IContentConverter<GameDefinitionBinary>
{
	public static GameDefinitionBinary Construct(string inputPath)
	{
		GameDefinitionData gameDefinitionData = GameDefinitionParser.Parse(File.ReadAllBytes(inputPath));
		return new(gameDefinitionData.GameClasses);
	}
}
