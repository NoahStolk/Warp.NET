using Warp.NET.Content.Binaries.Data;
using Warp.NET.Content.Binaries.Parsers;
using Warp.NET.Extensions;

namespace Warp.NET.Content.Binaries.Types;

public record ModelBinary(ContentType ContentType, byte[] Contents) : IBinary<ModelBinary>
{
	public static ModelBinary Construct(string inputPath)
	{
		ModelData modelData = ObjParser.Parse(File.ReadAllBytes(inputPath));

		if (modelData.Materials.Count > 1)
			return new(ContentType.Model, ModelToBytes(modelData));

		return new(ContentType.Mesh, MeshToBytes(modelData, modelData.Materials.SelectMany(kvp => kvp.Value).ToList()));
	}

	private static byte[] ModelToBytes(ModelData modelData)
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write((ushort)modelData.Materials.Count);

		foreach (KeyValuePair<string, List<Face>> faceCollection in modelData.Materials)
		{
			bw.Write(faceCollection.Key);
			bw.Write(MeshToBytes(modelData, faceCollection.Value));
		}

		return ms.ToArray();
	}

	private static byte[] MeshToBytes(ModelData modelData, List<Face> faces)
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write((ushort)modelData.Positions.Count);
		bw.Write((ushort)modelData.Textures.Count);
		bw.Write((ushort)modelData.Normals.Count);
		bw.Write((ushort)faces.Count);

		foreach (Vector3 position in modelData.Positions)
			bw.WriteAsHalfPrecision(position);

		foreach (Vector2 texture in modelData.Textures)
			bw.WriteAsHalfPrecision(texture);

		foreach (Vector3 normal in modelData.Normals)
			bw.WriteAsHalfPrecision(normal);

		foreach (Face face in faces)
		{
			bw.Write(face.Position);
			bw.Write(face.Texture);
			bw.Write(face.Normal);
		}

		return ms.ToArray();
	}
}
