using System.Globalization;
using Warp.NET.Content.Binaries.Data;

namespace Warp.NET.Content.Binaries.Parsers;

public static class ObjParser
{
	public static ModelData Parse(byte[] fileContents)
	{
		List<Vector3> positions = new();
		List<Vector2> textures = new();
		List<Vector3> normals = new();
		Dictionary<string, List<Face>> materials = new();

		string text = Encoding.UTF8.GetString(fileContents);
		string[] lines = text.Split('\n');

		string useMaterial = string.Empty;
		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			string[] values = line.Split(' ');

			switch (values[0])
			{
				case "v": positions.Add(new(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]), ParseVertexFloat(values[3]))); break;
				case "vt": textures.Add(new(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]))); break;
				case "vn": normals.Add(new(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]), ParseVertexFloat(values[3]))); break;
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
						if (materials.TryGetValue(useMaterial, out List<Face>? value))
							value.Add(face);
						else
							materials.Add(useMaterial, new() { face });
					}

					break;
			}
		}

		return new(positions, textures, normals, materials);
	}

	private static float ParseVertexFloat(string value) => (float)double.Parse(value, NumberStyles.Float);
}