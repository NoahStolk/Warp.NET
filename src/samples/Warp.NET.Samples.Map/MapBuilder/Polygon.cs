using System.Numerics;
using Warp.NET.Content;

namespace Warp.NET.Samples.Map.MapBuilder;

public class Polygon
{
    public Polygon(Plane plane, Face face)
    {
        Face = face;
        TextureScales = new();
        Vertices = new();
        Plane = plane;
        Texture = TextureDictionary.Textures.TryGetValue(face.TextureName, out Texture? texture) ? texture : Textures.Placeholder;
    }

    public List<Vector3> Vertices { get; }
    public List<Vector2> TextureScales { get; }
    public Plane Plane { get; }
    public Face Face { get; }
    public Texture Texture { get; }
}
