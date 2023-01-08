using Warp.NET.Extensions;

namespace Warp.NET.Content.Conversion.Maps;

public static class MapParser
{
	public static MapData Parse(byte[] fileContents)
	{
		List<Entity> entities = new();

		// Reading the file line by line seems OK according to the specifications.
		string text = Encoding.UTF8.GetString(fileContents);
		using StringReader stringReader = new(text);

		while (true)
		{
			string? line = stringReader.ReadLine();
			if (line == null)
				return new(entities);

			if (line.StartsWith("{"))
				entities.Add(ReadEntity(stringReader));
		}
	}

	private static Entity ReadEntity(TextReader textReader)
	{
		Dictionary<string, string> properties = new();
		List<Brush> brushes = new();

		while (true)
		{
			string? line = textReader.ReadLine();
			if (line == null)
				throw new MapParseException("Unexpected end of file while reading entity data.");

			if (line.StartsWith("{"))
			{
				brushes.Add(ReadBrush(textReader));
			}
			else if (line.StartsWith("\""))
			{
				string[] keyValuePairParts = line.Split("\"", StringSplitOptions.RemoveEmptyEntries);
				if (keyValuePairParts.Length != 3)
					throw new MapParseException($"Invalid key-value pair: {line}");

				properties.Add(keyValuePairParts[0], keyValuePairParts[2]);
			}
			else if (line.StartsWith("}"))
			{
				return new(properties, brushes);
			}
		}
	}

	private static Brush ReadBrush(TextReader textReader)
	{
		List<Face> faces = new();

		while (true)
		{
			int c = textReader.Peek();
			if (c == -1)
				throw new MapParseException("Unexpected end of file while reading brush data.");

			if (c == '(')
			{
				faces.Add(ReadFace(textReader));
			}
			else
			{
				textReader.Read();

				if (c == '}')
					return new(faces);
			}
		}
	}

	private static Face ReadFace(TextReader textReader)
	{
		Vector3 p1 = ReadPoint(textReader);
		Vector3 p2 = ReadPoint(textReader);
		Vector3 p3 = ReadPoint(textReader);
		string textureName = ReadTextureName(textReader);
		Plane textureAxisU = ReadPlane(textReader);
		Plane textureAxisV = ReadPlane(textReader);

		// Skip space.
		textReader.Read();

		// Skip texture rotation.
		textReader.ReadUntil(' ');

		Vector2 textureScale = ReadTextureScale(textReader);

		return new(p1, p2, p3, textureName, textureAxisU, textureAxisV, textureScale);

		static Vector3 ReadPoint(TextReader textReader)
		{
			string stringData;

			try
			{
				stringData = textReader.ReadBetween('(', ')');
			}
			catch (EndOfStreamException)
			{
				throw new MapParseException("Unexpected end of file while reading face plane.");
			}

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

		static string ReadTextureName(TextReader textReader)
		{
			StringBuilder builder = new();
			while (true)
			{
				int c = textReader.Peek(); // Use Peek() to not advance the stream. We stop reading the texture name when we see the starting [ character of the texture UV data.
				switch (c)
				{
					case -1: throw new MapParseException("Unexpected end of file while reading texture name.");
					case '[': return builder.ToString().Trim(); // Trim spaces around the texture name.
					default: builder.Append((char)textReader.Read()); break;
				}
			}
		}

		static Plane ReadPlane(TextReader textReader)
		{
			string stringData;

			try
			{
				stringData = textReader.ReadBetween('[', ']');
			}
			catch (EndOfStreamException)
			{
				throw new MapParseException("Unexpected end of file while reading face plane.");
			}

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

		static Vector2 ReadTextureScale(TextReader textReader)
		{
			string stringData = textReader.ReadUntilNewline();

			string[] parts = stringData.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length != 2)
				throw new MapParseException($"Invalid texture scale: {stringData}");

			if (!float.TryParse(parts[0], out float x))
				throw new MapParseException($"Invalid texture scale: {stringData}");
			if (!float.TryParse(parts[1], out float y))
				throw new MapParseException($"Invalid texture scale: {stringData}");

			return new(x, y);
		}
	}
}
