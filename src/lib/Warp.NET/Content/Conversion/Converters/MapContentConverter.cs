using Warp.NET.Content.Conversion.Binaries;
using Warp.NET.Content.Conversion.Data;

namespace Warp.NET.Content.Conversion.Converters;

public sealed class MapContentConverter : IContentConverter<MapBinary>
{
	public static MapBinary Construct(string inputPath)
	{
		MapData mapData = ObjParser.Parse(File.ReadAllBytes(inputPath));
		return new(mapData.Positions.ToArray(), mapData.Textures.ToArray(), mapData.Normals.ToArray(), mapData.Meshes.Select(m => new MeshBinaryData(m.MaterialName, m.Faces)).ToList());
	}
}
