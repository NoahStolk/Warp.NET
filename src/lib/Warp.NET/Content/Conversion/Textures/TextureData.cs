namespace Warp.NET.Content.Conversion.Textures;

/// <summary>
/// Represents data parsed from a texture format, such as a .tga file.
/// </summary>
internal record TextureData(ushort Width, ushort Height, byte[] ColorData);
