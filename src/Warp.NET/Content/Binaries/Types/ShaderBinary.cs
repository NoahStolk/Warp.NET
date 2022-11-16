namespace Warp.NET.Content.Binaries.Types;

public class ShaderBinary : IBinary
{
	private string? _code;

	public ContentType ReadFromPath(string path)
	{
		_code = File.ReadAllText(path);

		string extension = Path.GetExtension(path);
		return extension switch
		{
			".vert" => ContentType.VertexShader,
			".geom" => ContentType.GeometryShader,
			".frag" => ContentType.FragmentShader,
			_ => throw new NotSupportedException($"Extension {extension} for shaders is not supported."),
		};
	}

	public byte[] ToBytes(ContentType contentType)
	{
		if (contentType is not (ContentType.VertexShader or ContentType.GeometryShader or ContentType.FragmentShader))
			throw new NotSupportedException($"Calling {nameof(ShaderBinary)}.{nameof(ToBytes)} with {nameof(ContentType)} '{contentType}' is not supported.");

		if (_code == null)
			throw new InvalidOperationException("Binary is not initialized.");

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(Encoding.UTF8.GetBytes(_code));
		return ms.ToArray();
	}
}
