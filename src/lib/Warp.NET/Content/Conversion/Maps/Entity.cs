namespace Warp.NET.Content.Conversion.Maps;

public class Entity
{
	public Entity(Dictionary<string, string> properties, List<Brush> brushes)
	{
		Properties = properties;
		Brushes = brushes;
	}

	public Dictionary<string, string> Properties { get; }
	public List<Brush> Brushes { get; }
}
