using System.Collections.Generic;
using Warp.NET.Content.Conversion.GameDefinitions;
using Warp.NET.Content.Conversion.GameDefinitions.Properties;
using Warp.NET.Tests.Utils;

namespace Warp.NET.Tests.ParsingTests;

[TestClass]
public class GameDefinitionTests
{
	[TestMethod]
	public void ParseGameDefinitions()
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath("EntityDefinitions.fgd"));
		GameDefinitionData gameDefinitionData = GameDefinitionParser.Parse(bytes);

		Assert.AreEqual(4, gameDefinitionData.GameClasses.Count);

		IReadOnlyList<GameClass> solidClasses = gameDefinitionData.GameClasses.Where(gc => gc.Type == ClassType.Solid).ToList();
		IReadOnlyList<GameClass> pointClasses = gameDefinitionData.GameClasses.Where(gc => gc.Type == ClassType.Point).ToList();
		IReadOnlyList<GameClass> baseClasses = gameDefinitionData.GameClasses.Where(gc => gc.Type == ClassType.Base).ToList();

		Assert.AreEqual(1, solidClasses.Count);
		Assert.AreEqual(2, pointClasses.Count);
		Assert.AreEqual(1, baseClasses.Count);

		GameClass worldSpawn = solidClasses[0];
		Assert.AreEqual("worldspawn", worldSpawn.Name);
		Assert.AreEqual(0, worldSpawn.BaseClassNames.Count);
		Assert.AreEqual(0, worldSpawn.ChoicesProperties.Count);
		Assert.AreEqual(0, worldSpawn.FlagsProperties.Count);
		Assert.AreEqual(0, worldSpawn.IntegerProperties.Count);
		Assert.AreEqual(0, worldSpawn.StringProperties.Count);

		GameClass playerClass = baseClasses[0];
		Assert.AreEqual("PlayerClass", playerClass.Name);
		Assert.AreEqual(0, playerClass.BaseClassNames.Count);
		Assert.AreEqual(0, playerClass.ChoicesProperties.Count);
		Assert.AreEqual(0, playerClass.FlagsProperties.Count);
		Assert.AreEqual(0, playerClass.IntegerProperties.Count);
		Assert.AreEqual(0, playerClass.StringProperties.Count);

		GameClass playerStart = pointClasses[0];
		Assert.AreEqual("player_start", playerStart.Name);
		Assert.AreEqual(1, playerStart.BaseClassNames.Count);

		Assert.AreEqual("PlayerClass", playerStart.BaseClassNames[0]);

		Assert.AreEqual(0, playerStart.ChoicesProperties.Count);
		Assert.AreEqual(0, playerStart.FlagsProperties.Count);
		Assert.AreEqual(0, playerStart.IntegerProperties.Count);
		Assert.AreEqual(0, playerStart.StringProperties.Count);

		GameClass light = pointClasses[1];
		Assert.AreEqual("light", light.Name);
		Assert.AreEqual(0, light.BaseClassNames.Count);
		Assert.AreEqual(0, light.ChoicesProperties.Count);
		Assert.AreEqual(0, light.FlagsProperties.Count);
		Assert.AreEqual(0, light.IntegerProperties.Count);
		Assert.AreEqual(1, light.StringProperties.Count);

		StringProperty lightColor = light.StringProperties[0];
		Assert.AreEqual("color", lightColor.Name);
		Assert.AreEqual("255 127 0", lightColor.DefaultValue);
	}
}
