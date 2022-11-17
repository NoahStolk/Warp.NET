using Warp.NET.Content.Binaries.Data;
using Warp.NET.Content.Binaries.Parsers;
using Warp.NET.Extensions;

namespace Warp.NET.Content.Binaries.Types;

public class ModelBinary : IBinary
{
	private ModelData? _modelData;

	public ContentType ReadFromPath(string path)
	{
		_modelData = ObjParser.Parse(File.ReadAllBytes(path));

		return _modelData.Materials.Count > 1 ? ContentType.Model : ContentType.Mesh;
	}

	public byte[] ToBytes(ContentType contentType)
	{
		if (_modelData == null)
			throw new InvalidOperationException("Binary is not initialized.");

		return contentType switch
		{
			ContentType.Model => ModelToBytes(_modelData),
			ContentType.Mesh => MeshToBytes(_modelData, _modelData.Materials.SelectMany(kvp => kvp.Value).ToList()),
			_ => throw new NotSupportedException($"Calling {nameof(ModelBinary)}.{nameof(ToBytes)} with {nameof(ContentType)} '{contentType}' is not supported."),
		};
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
