namespace Warp.NET.Content.Conversion.Maps.GeometryCalculator;

public class Polygon
{
	public Polygon(Plane plane, Face face, IReadOnlyDictionary<string, Texture> textures, Texture fallbackTexture)
	{
		Face = face;
		TextureScales = new();
		Vertices = new();
		Plane = plane;
		Texture = textures.TryGetValue(face.TextureName, out Texture? texture) ? texture : fallbackTexture;
	}

	public List<Vector3> Vertices { get; }
	public List<Vector2> TextureScales { get; }
	public Plane Plane { get; }
	public Face Face { get; }
	public Texture Texture { get; }
}
