namespace Warp.NET.Content.Conversion.Maps;

public class Face
{
	public Face(Vector3 p1, Vector3 p2, Vector3 p3, string textureName, Plane textureAxisU, Plane textureAxisV, Vector2 textureScale)
	{
		P1 = p1;
		P2 = p2;
		P3 = p3;
		TextureName = textureName;
		TextureAxisU = textureAxisU;
		TextureAxisV = textureAxisV;
		TextureScale = textureScale;
	}

	public Vector3 P1 { get; }
	public Vector3 P2 { get; }
	public Vector3 P3 { get; }
	public string TextureName { get; }
	public Plane TextureAxisU { get; }
	public Plane TextureAxisV { get; }
	public Vector2 TextureScale { get; }
}
