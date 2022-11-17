using Warp.NET.Content.Binaries.Data;
using Warp.NET.Content.Binaries.Parsers;
using Warp.NET.Extensions;

namespace Warp.NET.Content.Binaries.Types;

public record ModelBinary(byte[] Contents) : IBinary<ModelBinary>
{
	public ContentType ContentType => ContentType.Model;

	public static ModelBinary Construct(string inputPath)
	{
		ModelData modelData = ObjParser.Parse(File.ReadAllBytes(inputPath));

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		bw.Write((ushort)modelData.Positions.Count);
		foreach (Vector3 position in modelData.Positions)
			bw.WriteAsHalfPrecision(position);

		bw.Write((ushort)modelData.Textures.Count);
		foreach (Vector2 texture in modelData.Textures)
			bw.WriteAsHalfPrecision(texture);

		bw.Write((ushort)modelData.Normals.Count);
		foreach (Vector3 normal in modelData.Normals)
			bw.WriteAsHalfPrecision(normal);

		bw.Write((ushort)modelData.Meshes.Count);
		foreach (KeyValuePair<string, List<Face>> mesh in modelData.Meshes)
		{
			bw.Write(mesh.Key);

			bw.Write((ushort)mesh.Value.Count);
			foreach (Face face in mesh.Value)
			{
				bw.Write(face.Position);
				bw.Write(face.Texture);
				bw.Write(face.Normal);
			}
		}

		return new(ms.ToArray());
	}

	public static Model Deconstruct(BinaryReader br)
	{
		Span<Vector3> positions = stackalloc Vector3[br.ReadUInt16()];
		for (int i = 0; i < positions.Length; i++)
			positions[i] = br.ReadVector3AsHalfPrecision();

		Span<Vector2> textures = stackalloc Vector2[br.ReadUInt16()];
		for (int i = 0; i < textures.Length; i++)
			textures[i] = br.ReadVector2AsHalfPrecision();

		Span<Vector3> normals = stackalloc Vector3[br.ReadUInt16()];
		for (int i = 0; i < normals.Length; i++)
			normals[i] = br.ReadVector3AsHalfPrecision();

		ushort meshCount = br.ReadUInt16();
		Dictionary<string, Mesh> meshes = new();
		for (int i = 0; i < meshCount; i++)
		{
			string useMaterial = br.ReadString();
			Mesh mesh = GetMesh(positions, textures, normals);

			meshes.Add(useMaterial, mesh);
		}

		return new(meshes);

		Mesh GetMesh(Span<Vector3> positions, Span<Vector2> textures, Span<Vector3> normals)
		{
			Span<Face> faces = stackalloc Face[br.ReadUInt16()];
			for (int j = 0; j < faces.Length; j++)
				faces[j] = new(br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16());

			Vertex[] outVertices = new Vertex[faces.Length];
			uint[] outFaces = new uint[faces.Length];
			for (int j = 0; j < faces.Length; j++)
			{
				ushort t = faces[j].Texture;

				outVertices[j] = new(
				positions[faces[j].Position - 1],
				textures.Length > t - 1 && t > 0 ? textures[t - 1] : default, // TODO: Separate face type?
				normals[faces[j].Normal - 1]);
				outFaces[j] = (ushort)j;
			}

			return new(outVertices, outFaces, TriangleRenderMode.Triangles);
		}
	}
}
