namespace Warp.NET.Content.Binaries;

public enum ContentType
{
	/// <summary>
	/// Vertex shader source code.
	/// </summary>
	VertexShader = 0x00,

	/// <summary>
	/// Geometry shader source code.
	/// </summary>
	GeometryShader = 0x01,

	/// <summary>
	/// Fragment shader source code.
	/// </summary>
	FragmentShader = 0x02,

	/// <summary>
	/// Texture where every pixel is stored in 1 bit and therefore only supports 2 colors (white and transparent) -- typically used for fonts.
	/// </summary>
	TextureW1 = 0x03,

	/// <summary>
	/// Texture where every pixel consists of 8-bit RGB color components. Alpha is not stored and should be set to 255.
	/// </summary>
	TextureRgb24 = 0x04,

	/// <summary>
	/// Texture where every pixel consists of 8-bit RGBA color components.
	/// </summary>
	TextureRgba32 = 0x05,

	/// <summary>
	/// Texture where every pixel consists of a single 8-bit color component used for R, G, and B channels (grayscale). Alpha is not stored and should be set to 255.
	/// </summary>
	TextureW8 = 0x06,

	/// <summary>
	/// Texture where every pixel consists of a single 8-bit color component used for R, G, and B channels (grayscale), and an 8-bit color component used for the alpha channel.
	/// </summary>
	TextureWa16 = 0x07,

	/// <summary>
	/// Raw wave audio contents.
	/// </summary>
	Sound = 0x08,

	/// <summary>
	/// Binary data for 3D meshes.
	/// </summary>
	Mesh = 0x09,

	/// <summary>
	/// Binary data for 3D models (multiple meshes).
	/// </summary>
	Model = 0x0A,
}
