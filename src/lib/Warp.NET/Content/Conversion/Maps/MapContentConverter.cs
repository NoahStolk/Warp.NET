namespace Warp.NET.Content.Conversion.Maps;

public sealed class MapContentConverter : IContentConverter<MapBinary>
{
	public static MapBinary Construct(string inputPath)
	{
		MapData mapData = MapParser.Parse(File.ReadAllBytes(inputPath));
		return new(mapData.Entities);
	}
}
