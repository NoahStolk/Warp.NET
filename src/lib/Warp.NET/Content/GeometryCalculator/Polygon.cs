namespace Warp.NET.Content.GeometryCalculator;

internal class Polygon
{
	public Polygon(Plane plane, Face face, IReadOnlyDictionary<string, Texture> textures, Texture fallbackTexture)
	{
		Plane = plane;
		Face = face;
		Texture = textures.TryGetValue(face.TextureName, out Texture? texture) ? texture : fallbackTexture;

		Vertices = new();
		TextureScales = new();
	}

	public Plane Plane { get; }
	public Face Face { get; }
	public Texture Texture { get; }

	public List<Vector3> Vertices { get; }
	public List<Vector2> TextureScales { get; }
}
