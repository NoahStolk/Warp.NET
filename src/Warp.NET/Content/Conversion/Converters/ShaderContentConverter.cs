namespace Warp.NET.Content.Conversion.Converters;

public record ShaderContentConverter(byte[] Contents) : IContentConverter<ShaderContentConverter>
{
	public ContentType ContentType => ContentType.Shader;

	public static ShaderContentConverter Construct(string inputPath)
	{
		byte[] code = File.ReadAllBytes(inputPath);
		string extension = Path.GetExtension(inputPath);
		ShaderContentType shaderContentType = extension switch
		{
			".vert" => ShaderContentType.Vertex,
			".geom" => ShaderContentType.Geometry,
			".frag" => ShaderContentType.Fragment,
			_ => throw new NotSupportedException($"Extension {extension} for shaders is not supported."),
		};

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write((byte)shaderContentType);
		bw.Write((ushort)code.Length);
		bw.Write(code);
		return new(ms.ToArray());
	}

	public static ShaderSource Deconstruct(BinaryReader br)
	{
		ShaderContentType shaderContentType = (ShaderContentType)br.ReadByte();
		ushort codeLength = br.ReadUInt16();
		byte[] code = br.ReadBytes(codeLength);
		return new(shaderContentType, code);
	}
}
