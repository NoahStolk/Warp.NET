namespace Warp.NET.Content;

public class Model
{
	public Model(Dictionary<string, Mesh> meshes)
	{
		if (meshes.Count == 0)
			throw new InvalidOperationException("Model must have meshes.");

		Meshes = meshes;
		MainMesh = meshes.Values.First();
	}

	public Dictionary<string, Mesh> Meshes { get; }

	public Mesh MainMesh { get; }
}
