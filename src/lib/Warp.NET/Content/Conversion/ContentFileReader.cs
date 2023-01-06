using Warp.NET.Content.Conversion.Blobs;
using Warp.NET.Content.Conversion.Charsets;
using Warp.NET.Content.Conversion.Maps;
using Warp.NET.Content.Conversion.Models;
using Warp.NET.Content.Conversion.Shaders;
using Warp.NET.Content.Conversion.Sounds;
using Warp.NET.Content.Conversion.Textures;

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

		Dictionary<string, Blob> blobs = new();
		Dictionary<string, Charset> charsets = new();
		Dictionary<string, Map> maps = new();
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
				case ContentType.Blob: blobs[tocEntry.Name] = GetBlob(br); break;
				case ContentType.Charset: charsets[tocEntry.Name] = GetCharset(br); break;
				case ContentType.Map: maps[tocEntry.Name] = GetMap(br); break;
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

		return new(blobs, charsets, maps, models, shaders, sounds, textures);
	}

	private static Blob GetBlob(BinaryReader br)
	{
		BlobBinary blobBinary = BlobBinary.FromStream(br);
		return new(blobBinary.Data);
	}

	private static Charset GetCharset(BinaryReader br)
	{
		CharsetBinary charsetBinary = CharsetBinary.FromStream(br);
		return new(charsetBinary.Characters);
	}

	private static Map GetMap(BinaryReader br)
	{
		MapBinary mapBinary = MapBinary.FromStream(br);
		return new(mapBinary.Entities);
	}

	private static Model GetModel(BinaryReader br)
	{
		ModelBinary modelBinary = ModelBinary.FromStream(br);
		return new(modelBinary.Meshes.ToDictionary(m => m.MaterialName, m => GetMesh(modelBinary, m)));

		static Mesh GetMesh(ModelBinary modelBinary, MeshData meshData)
		{
			Vertex[] outVertices = new Vertex[meshData.Faces.Count];
			uint[] outFaces = new uint[meshData.Faces.Count];
			for (int j = 0; j < meshData.Faces.Count; j++)
			{
				ushort t = meshData.Faces[j].Texture;

				outVertices[j] = new(
				modelBinary.Positions[meshData.Faces[j].Position - 1],
				modelBinary.Textures.Count > t - 1 && t > 0 ? modelBinary.Textures[t - 1] : default, // TODO: Separate face type?
				modelBinary.Normals[meshData.Faces[j].Normal - 1]);
				outFaces[j] = (ushort)j;
			}

			return new(outVertices, outFaces);
		}
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
