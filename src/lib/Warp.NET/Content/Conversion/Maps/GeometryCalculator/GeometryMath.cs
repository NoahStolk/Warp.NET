namespace Warp.NET.Content.Conversion.Maps.GeometryCalculator;

public static class GeometryMath
{
	private const double _epsilon = 1e-3;

	public enum ECp
	{
		Front = 0,
		Back = 1,
		OnPlane = 2,
	}

	private static float DistanceToPlane(Vector3 vector, Plane plane)
	{
		return Vector3.Dot(plane.Normal, vector) + plane.D;
	}

	public static ECp ClassifyPoint(Vector3 vector, Plane plane)
	{
		float distance = DistanceToPlane(vector, plane);

		if (distance > _epsilon)
			return ECp.Front;

		return distance < -_epsilon ? ECp.Back : ECp.OnPlane;
	}

	public static bool GetIntersection(Plane p1, Plane p2, Plane p3, out Vector3 v)
	{
		v = new(0, 0, 0);

		float denominator = Vector3.Dot(p3.Normal, Vector3.Cross(p1.Normal, p2.Normal));

		if (Math.Abs(denominator) < _epsilon)
			return false;

		v = (Vector3.Cross(p1.Normal, p2.Normal) * -p3.D
			- Vector3.Cross(p2.Normal, p3.Normal) * p1.D
			- Vector3.Cross(p3.Normal, p1.Normal) * p2.D) / denominator;

		return true;
	}

	public static void SortVerticesCw(Polygon poly)
	{
		Vector3 center = Vector3.Zero;
		foreach (Vector3 vector in poly.Vertices)
			center += vector;

		center /= poly.Vertices.Count;

		for (int i = 0; i < poly.Vertices.Count - 2; i++)
		{
			double smallestAngle = -1;
			int smallest = -1;

			Vector3 a = Vector3.Normalize(poly.Vertices[i] - center);

			Plane p = Plane.CreateFromVertices(poly.Vertices[i], center, center + poly.Plane.Normal);

			for (int j = i + 1; j < poly.Vertices.Count; j++)
			{
				if (ClassifyPoint(poly.Vertices[j], p) != ECp.Back)
				{
					Vector3 b = Vector3.Normalize(poly.Vertices[j] - center);

					double angle = Vector3.Dot(a, b);

					if (angle > smallestAngle)
					{
						smallestAngle = angle;
						smallest = j;
					}
				}
			}

			if (smallest == -1)
			{
				return;
			}

			(poly.Vertices[smallest], poly.Vertices[i + 1]) = (poly.Vertices[i + 1], poly.Vertices[smallest]);
		}

		Plane beforePlane = poly.Plane;

		CalculatePlane(poly, out Plane afterPlane);

		if (Vector3.Dot(afterPlane.Normal, beforePlane.Normal) < 0)
		{
			int j = poly.Vertices.Count;

			for (int i = 0; i < j / 2; i++)
			{
				(poly.Vertices[i], poly.Vertices[j - i - 1]) = (poly.Vertices[j - i - 1], poly.Vertices[i]);
			}
		}
	}

	public static void CalculateTextureCoordinates(Polygon poly)
	{
		for (int i = 0; i < poly.Vertices.Count; i++)
		{
			float u = Vector3.Dot(poly.Face.TextureAxisU.Normal, poly.Vertices[i]);
			u = u / poly.Texture.Width / poly.Face.TextureScale[0];
			u = u + poly.Face.TextureAxisU.D / poly.Texture.Width;

			float v = Vector3.Dot(poly.Face.TextureAxisV.Normal, poly.Vertices[i]);
			v = v / poly.Texture.Height / poly.Face.TextureScale[1];
			v = v + poly.Face.TextureAxisV.D / poly.Texture.Height;

			poly.TextureScales.Add(new(u, v));
		}

		bool bDoU = true;
		bool bDoV = true;
		for (int i = 0; i < poly.Vertices.Count; i++)
		{
			if (poly.TextureScales[i].X < 1 && poly.TextureScales[i].Y > -1)
				bDoU = false;

			if (poly.TextureScales[i].X < 1 && poly.TextureScales[i].Y > -1)
				bDoV = false;
		}

		if (bDoU || bDoV)
		{
			double nearestU = 0;
			double u = poly.TextureScales[0].X;

			double nearestV = 0;
			double v = poly.TextureScales[0].Y;

			if (bDoU)
			{
				nearestU = u > 1 ? Math.Floor(u) : Math.Ceiling(u);
			}

			if (bDoV)
			{
				nearestV = v > 1 ? Math.Floor(v) : Math.Ceiling(v);
			}

			for (int i = 0; i < poly.Vertices.Count; i++)
			{
				if (bDoU)
				{
					u = poly.TextureScales[i].X;

					if (Math.Abs(u) < Math.Abs(nearestU))
					{
						nearestU = u > 1 ? Math.Floor(u) : Math.Ceiling(u);
					}
				}

				if (bDoV)
				{
					v = poly.TextureScales[i].Y;

					if (Math.Abs(v) < Math.Abs(nearestV))
					{
						nearestV = v > 1 ? Math.Floor(v) : Math.Ceiling(v);
					}
				}
			}

			for (int i = 0; i < poly.Vertices.Count; i++)
			{
				poly.TextureScales[i] = new(poly.TextureScales[i].X - (float)nearestU, poly.TextureScales[i].Y - (float)nearestV);
			}
		}
	}

	private static void CalculatePlane(Polygon poly, out Plane plane)
	{
		plane = poly.Plane;

		if (poly.Vertices.Count < 3)
			return;

		plane.Normal.X = 0f;
		plane.Normal.Y = 0f;
		plane.Normal.Z = 0f;

		Vector3 centerOfMass = Vector3.Zero;
		for (int i = 0; i < poly.Vertices.Count; i++)
		{
			int j = i + 1;

			if (j >= poly.Vertices.Count)
				j = 0;

			plane.Normal.X += (poly.Vertices[i].Y - poly.Vertices[j].Y) * (poly.Vertices[i].Z + poly.Vertices[j].Z);
			plane.Normal.Y += (poly.Vertices[i].Z - poly.Vertices[j].Z) * (poly.Vertices[i].X + poly.Vertices[j].X);
			plane.Normal.Z += (poly.Vertices[i].X - poly.Vertices[j].X) * (poly.Vertices[i].Y + poly.Vertices[j].Y);

			centerOfMass.X += poly.Vertices[i].X;
			centerOfMass.Y += poly.Vertices[i].Y;
			centerOfMass.Z += poly.Vertices[i].Z;
		}

		if (Math.Abs(plane.Normal.X) < _epsilon &&
		    Math.Abs(plane.Normal.Y) < _epsilon &&
		    Math.Abs(plane.Normal.Z) < _epsilon)
		{
			return;
		}

		double magnitude = Math.Sqrt(plane.Normal.X * plane.Normal.X + plane.Normal.Y * plane.Normal.Y + plane.Normal.Z * plane.Normal.Z);

		if (magnitude < _epsilon)
			return;

		plane.Normal.X /= (float)magnitude;
		plane.Normal.Y /= (float)magnitude;
		plane.Normal.Z /= (float)magnitude;

		centerOfMass.X /= poly.Vertices.Count;
		centerOfMass.Y /= poly.Vertices.Count;
		centerOfMass.Z /= poly.Vertices.Count;

		plane.D = -Vector3.Dot(centerOfMass, plane.Normal);
	}
}
