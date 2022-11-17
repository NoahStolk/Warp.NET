namespace Warp.NET.Content.Binaries.Data;

public record ModelData(List<Vector3> Positions, List<Vector2> Textures, List<Vector3> Normals, Dictionary<string, List<Face>> Materials);
