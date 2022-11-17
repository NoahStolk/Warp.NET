using Warp.NET.Content.Binaries;

namespace Warp.NET.Content;

public record ShaderSource(ShaderContentType Type, byte[] SourceContents);
