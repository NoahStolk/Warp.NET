using Microsoft.CodeAnalysis;
using Warp.NET.SourceGen.Extensions;
using Warp.NET.SourceGen.Generators.Data;
using Warp.NET.SourceGen.Utils;

namespace Warp.NET.SourceGen.Generators;

[Generator]
public class ShaderUniformGenerator : ISourceGenerator
{
	private const string _namespacePlaceholder = "%namespace%";
	private const string _classNamePlaceholder = "%className%";
	private const string _uniformPropertiesPlaceholder = "%uniformProperties%";
	private const string _uniformInitializationPlaceholder = "%uniformInitialization%";

	private const string _template = $$"""
		namespace {{_namespacePlaceholder}};

		public static class {{_classNamePlaceholder}}
		{
			{{_uniformPropertiesPlaceholder}}

			public static void Initialize()
			{
				{{_uniformInitializationPlaceholder}}
			}
		}
		""";

	private const string _uniformCollectionInitializationPlaceholder = "%uniformCollectionInitialization%";

	private const string _initializationTemplate = $$"""
		using {{Constants.RootNamespace}};

		namespace {{_namespacePlaceholder}};

		public class ShaderUniformInitializer : IShaderUniformInitializer
		{
			public static void Initialize()
			{
				{{_uniformCollectionInitializationPlaceholder}}
			}
		}
		""";

	public void Initialize(GeneratorInitializationContext context)
	{
		// Method intentionally left empty.
	}

	public void Execute(GeneratorExecutionContext context)
	{
		string? gameNamespace = context.Compilation.AssemblyName;
		if (gameNamespace == null)
			return;

		string? contentRootDirectory = Path.GetDirectoryName(context.AdditionalFiles.FirstOrDefault(at => Path.GetFileName(at.Path) == "Content")?.Path);
		if (contentRootDirectory == null)
			return;

		string shadersDirectory = Path.Combine(contentRootDirectory, "Shaders");

		List<string> shaderNames = GetShaderNames(shadersDirectory);
		foreach (string shaderName in shaderNames)
		{
			List<ShaderUniform> vertUniforms = GetUniformsFromGlslFile(Path.Combine(shadersDirectory, $"{shaderName}.vert"));
			List<ShaderUniform> geomUniforms = GetUniformsFromGlslFile(Path.Combine(shadersDirectory, $"{shaderName}.geom"));
			List<ShaderUniform> fragUniforms = GetUniformsFromGlslFile(Path.Combine(shadersDirectory, $"{shaderName}.frag"));

			// Generate the class, even if there are no uniforms.
			CreateFile(context, gameNamespace, shaderName, vertUniforms.Concat(geomUniforms).Concat(fragUniforms).Distinct().ToList());
		}

		CreateInitializationFile(context, gameNamespace, shaderNames);
	}

	private static List<string> GetShaderNames(string shadersDirectory)
	{
		return !Directory.Exists(shadersDirectory) ? new() : Directory.GetFiles(shadersDirectory, "*.*", SearchOption.AllDirectories)
			.Where(FileNameUtils.PathIsValid)
			.Select(Path.GetFileNameWithoutExtension)
			.Distinct()
			.ToList();
	}

	private static List<ShaderUniform> GetUniformsFromGlslFile(string filePath)
	{
		if (!File.Exists(filePath))
			return new();

		// ! LINQ query filters out null values.
		return File.ReadAllLines(filePath).Select(GlslUtils.GetFromGlslLine).Where(su => su != null).ToList()!;
	}

	private static void CreateFile(GeneratorExecutionContext context, string gameNamespace, string shaderName, List<ShaderUniform> uniformNames)
	{
		string className = $"{shaderName}Uniforms";
		string sourceBuilder = _template
			.Replace(_namespacePlaceholder, gameNamespace)
			.Replace(_classNamePlaceholder, className)
			.Replace(_uniformPropertiesPlaceholder, string.Join(Constants.NewLine, uniformNames.ConvertAll(u => $"public static int {u.PropertyName} {{ get; private set; }}")).IndentCode(1))
			.Replace(_uniformInitializationPlaceholder, string.Join(Constants.NewLine, uniformNames.ConvertAll(u => $"{u.PropertyName} = {Constants.RootNamespace}.Content.Shader.GetUniformLocation(Shaders.{shaderName}.Id, \"{u.Name}\");")).IndentCode(2));

		context.AddSource(className, SourceBuilderUtils.Build(sourceBuilder));
	}

	private static void CreateInitializationFile(GeneratorExecutionContext context, string gameNamespace, List<string> shaderNames)
	{
		string sourceBuilder = _initializationTemplate
			.Replace(_namespacePlaceholder, gameNamespace)
			.Replace(_uniformCollectionInitializationPlaceholder, string.Join(Constants.NewLine, shaderNames.ConvertAll(u => $"{u}Uniforms.Initialize();")).IndentCode(2));

		context.AddSource("ShaderUniformInitializer", SourceBuilderUtils.Build(sourceBuilder));
	}
}
