using Warp.NET.Extensions;

namespace Warp.NET.Content.Conversion.Maps;

internal record MapBinary(IReadOnlyList<Entity> Entities) : IBinary<MapBinary>
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
					bw.Write(face.TextureName);
					bw.Write(face.TextureAxisU);
					bw.Write(face.TextureAxisV);
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
					string textureName = br.ReadString();
					Plane textureAxisU = br.ReadPlane();
					Plane textureAxisV = br.ReadPlane();
					Vector2 textureScale = br.ReadVector2();
					faces.Add(new(p1, p2, p3, textureName, textureAxisU, textureAxisV, textureScale));
				}

				brushes.Add(new(faces));
			}

			entities.Add(new(properties, brushes));
		}

		return new(entities);
	}
}
