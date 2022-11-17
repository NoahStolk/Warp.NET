using Warp.NET.Content.Conversion.Binaries;
using Warp.NET.Content.Conversion.Binaries.Data;
using Warp.NET.Content.Conversion.Data;
using Warp.NET.Content.Conversion.Parsers;

namespace Warp.NET.Content.Conversion.Converters;

public sealed class ModelContentConverter : IContentConverter<ModelBinary>
{
	public static ModelBinary Construct(string inputPath)
	{
		ModelData modelData = ObjParser.Parse(File.ReadAllBytes(inputPath));
		return new(modelData.Positions.ToArray(), modelData.Textures.ToArray(), modelData.Normals.ToArray(), modelData.Meshes.Select(m => new MeshBinaryData(m.MaterialName, m.Faces)).ToList());
	}
}
