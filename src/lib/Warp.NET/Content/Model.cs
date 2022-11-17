namespace Warp.NET.Content;

public class Model
{
	public Model(Dictionary<string, Mesh> meshes)
	{
		Meshes = meshes;
	}

	public Dictionary<string, Mesh> Meshes { get; }
}
