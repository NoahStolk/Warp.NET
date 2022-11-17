using Warp.NET.Content.Conversion;

namespace Warp.NET.Content;

public record ShaderSource(ShaderContentType Type, byte[] SourceContents);
