namespace Warp.NET.Content.Conversion;

public record DecompiledContentFile(
	IReadOnlyDictionary<string, Blob> Blobs,
	IReadOnlyDictionary<string, Charset> Charsets,
	IReadOnlyDictionary<string, Map> Maps,
	IReadOnlyDictionary<string, Model> Models,
	IReadOnlyDictionary<string, Shader> Shaders,
	IReadOnlyDictionary<string, Sound> Sounds,
	IReadOnlyDictionary<string, Texture> Textures);
