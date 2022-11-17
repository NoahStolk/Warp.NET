using Warp.NET.Content.Conversion.Converters;
using Warp.NET.Utils;

namespace Warp.NET.Content.Conversion;

/// <summary>
/// Turns all content source files into binary and writes everything to a single content file.
/// </summary>
public static class ContentFileWriter
{
	public static void GenerateContentFile(string inputContentRootDirectory, string outputContentFilePath)
	{
		string[] contentPaths = Directory.GetFiles(inputContentRootDirectory, "*.*", SearchOption.AllDirectories);

		List<TocEntry> tocEntries = new();
		using MemoryStream dataMemory = new();
		using BinaryWriter dataWriter = new(dataMemory);

		foreach (string path in contentPaths.Where(p => Path.GetExtension(p) == ".tga"))
			Write<TextureContentConverter>(path, tocEntries, dataWriter);

		foreach (string path in contentPaths.Where(p => Path.GetExtension(p) is ".vert" or ".geom" or ".frag"))
			Write<ShaderContentConverter>(path, tocEntries, dataWriter);

		foreach (string path in contentPaths.Where(p => Path.GetExtension(p) == ".wav"))
			Write<SoundContentConverter>(path, tocEntries, dataWriter);

		foreach (string path in contentPaths.Where(p => Path.GetExtension(p) == ".obj"))
			Write<ModelContentConverter>(path, tocEntries, dataWriter);

		using MemoryStream contentFile = new();
		using BinaryWriter contentFileWriter = new(contentFile);

		contentFileWriter.Write((ushort)tocEntries.Count);
		foreach (TocEntry tocEntry in tocEntries)
		{
			contentFileWriter.Write((byte)tocEntry.ContentType);
			contentFileWriter.Write(tocEntry.Name);
			contentFileWriter.Write(tocEntry.Length);
		}

		contentFileWriter.Write(dataMemory.ToArray());

		File.WriteAllBytes(outputContentFilePath, contentFile.ToArray());
	}

	private static void Write<TBinary>(string path, List<TocEntry> toc, BinaryWriter dataWriter)
		where TBinary : IContentConverter<TBinary>
	{
		if (!FileNameUtils.PathIsValid(path))
			return;

		TBinary binary = TBinary.Construct(path);
		dataWriter.Write(binary.Contents);
		toc.Add(new(binary.ContentType, Path.GetFileNameWithoutExtension(path), (uint)binary.Contents.Length));
	}
}
