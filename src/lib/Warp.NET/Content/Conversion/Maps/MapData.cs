namespace Warp.NET.Content.Conversion.Maps;

public record MapData
{
	public MapData(List<Entity> entities)
	{
		if (entities.Count == 0)
			throw new MapParseException("Map must have at least one entity.");

		Entities = entities;
	}

	public List<Entity> Entities { get; }
}
