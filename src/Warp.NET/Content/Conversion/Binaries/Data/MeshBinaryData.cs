namespace Warp.NET.Content.Conversion.Binaries.Data;

// TODO: Do not re-use Face struct.
public record MeshBinaryData(string MaterialName, IReadOnlyList<Face> Faces);
