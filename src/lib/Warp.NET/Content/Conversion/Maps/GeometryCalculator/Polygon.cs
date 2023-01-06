namespace Warp.NET.Content.Conversion.Maps.GeometryCalculator;

public class Polygon
{
	private static readonly Texture _defaultTexture = new(1, 1, new byte[] { 255, 127, 0, 255 });

	public Polygon(Plane plane, Face face, IReadOnlyDictionary<string, Texture> textures)
	{
		Face = face;
		TextureScales = new();
		Vertices = new();
		Plane = plane;
		Texture = textures.TryGetValue(face.TextureName, out Texture? texture) ? texture : _defaultTexture;
	}

	public List<Vector3> Vertices { get; }
	public List<Vector2> TextureScales { get; }
	public Plane Plane { get; }
	public Face Face { get; }
	public Texture Texture { get; }
}
