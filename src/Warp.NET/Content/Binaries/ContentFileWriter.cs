using Warp.NET.Content.Binaries.Types;
using Warp.NET.Utils;

namespace Warp.NET.Content.Binaries;

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
			Write<TextureBinary>(path, tocEntries, dataWriter);

		foreach (string path in contentPaths.Where(p => Path.GetExtension(p) is ".vert" or ".geom" or ".frag"))
			Write<ShaderBinary>(path, tocEntries, dataWriter);

		foreach (string path in contentPaths.Where(p => Path.GetExtension(p) == ".wav"))
			Write<SoundBinary>(path, tocEntries, dataWriter);

		foreach (string path in contentPaths.Where(p => Path.GetExtension(p) == ".obj"))
			Write<ModelBinary>(path, tocEntries, dataWriter);

		using MemoryStream tocMemory = new();
		using BinaryWriter tocWriter = new(tocMemory);
		foreach (TocEntry tocEntry in tocEntries)
		{
			tocWriter.Write((byte)tocEntry.ContentType);
			tocWriter.Write(tocEntry.Name);
			tocWriter.Write(tocEntry.Length);
		}

		byte[] final = new byte[sizeof(ushort) + tocMemory.Length + dataMemory.Length];
		Buffer.BlockCopy(BitConverter.GetBytes((ushort)tocMemory.Length), 0, final, 0, sizeof(ushort));
		Buffer.BlockCopy(tocMemory.GetBuffer(), 0, final, sizeof(ushort), (int)tocMemory.Length);
		Buffer.BlockCopy(dataMemory.GetBuffer(), 0, final, sizeof(ushort) + (int)tocMemory.Length, (int)dataMemory.Length);

		File.WriteAllBytes(outputContentFilePath, final);
	}

	private static void Write<TBinary>(string path, List<TocEntry> toc, BinaryWriter dataWriter)
		where TBinary : IBinary, new()
	{
		if (!FileNameUtils.PathIsValid(path))
			return;

		TBinary binary = new();
		ContentType contentType = binary.ReadFromPath(path);
		byte[] bytes = binary.ToBytes(contentType);
		dataWriter.Write(bytes);
		toc.Add(new(contentType, Path.GetFileNameWithoutExtension(path), (uint)bytes.Length));
	}
}
