namespace Warp.NET.Content.Conversion.Data;

// TODO: Do not re-use Face struct.
public record MeshData(string MaterialName, IReadOnlyList<Face> Faces);
