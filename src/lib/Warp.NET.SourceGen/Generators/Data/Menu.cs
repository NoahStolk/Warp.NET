namespace Warp.NET.SourceGen.Generators.Data;

public record Menu
{
	public Menu(string fullTypeName)
	{
		FullTypeName = fullTypeName;

		int separatorIndex = FullTypeName.LastIndexOf('.') + 1;
		PropertyName = separatorIndex == -1 ? FullTypeName : FullTypeName.Substring(separatorIndex, FullTypeName.Length - separatorIndex);
	}

	public string FullTypeName { get; }

	public string PropertyName { get; }
}
