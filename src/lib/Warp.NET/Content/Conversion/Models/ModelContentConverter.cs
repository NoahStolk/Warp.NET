namespace Warp.NET.Content.Conversion.Models;

public sealed class ModelContentConverter : IContentConverter<ModelBinary>
{
	public static ModelBinary Construct(string inputPath)
	{
		ModelData modelData = ObjParser.Parse(File.ReadAllBytes(inputPath));
		return new(modelData.Positions, modelData.Textures, modelData.Normals, modelData.Meshes);
	}
}
