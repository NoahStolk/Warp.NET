namespace Warp.NET.Samples.Map;

public static class TextureDictionary
{
	public static IReadOnlyDictionary<string, Texture> Textures { get; private set; } = new Dictionary<string, Texture>();

	public static void SetTextures(IReadOnlyDictionary<string, Texture> textures)
	{
		Textures = textures;
	}
}
