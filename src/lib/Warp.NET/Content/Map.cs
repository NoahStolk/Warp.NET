using Warp.NET.Content.Conversion.Maps;

namespace Warp.NET.Content;

public class Map
{
	public Map(List<Entity> entities)
	{
		if (entities.Count == 0)
			throw new InvalidOperationException("Map must have at least one entity.");

		Entities = entities;
	}

	// TODO: Don't re-use Entity type.
	public List<Entity> Entities { get; }
}
