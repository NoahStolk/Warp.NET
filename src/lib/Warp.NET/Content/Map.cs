namespace Warp.NET.Content;

public class Map
{
	public Map(List<Entity> entities)
	{
		if (entities.Count == 0)
			throw new InvalidOperationException("Map must have at least one entity.");

		Entities = entities;
	}

	public List<Entity> Entities { get; }
}
