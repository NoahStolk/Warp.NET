namespace Warp.NET.Content;

public class Map
{
	public Map(List<(Mesh Mesh, Texture Texture)> geometry)
	{
		if (geometry.Count == 0)
			throw new InvalidOperationException("Map must have geometry.");

		Meshes = geometry;
	}

	public List<(Mesh Mesh, Texture Texture)> Meshes { get; }
}
