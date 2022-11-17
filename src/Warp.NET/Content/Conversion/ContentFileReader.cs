using Warp.NET.Content.Conversion.Binaries;
using Warp.NET.Content.Conversion.Binaries.Data;

namespace Warp.NET.Content.Conversion;

public static class ContentFileReader
{
	/// <summary>
	/// Reads the binary content file at <paramref name="contentFilePath"/>, which is generated by the <see cref="ContentFileWriter"/>, then constructs and returns the content objects.
	/// </summary>
	public static DecompiledContentFile Read(string contentFilePath)
	{
		if (!File.Exists(contentFilePath))
			throw new InvalidOperationException("Content file does not exist.");

		using FileStream fs = new(contentFilePath, FileMode.Open);
		using BinaryReader br = new(fs);
		ushort tocEntryCount = br.ReadUInt16();

		TocEntry[] tocEntries = new TocEntry[tocEntryCount];
		for (int i = 0; i < tocEntryCount; i++)
		{
			ContentType contentType = (ContentType)br.ReadByte();
			string name = br.ReadString();
			uint length = br.ReadUInt32();

			tocEntries[i] = new(contentType, name, length);
		}

		Dictionary<string, Model> models = new();
		Dictionary<string, Shader> shaders = new();
		Dictionary<string, Sound> sounds = new();
		Dictionary<string, Texture> textures = new();

		Dictionary<string, ShaderSourceCollection> shaderSourceCollections = new();

		foreach (TocEntry tocEntry in tocEntries)
		{
			long pos = br.BaseStream.Position;

			switch (tocEntry.ContentType)
			{
				case ContentType.Model: models[tocEntry.Name] = GetModel(br); break;
				case ContentType.Shader: SetShaderSource(shaderSourceCollections, br, tocEntry.Name); break;
				case ContentType.Sound: sounds[tocEntry.Name] = GetSound(br); break;
				case ContentType.Texture: textures[tocEntry.Name] = GetTexture(br); break;
				default: throw new NotSupportedException($"Reading {nameof(ContentType)} '{tocEntry.ContentType}' is not supported.");
			}

			if (br.BaseStream.Position != pos + tocEntry.Length)
				throw new InvalidOperationException($"Toc entry is {tocEntry.Length} bytes, but binary reader read {br.BaseStream.Position - pos} bytes!");
		}

		foreach (KeyValuePair<string, ShaderSourceCollection> shaderSource in shaderSourceCollections)
		{
			if (shaderSource.Value.VertexCode == null)
				throw new InvalidOperationException($"Vertex shader source for '{shaderSource.Key}' not found.");
			if (shaderSource.Value.FragmentCode == null)
				throw new InvalidOperationException($"Fragment shader source for '{shaderSource.Key}' not found.");

			shaders[shaderSource.Key] = new(shaderSource.Value.VertexCode, shaderSource.Value.GeometryCode, shaderSource.Value.FragmentCode);
		}

		return new(models, shaders, sounds, textures);
	}

	private static Model GetModel(BinaryReader br)
	{
		ModelBinary modelBinary = ModelBinary.FromStream(br);
		return new(modelBinary.Meshes.ToDictionary(m => m.MaterialName, m => GetMesh(modelBinary, m)));
	}

	private static Mesh GetMesh(ModelBinary modelBinary, MeshBinaryData meshBinaryData)
	{
		Vertex[] outVertices = new Vertex[meshBinaryData.Faces.Count];
		uint[] outFaces = new uint[meshBinaryData.Faces.Count];
		for (int j = 0; j < meshBinaryData.Faces.Count; j++)
		{
			ushort t = meshBinaryData.Faces[j].Texture;

			outVertices[j] = new(
			modelBinary.Positions[meshBinaryData.Faces[j].Position - 1],
			modelBinary.Textures.Count > t - 1 && t > 0 ? modelBinary.Textures[t - 1] : default, // TODO: Separate face type?
			modelBinary.Normals[meshBinaryData.Faces[j].Normal - 1]);
			outFaces[j] = (ushort)j;
		}

		return new(outVertices, outFaces, TriangleRenderMode.Triangles);
	}

	private static void SetShaderSource(IDictionary<string, ShaderSourceCollection> shaderSources, BinaryReader br, string shaderName)
	{
		if (!shaderSources.TryGetValue(shaderName, out ShaderSourceCollection? value))
		{
			value = new();
			shaderSources.Add(shaderName, value);
		}

		ShaderBinary shaderBinary = ShaderBinary.FromStream(br);
		string code = Encoding.UTF8.GetString(shaderBinary.Code);
		switch (shaderBinary.ShaderContentType)
		{
			case ShaderContentType.Vertex:
				value.VertexCode = code;
				break;
			case ShaderContentType.Geometry:
				value.GeometryCode = code;
				break;
			case ShaderContentType.Fragment:
				value.FragmentCode = code;
				break;
			default:
				throw new NotSupportedException($"{nameof(ShaderContentType)} '{shaderBinary.ShaderContentType}' is not supported.");
		}
	}

	private static Sound GetSound(BinaryReader br)
	{
		SoundBinary soundBinary = SoundBinary.FromStream(br);
		return new(soundBinary.Channels, soundBinary.SampleRate, soundBinary.BitsPerSample, soundBinary.Data.Length, soundBinary.Data);
	}

	private static Texture GetTexture(BinaryReader br)
	{
		TextureBinary textureBinary = TextureBinary.FromStream(br);
		return new(textureBinary.Width, textureBinary.Height, textureBinary.ColorData);
	}

	private sealed class ShaderSourceCollection
	{
		public string? VertexCode { get; set; }
		public string? GeometryCode { get; set; }
		public string? FragmentCode { get; set; }
	}
}
