namespace Warp.NET.SourceGen.Generators.Data;

public record TypeName(string Type, string Namespace)
{
	public string Type { get; } = Type;

	public string Namespace { get; } = Namespace;

	public string FullName => $"{Namespace}.{Type}";
}
