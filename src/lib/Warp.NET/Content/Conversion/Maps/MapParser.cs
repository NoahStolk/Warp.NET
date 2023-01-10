namespace Warp.NET.Content.Conversion.Maps;

internal static class MapParser
{
	public static MapData Parse(byte[] fileContents)
	{
		List<Entity> entities = new();

		StringIterator stringIterator = new(Encoding.UTF8.GetString(fileContents));

		do
		{
			if (stringIterator.IsNext("//"))
				stringIterator.ReadUntil("\n", false);
			else if (stringIterator.IsNext("{"))
				entities.Add(ReadEntity(stringIterator));
		}
		while (stringIterator.Advance());

		return new(entities);
	}

	private static Entity ReadEntity(StringIterator stringIterator)
	{
		stringIterator.Advance();

		Dictionary<string, string> properties = new();
		List<Brush> brushes = new();

		do
		{
			if (stringIterator.IsNext("{"))
			{
				brushes.Add(ReadBrush(stringIterator));
			}
			else if (stringIterator.IsNext("\""))
			{
				properties.Add(stringIterator.ReadBetween("\"", "\"", true), stringIterator.ReadBetween("\"", "\"", true));
			}
			else if (stringIterator.IsNext("//"))
			{
				stringIterator.ReadUntil("\n", false);
			}
			else if (stringIterator.IsNext("}"))
			{
				stringIterator.Advance();
				break;
			}
		}
		while (stringIterator.Advance());

		return new(properties, brushes);
	}

	private static Brush ReadBrush(StringIterator stringIterator)
	{
		List<Face> faces = new();

		do
		{
			if (stringIterator.IsNext("("))
				faces.Add(ReadFace(stringIterator));
			else if (stringIterator.IsNext("}"))
				break;
		}
		while (stringIterator.Advance());

		return new(faces);
	}

	private static Face ReadFace(StringIterator stringIterator)
	{
		Vector3 p1 = ReadPoint(stringIterator);
		Vector3 p2 = ReadPoint(stringIterator);
		Vector3 p3 = ReadPoint(stringIterator);
		string textureName = stringIterator.ReadBetween(" ", " ", true);
		Plane textureAxisU = ReadPlane(stringIterator);
		Plane textureAxisV = ReadPlane(stringIterator);

		stringIterator.ReadBetween(" ", " ", false); // Skip texture rotation.
		string textureScaleXStr = stringIterator.ReadBetween(" ", " ", false);
		string textureScaleYStr = stringIterator.ReadBetween(" ", "\n", false);

		if (!float.TryParse(textureScaleXStr, out float textureScaleX))
			throw new MapParseException($"Invalid texture scale X: {textureScaleXStr}");
		if (!float.TryParse(textureScaleYStr, out float textureScaleY))
			throw new MapParseException($"Invalid texture scale Y: {textureScaleYStr}");

		return new(p1, p2, p3, textureName, textureAxisU, textureAxisV, new(textureScaleX, textureScaleY));
	}

	private static Vector3 ReadPoint(StringIterator stringIterator)
	{
		string stringData = stringIterator.ReadBetween("(", ")", true);

		string[] parts = stringData.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length != 3)
			throw new MapParseException($"Invalid face point: {stringData}");

		// Note: y and z are swapped in the map format.
		if (!float.TryParse(parts[0], out float x))
			throw new MapParseException($"Invalid face point: {stringData}");
		if (!float.TryParse(parts[1], out float z))
			throw new MapParseException($"Invalid face point: {stringData}");
		if (!float.TryParse(parts[2], out float y))
			throw new MapParseException($"Invalid face point: {stringData}");

		return new(x, y, z);
	}

	private static Plane ReadPlane(StringIterator stringIterator)
	{
		string stringData = stringIterator.ReadBetween("[", "]", true);

		string[] parts = stringData.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length != 4)
			throw new MapParseException($"Invalid face plane: {stringData}");

		// Note: y and z are swapped in the map format.
		if (!float.TryParse(parts[0], out float x))
			throw new MapParseException($"Invalid face plane: {stringData}");
		if (!float.TryParse(parts[1], out float z))
			throw new MapParseException($"Invalid face plane: {stringData}");
		if (!float.TryParse(parts[2], out float y))
			throw new MapParseException($"Invalid face plane: {stringData}");
		if (!float.TryParse(parts[3], out float d))
			throw new MapParseException($"Invalid face plane: {stringData}");

		return new(x, y, z, d);
	}
}
