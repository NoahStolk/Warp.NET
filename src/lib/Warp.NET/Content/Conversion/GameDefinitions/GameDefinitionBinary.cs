namespace Warp.NET.Content.Conversion.GameDefinitions;

internal record GameDefinitionBinary(List<Entity> Entities) : IBinary<GameDefinitionBinary>
{
	public ContentType ContentType => ContentType.GameDefinition;

	public byte[] ToBytes()
	{
		throw new NotImplementedException();
	}

	public static GameDefinitionBinary FromStream(BinaryReader br)
	{
		throw new NotImplementedException();
	}
}
