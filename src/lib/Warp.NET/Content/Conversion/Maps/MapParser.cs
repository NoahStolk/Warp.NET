namespace Warp.NET.Content.Conversion.Maps;

public static class MapParser
{
	public static MapData Parse(byte[] fileContents)
	{
		List<Entity> entities = new();
		Entity? currentEntity = null;
		List<Face>? currentFaces = null;

		string text = Encoding.UTF8.GetString(fileContents);
		string[] lines = text.Split('\n');
		foreach (string line in lines)
		{
			if (currentEntity == null)
			{
				if (line.StartsWith('{'))
					currentEntity = new();
			}
			else if (currentFaces == null)
			{
				if (line.StartsWith('"'))
				{
					string[] properties = line.Replace("\"", string.Empty).Split(' ');
					currentEntity.Properties.Add(properties[0], properties[1]);
				}
				else if (line.StartsWith('{'))
				{
					currentFaces = new();
				}
				else if (line.StartsWith('}'))
				{
					entities.Add(currentEntity);
					currentEntity = null; // ?
				}
			}
			else
			{
				if (line.StartsWith('}'))
				{
					currentEntity.Brushes.Add(new(currentFaces));
					currentFaces = null; // ?
				}
				else
				{
					// TODO: Find a better way to parse this.
					string[] points = line.Replace("( ", string.Empty).Split(')', StringSplitOptions.RemoveEmptyEntries)[..3];

					string[] p1 = points[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
					string[] p2 = points[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
					string[] p3 = points[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);

					string[] afterPoints = line[(line.LastIndexOf(')') + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
					string textureName = afterPoints[0].Trim();

					string afterTextureName = line[(line.LastIndexOf(textureName, StringComparison.Ordinal) + textureName.Length)..];

					int separator = afterTextureName.LastIndexOf(']');
					string[] textureAxes = afterTextureName[..separator].Split("] [");

					string[] textureAxisU = textureAxes[0].Replace("[", string.Empty).Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
					string[] textureAxisV = textureAxes[1].Replace("]", string.Empty).Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

					string[] textureUv = afterTextureName[(separator + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries);

					currentFaces.Add(new(
						p1: new(float.Parse(p1[0]), float.Parse(p1[2]), float.Parse(p1[1])),
						p2: new(float.Parse(p2[0]), float.Parse(p2[2]), float.Parse(p2[1])),
						p3: new(float.Parse(p3[0]), float.Parse(p3[2]), float.Parse(p3[1])),
						textureAxisU: new(float.Parse(textureAxisU[0]), float.Parse(textureAxisU[2]), float.Parse(textureAxisU[1]), float.Parse(textureAxisU[3])),
						textureAxisV: new(float.Parse(textureAxisV[0]), float.Parse(textureAxisV[2]), float.Parse(textureAxisV[1]), float.Parse(textureAxisV[3])),
						textureName: textureName,
						textureScale: new(float.Parse(textureUv[1]), float.Parse(textureUv[2]))));
				}
			}
		}

		return new(entities);
	}
}
