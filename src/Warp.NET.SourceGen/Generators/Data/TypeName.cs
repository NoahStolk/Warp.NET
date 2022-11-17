using Warp.NET.SourceGen.Utils;

namespace Warp.NET.SourceGen.Generators.Data;

public record TypeName(string Type, string Namespace = Constants.RootNamespace)
{
	public string Type { get; } = Type;

	public string Namespace { get; } = Namespace;

	public string FullName => $"{Namespace}.{Type}";
}
