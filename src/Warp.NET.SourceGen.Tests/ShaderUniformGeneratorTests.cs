using Warp.NET.SourceGen.Generators.Data;
using Warp.NET.SourceGen.Utils;

namespace Warp.NET.SourceGen.Tests;

[TestClass]
public class ShaderUniformGeneratorTests
{
	[DataTestMethod]
	[DataRow("model", "uniform mat4 model;")]
	[DataRow("view", "uniform mat4 view;")]
	[DataRow("projection", "uniform mat4 projection;")]
	[DataRow("offset", "uniform vec2 offset;")]
	[DataRow("test_test", "uniform float test_test;")]
	[DataRow("lightCount", "uniform int lightCount;")]
	[DataRow("lightPosition", "uniform vec3 lightPosition[maxLight];")]
	[DataRow("textureDiffuse", "uniform sampler2D textureDiffuse;")]
	[DataRow(null, "out vec4 FragColor;")]
	[DataRow(null, "in vec3 fragPosition;")]
	[DataRow(null, "const int maxLight = 128;")]
	public void TestGlslUniforms(string? expectedShaderUniformName, string line)
	{
		ShaderUniform? su = GlslUtils.GetFromGlslLine(line);
		if (expectedShaderUniformName == null)
		{
			Assert.IsNull(su);
		}
		else
		{
			Assert.IsNotNull(su);
			Assert.AreEqual(expectedShaderUniformName, su.Name);
		}
	}
}
