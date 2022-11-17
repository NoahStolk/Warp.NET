namespace Warp.NET.Content.Binaries.Types;

public record ShaderBinary(ContentType ContentType, byte[] Contents) : IBinary<ShaderBinary>
{
	private static readonly Encoding _encoding = Encoding.UTF8;

	public static ShaderBinary Construct(string inputPath)
	{
		string code = File.ReadAllText(inputPath, _encoding);

		string extension = Path.GetExtension(inputPath);
		ContentType contentType = extension switch
		{
			".vert" => ContentType.VertexShader,
			".geom" => ContentType.GeometryShader,
			".frag" => ContentType.FragmentShader,
			_ => throw new NotSupportedException($"Extension {extension} for shaders is not supported."),
		};

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(_encoding.GetBytes(code));
		return new(contentType, ms.ToArray());
	}
}
