using Warp.NET.Content.Binaries.Parsers.Exceptions;

namespace Warp.NET.Content.Binaries.Data;

public record ModelData
{
	public ModelData(List<Vector3> positions, List<Vector2> textures, List<Vector3> normals, Dictionary<string, List<Face>> meshes)
	{
		if (meshes.Count == 0)
			throw new ObjParseException("Model must have at least one mesh.");

		Positions = positions;
		Textures = textures;
		Normals = normals;
		Meshes = meshes;
	}

	public List<Vector3> Positions { get; }
	public List<Vector2> Textures { get; }
	public List<Vector3> Normals { get; }
	public Dictionary<string, List<Face>> Meshes { get; }
}
