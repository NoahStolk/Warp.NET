namespace Warp.NET.Content.Conversion.Maps;

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
