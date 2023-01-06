namespace Warp.NET.Content.Conversion.Maps.GeometryCalculator;

public static class MapGeometryCalculator
{
	public static List<(Mesh Mesh, Texture Texture)> ToMap(Map map, IReadOnlyDictionary<string, Texture> textures)
	{
		Dictionary<Brush, List<Polygon>> geometry = new();

		foreach (Brush brush in map.Entities.SelectMany(e => e.Brushes))
			geometry.Add(brush, ToPolygons(brush, textures));

		SortVerticesCw(geometry);
		CalculateTextureCoordinates(geometry);
		Inverse(geometry);

		List<(Mesh Mesh, Texture Texture)> meshes = new();
		foreach (KeyValuePair<Brush, List<Polygon>> brush in geometry)
		{
			foreach (Polygon polygon in brush.Value)
			{
				List<Vertex> vertices = new();
				List<uint> indices = new();

				for (int i = 0; i < polygon.Vertices.Count; i++)
				{
					Vector3 position = polygon.Vertices[i];
					Vector2 texCoord = polygon.TextureScales.Count > i ? polygon.TextureScales[i] : Vector2.Zero;
					vertices.Add(new(position, texCoord, Vector3.One));
				}

				for (int i = 0; i < polygon.Vertices.Count - 2; i++)
				{
					indices.Add(0);
					indices.Add((uint)(i + 2));
					indices.Add((uint)(i + 1));
				}

				Mesh mesh = new(vertices.ToArray(), indices.ToArray());

				meshes.Add((mesh, polygon.Texture));
			}
		}

		return meshes;
	}

	private static void Inverse(Dictionary<Brush, List<Polygon>> brushes)
	{
		foreach (KeyValuePair<Brush, List<Polygon>> brush in brushes)
		{
			foreach (Polygon polygon in brush.Value)
			{
				for (int i = 0; i < polygon.Vertices.Count; i++)
				{
					polygon.Vertices[i] = new(polygon.Vertices[i].X * -1, polygon.Vertices[i].Y, polygon.Vertices[i].Z);
				}
			}
		}
	}

	private static void CalculateTextureCoordinates(Dictionary<Brush, List<Polygon>> brushes)
	{
		foreach (KeyValuePair<Brush, List<Polygon>> brush in brushes)
		{
			foreach (Polygon polygon in brush.Value)
			{
				GeometryMath.CalculateTextureCoordinates(polygon);
			}
		}
	}

	private static void SortVerticesCw(Dictionary<Brush, List<Polygon>> brushes)
	{
		foreach (KeyValuePair<Brush, List<Polygon>> brush in brushes)
		{
			foreach (Polygon polygon in brush.Value)
			{
				GeometryMath.SortVerticesCw(polygon);
			}
		}
	}

	private static List<Polygon> ToPolygons(Brush brush, IReadOnlyDictionary<string, Texture> textures)
	{
		Face[] faces = brush.Faces.ToArray();

		List<Plane> planes = new();

		for (int i = 0; i < faces.Length; i++)
		{
			planes.Add(Plane.CreateFromVertices(faces[i].P1, faces[i].P2, faces[i].P3));
		}

		Plane lfi = default;
		Plane lfj = default;
		Plane lfk = default;

		List<Polygon> polygons = new();

		for (int i = 0; i < faces.Length; i++)
		{
			polygons.Add(new(planes[i], faces[i], textures));

			if (i == faces.Length - 3)
			{
				if (i + 1 < faces.Length)
				{
					lfi = planes[i + 1];
				}
			}
			else if (i == faces.Length - 2)
			{
				if (i + 1 < faces.Length)
				{
					lfj = planes[i + 1];
				}
			}
			else if (i == faces.Length - 1)
			{
				if (i + 1 < faces.Length)
				{
					lfk = planes[i + 1];
				}
			}
		}

		for (int fi = 0; planes[fi] != lfi; fi++)
		{
			for (int fj = fi + 1; planes[fj] != lfj; fj++)
			{
				for (int fk = fj + 1; planes[fk] != lfk; fk++)
				{
					if (GeometryMath.GetIntersection(planes[fj], planes[fk], planes[fi], out Vector3 p))
					{
						bool illegal = false;

						for (int i = 0; i < faces.Length; i++)
						{
							if (GeometryMath.ClassifyPoint(p, planes[i]) == GeometryMath.ECp.Front)
							{
								illegal = true;
								break;
							}
						}

						if (!illegal)
						{
							polygons[fi].Vertices.Add(p);
							polygons[fj].Vertices.Add(p);
							polygons[fk].Vertices.Add(p);
						}
					}

					if (fk + 1 >= faces.Length)
					{
						break;
					}
				}
			}
		}

		return polygons;
	}
}
