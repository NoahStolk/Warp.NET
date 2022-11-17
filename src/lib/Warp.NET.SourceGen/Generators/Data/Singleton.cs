namespace Warp.NET.SourceGen.Generators.Data;

public record Singleton
{
	public Singleton(string fullTypeName)
	{
		FullTypeName = fullTypeName;

		int separatorIndex = FullTypeName.LastIndexOf('.') + 1;
		PropertyName = separatorIndex == -1 ? FullTypeName : FullTypeName.Substring(separatorIndex, FullTypeName.Length - separatorIndex);
	}

	public string FullTypeName { get; }

	public string PropertyName { get; }
}
