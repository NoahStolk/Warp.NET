using System.Globalization;
using Warp.NET.Extensions;

namespace Warp.NET.Content.Binaries.Types;

public class ModelBinary : IBinary
{
	private Context? _context;

	public ContentType ReadFromPath(string path)
	{
		string text = File.ReadAllText(path);
		string[] lines = text.Split('\n');

		_context = new();

		string useMaterial = string.Empty;
		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			string[] values = line.Split(' ');

			switch (values[0])
			{
				case "v": _context.Positions.Add(new(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]), ParseVertexFloat(values[3]))); break;
				case "vt": _context.Textures.Add(new(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]))); break;
				case "vn": _context.Normals.Add(new(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]), ParseVertexFloat(values[3]))); break;
				case "usemtl": useMaterial = values[1].Trim(); break;
				case "f":
					if (values.Length < 4) // Invalid face.
						break;

					string[] rawIndices = values[1..];
					List<Face> faces = new();
					for (int j = 0; j < rawIndices.Length; j++)
					{
						string[] indexEntries = rawIndices[j].Split('/');
						faces.Add(new(ushort.Parse(indexEntries[0]), ushort.TryParse(indexEntries[1], out ushort texture) ? texture : (ushort)0, ushort.Parse(indexEntries[2])));

						if (j >= 3)
						{
							faces.Add(faces[0]);
							faces.Add(faces[j - 1]);
						}
					}

					foreach (Face face in faces)
					{
						if (_context.GroupsOfFaces.TryGetValue(useMaterial, out List<Face>? value))
							value.Add(face);
						else
							_context.GroupsOfFaces.Add(useMaterial, new() { face });
					}

					break;
			}
		}

		return _context.GroupsOfFaces.Count > 1 ? ContentType.Model : ContentType.Mesh;

		static float ParseVertexFloat(string value)
			=> (float)double.Parse(value, NumberStyles.Float);
	}

	public byte[] ToBytes(ContentType contentType)
	{
		if (_context == null)
			throw new InvalidOperationException("Binary is not initialized.");

		return contentType switch
		{
			ContentType.Model => ModelToBytes(_context),
			ContentType.Mesh => MeshToBytes(_context, _context.GroupsOfFaces.SelectMany(kvp => kvp.Value).ToList()),
			_ => throw new NotSupportedException($"Calling {nameof(ModelBinary)}.{nameof(ToBytes)} with {nameof(ContentType)} '{contentType}' is not supported."),
		};
	}

	private static byte[] ModelToBytes(Context context)
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write((ushort)context.GroupsOfFaces.Count);

		foreach (KeyValuePair<string, List<Face>> faceCollection in context.GroupsOfFaces)
		{
			bw.Write(faceCollection.Key);
			bw.Write(MeshToBytes(context, faceCollection.Value));
		}

		return ms.ToArray();
	}

	private static byte[] MeshToBytes(Context context, List<Face> faces)
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write((ushort)context.Positions.Count);
		bw.Write((ushort)context.Textures.Count);
		bw.Write((ushort)context.Normals.Count);
		bw.Write((ushort)faces.Count);

		foreach (Vector3 position in context.Positions)
			bw.WriteAsHalfPrecision(position);

		foreach (Vector2 texture in context.Textures)
			bw.WriteAsHalfPrecision(texture);

		foreach (Vector3 normal in context.Normals)
			bw.WriteAsHalfPrecision(normal);

		foreach (Face face in faces)
		{
			bw.Write(face.Position);
			bw.Write(face.Texture);
			bw.Write(face.Normal);
		}

		return ms.ToArray();
	}

	private sealed class Context
	{
		public List<Vector3> Positions { get; } = new();
		public List<Vector2> Textures { get; } = new();
		public List<Vector3> Normals { get; } = new();
		public Dictionary<string, List<Face>> GroupsOfFaces { get; } = new();
	}
}
