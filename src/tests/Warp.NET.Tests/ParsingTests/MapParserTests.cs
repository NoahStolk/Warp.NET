using Warp.NET.Content.Conversion.Maps;
using Warp.NET.Tests.Utils;

namespace Warp.NET.Tests.ParsingTests;

[TestClass]
public class MapParserTests
{
	[TestMethod]
	public void ParseBrushAndPointEntity()
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath("GenericValve_BrushAndPointEntity.map"));
		MapData mapData = MapParser.Parse(bytes);

		Assert.AreEqual(2, mapData.Entities.Count);

		Entity worldSpawn = mapData.Entities[0];
		Assert.AreEqual(2, worldSpawn.Properties.Count);

		ValidateProperty(worldSpawn, "classname", "worldspawn");
		ValidateProperty(worldSpawn, "mapversion", "220");

		Assert.AreEqual(1, worldSpawn.Brushes.Count);

		Brush brush0 = worldSpawn.Brushes[0];
		Assert.AreEqual(6, brush0.Faces.Count);

		ValidateFace(brush0.Faces[0], new(-64, -16, -64), new(-64, -16, -63), new(-64, -15, -64), "__TB_empty", new(+0, +0, -1, +0), new(+0, -1, +0, +0), new(+1, +1));
		ValidateFace(brush0.Faces[1], new(-64, -16, -64), new(-64, -15, -64), new(-63, -16, -64), "__TB_empty", new(+1, +0, +0, +0), new(+0, -1, +0, +0), new(+1, +1));
		ValidateFace(brush0.Faces[2], new(-64, -16, -64), new(-63, -16, -64), new(-64, -16, -63), "__TB_empty", new(-1, +0, +0, +0), new(+0, +0, -1, +0), new(+1, +1));
		ValidateFace(brush0.Faces[3], new(+64, +16, +64), new(+64, +16, +65), new(+65, +16, +64), "__TB_empty", new(+1, +0, +0, +0), new(+0, +0, -1, +0), new(+1, +1));
		ValidateFace(brush0.Faces[4], new(+64, +16, +64), new(+65, +16, +64), new(+64, +17, +64), "__TB_empty", new(-1, +0, +0, +0), new(+0, -1, +0, +0), new(+1, +1));
		ValidateFace(brush0.Faces[5], new(+64, +16, +64), new(+64, +17, +64), new(+64, +16, +65), "__TB_empty", new(+0, +0, +1, +0), new(+0, -1, +0, +0), new(+1, +1));

		Entity pointEntity = mapData.Entities[1];
		Assert.AreEqual(2, pointEntity.Properties.Count);
		ValidateProperty(pointEntity, "classname", "info_player_start");
		ValidateProperty(pointEntity, "origin", "0 0 40");
	}

	[TestMethod]
	public void ParseTwoBrushes()
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath("GenericValve_TwoBrushes.map"));
		MapData mapData = MapParser.Parse(bytes);

		Assert.AreEqual(1, mapData.Entities.Count);

		Entity worldSpawn = mapData.Entities[0];
		Assert.AreEqual(2, worldSpawn.Properties.Count);

		ValidateProperty(worldSpawn, "classname", "worldspawn");
		ValidateProperty(worldSpawn, "mapversion", "220");

		Assert.AreEqual(2, worldSpawn.Brushes.Count);

		Brush brush0 = worldSpawn.Brushes[0];
		Assert.AreEqual(6, brush0.Faces.Count);

		ValidateFace(brush0.Faces[0], new(-64, -16, -64), new(-64, -16, -63), new(-64, -15, -64), "__TB_empty", new(+0, +0, -1, +0), new(+0, -1, +0, +0), new(+1, +1));
		ValidateFace(brush0.Faces[1], new(-64, -16, -64), new(-64, -15, -64), new(-63, -16, -64), "__TB_empty", new(+1, +0, +0, +0), new(+0, -1, +0, +0), new(+1, +1));
		ValidateFace(brush0.Faces[2], new(-64, -16, -64), new(-63, -16, -64), new(-64, -16, -63), "__TB_empty", new(-1, +0, +0, +0), new(+0, +0, -1, +0), new(+1, +1));
		ValidateFace(brush0.Faces[3], new(+64, +16, +64), new(+64, +16, +65), new(+65, +16, +64), "__TB_empty", new(+1, +0, +0, +0), new(+0, +0, -1, +0), new(+1, +1));
		ValidateFace(brush0.Faces[4], new(+64, +16, +64), new(+65, +16, +64), new(+64, +17, +64), "__TB_empty", new(-1, +0, +0, +0), new(+0, -1, +0, +0), new(+1, +1));
		ValidateFace(brush0.Faces[5], new(+64, +16, +64), new(+64, +17, +64), new(+64, +16, +65), "__TB_empty", new(+0, +0, +1, +0), new(+0, -1, +0, +0), new(+1, +1));

		Brush brush1 = worldSpawn.Brushes[1];
		Assert.AreEqual(6, brush1.Faces.Count);

		ValidateFace(brush1.Faces[0], new(+64, +16, -64), new(+64, +16, -63), new(+64, +17, -64), "__TB_empty", new(+0, +0, -1, +0), new(+0, -1, +0, +0), new(+1, +1));
		ValidateFace(brush1.Faces[1], new(+64, +16, -64), new(+64, +17, -64), new(+65, +16, -64), "__TB_empty", new(+1, +0, +0, +0), new(+0, -1, +0, +0), new(+1, +1));
		ValidateFace(brush1.Faces[2], new(+64, +16, -64), new(+65, +16, -64), new(+64, +16, -63), "__TB_empty", new(-1, +0, +0, +0), new(+0, +0, -1, +0), new(+1, +1));
		ValidateFace(brush1.Faces[3], new(192, +48, +64), new(192, +48, +65), new(193, +48, +64), "__TB_empty", new(+1, +0, +0, +0), new(+0, +0, -1, +0), new(+1, +1));
		ValidateFace(brush1.Faces[4], new(192, +48, +64), new(193, +48, +64), new(192, +49, +64), "__TB_empty", new(-1, +0, +0, +0), new(+0, -1, +0, +0), new(+1, +1));
		ValidateFace(brush1.Faces[5], new(192, +48, +64), new(192, +49, +64), new(192, +48, +65), "__TB_empty", new(+0, +0, +1, +0), new(+0, -1, +0, +0), new(+1, +1));
	}

	[DataTestMethod]
	[DataRow("GenericQuake2_BrushAndPointEntity.map")]
	[DataRow("GenericStandard_BrushAndPointEntity.map")]
	public void ParseUnsupportedMapFormats(string fileName)
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath(fileName));
		Assert.ThrowsException<MapParseException>(() => MapParser.Parse(bytes));
	}

	private static void ValidateProperty(Entity entity, string propertyName, string propertyValue)
	{
		if (!entity.Properties.TryGetValue(propertyName, out string? classname))
			Assert.Fail("classname property not found");

		Assert.AreEqual(propertyValue, classname);
	}

	private static void ValidateFace(Face face, Vector3 p1, Vector3 p2, Vector3 p3, string textureName, Plane textureAxisU, Plane textureAxisV, Vector2 textureScale)
	{
		Assert.AreEqual(p1, face.P1);
		Assert.AreEqual(p2, face.P2);
		Assert.AreEqual(p3, face.P3);
		Assert.AreEqual(textureName, face.TextureName);
		Assert.AreEqual(textureAxisU, face.TextureAxisU);
		Assert.AreEqual(textureAxisV, face.TextureAxisV);
		Assert.AreEqual(textureScale, face.TextureScale);
	}
}
