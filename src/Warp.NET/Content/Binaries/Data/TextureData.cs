namespace Warp.NET.Content.Binaries.Data;

/// <summary>
/// Represents data parsed from a texture format, such as a .tga file.
/// </summary>
public record TextureData(ushort Width, ushort Height, byte[] ColorData);
