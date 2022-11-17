using Warp.NET.Content.Conversion.Binaries;

namespace Warp.NET.Content.Conversion.Converters;

public sealed class ShaderContentConverter : IContentConverter<ShaderBinary>
{
	public static ShaderBinary Construct(string inputPath)
	{
		string extension = Path.GetExtension(inputPath);
		ShaderContentType shaderContentType = extension switch
		{
			".vert" => ShaderContentType.Vertex,
			".geom" => ShaderContentType.Geometry,
			".frag" => ShaderContentType.Fragment,
			_ => throw new NotSupportedException($"Extension {extension} for shaders is not supported."),
		};
		byte[] code = File.ReadAllBytes(inputPath);

		return new(shaderContentType, code);
	}
}
