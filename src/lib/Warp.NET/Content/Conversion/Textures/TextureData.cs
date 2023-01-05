namespace Warp.NET.Content.Conversion.Textures;

/// <summary>
/// Represents data parsed from a texture format, such as a .tga file.
/// </summary>
public record TextureData(ushort Width, ushort Height, byte[] ColorData);
