using Warp.NET.Extensions;

namespace Warp.NET.Content.Conversion.Binaries;

public record MapBinary(List<MapBinary.Entity> Entities) : IBinary<MapBinary>
{
	public ContentType ContentType => ContentType.Map;

	public byte[] ToBytes()
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write((ushort)Entities.Count);

		foreach (Entity entity in Entities)
		{
			bw.Write((ushort)entity.Properties.Count);
			foreach (KeyValuePair<string, string> kvp in entity.Properties)
			{
				bw.Write(kvp.Key);
				bw.Write(kvp.Value);
			}

			bw.Write((ushort)entity.Brushes.Count);
			foreach (List<Face> faces in entity.Brushes.Select(b => b.Faces))
			{
				bw.Write((ushort)faces.Count);
				foreach (Face face in faces)
				{
					bw.Write(face.P1);
					bw.Write(face.P2);
					bw.Write(face.P3);
					bw.Write(face.TextureAxisU);
					bw.Write(face.TextureAxisV);
					bw.Write(face.TextureName);
					bw.Write(face.TextureScale);
				}
			}
		}

		return ms.ToArray();
	}

	public static MapBinary FromStream(BinaryReader br)
	{
		ushort entityCount = br.ReadUInt16();
		List<Entity> entities = new(entityCount);

		for (int i = 0; i < entityCount; i++)
		{
			ushort propertyCount = br.ReadUInt16();
			Dictionary<string, string> properties = new(propertyCount);

			for (int j = 0; j < propertyCount; j++)
			{
				string key = br.ReadString();
				string value = br.ReadString();
				properties.Add(key, value);
			}

			ushort brushCount = br.ReadUInt16();
			List<Brush> brushes = new(brushCount);

			for (int j = 0; j < brushCount; j++)
			{
				ushort faceCount = br.ReadUInt16();
				List<Face> faces = new(faceCount);

				for (int k = 0; k < faceCount; k++)
				{
					Vector3 p1 = br.ReadVector3();
					Vector3 p2 = br.ReadVector3();
					Vector3 p3 = br.ReadVector3();
					Plane textureAxisU = br.ReadPlane();
					Plane textureAxisV = br.ReadPlane();
					string textureName = br.ReadString();
					Vector2 textureScale = br.ReadVector2();
					faces.Add(new(p1, p2, p3, textureAxisU, textureAxisV, textureName, textureScale));
				}

				brushes.Add(new(faces));
			}

			entities.Add(new(properties, brushes));
		}

		return new(entities);
	}

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

	public class Brush
	{
		public Brush(List<Face> faces)
		{
			if (faces.Count < 4)
				throw new ArgumentException("There must be at least four faces defined in a brush.", nameof(faces));

			Faces = faces;
		}

		public List<Face> Faces { get; }
	}

	public class Face
	{
		public Face(Vector3 p1, Vector3 p2, Vector3 p3, Plane textureAxisU, Plane textureAxisV, string textureName, Vector2 textureScale)
		{
			P1 = p1;
			P2 = p2;
			P3 = p3;
			TextureAxisU = textureAxisU;
			TextureAxisV = textureAxisV;
			TextureName = textureName;
			TextureScale = textureScale;
		}

		public Vector3 P1 { get; }
		public Vector3 P2 { get; }
		public Vector3 P3 { get; }
		public Plane TextureAxisU { get; }
		public Plane TextureAxisV { get; }
		public string TextureName { get; }
		public Vector2 TextureScale { get; }
	}
}
