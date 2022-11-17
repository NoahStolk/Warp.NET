using Microsoft.CodeAnalysis;
using Warp.NET.SourceGen.Extensions;
using Warp.NET.SourceGen.Utils;

namespace Warp.NET.SourceGen.Generators;

[Generator]
public class ContentGenerator : ISourceGenerator
{
	private const string _namespacePlaceholder = "%namespace%";
	private const string _classNamePlaceholder = "%className%";
	private const string _contentPlaceholder = "%content%";
	private const string _contentTypePlaceholder = "%contentType%";

	private const string _template = $$"""
		using System;
		using System.Collections.Generic;
		using System.Linq;

		namespace {{_namespacePlaceholder}};

		public static class {{_classNamePlaceholder}}
		{
			{{_contentPlaceholder}}
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

		List<string> shaderPaths = GetFiles(contentRootDirectory, "Shaders", "*.vert");
		List<string> tgaTexturePaths = GetFiles(contentRootDirectory, "Textures", "*.tga");
		List<string> meshPaths = GetFiles(contentRootDirectory, "Meshes", "*.obj");
		List<string> modelPaths = GetFiles(contentRootDirectory, "Models", "*.obj");
		List<string> soundPaths = GetFiles(contentRootDirectory, "Sounds", "*.wav");

		CreateFile(context, gameNamespace, "Shaders", $"{Constants.RootNamespace}.Content.Shader", shaderPaths);
		CreateFile(context, gameNamespace, "Textures", $"{Constants.RootNamespace}.Content.Texture", tgaTexturePaths);
		CreateFile(context, gameNamespace, "Meshes", $"{Constants.RootNamespace}.Content.Mesh", meshPaths);
		CreateFile(context, gameNamespace, "Models", $"{Constants.RootNamespace}.Content.Model", modelPaths);
		CreateFile(context, gameNamespace, "Sounds", $"{Constants.RootNamespace}.Content.Sound", soundPaths);
	}

	private static List<string> GetFiles(string basePath, string folderName, string searchPattern)
	{
		string path = Path.Combine(basePath, folderName);
		return Directory.Exists(path) ? Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories).Where(FileNameUtils.PathIsValid).ToList() : new();
	}

	private static void CreateFile(GeneratorExecutionContext context, string gameNamespace, string className, string contentTypeName, List<string> filePaths)
	{
		string sourceBuilder = _template
			.Replace(_namespacePlaceholder, gameNamespace)
			.Replace(_classNamePlaceholder, className)
			.Replace(_contentTypePlaceholder, contentTypeName)
			.Replace(_contentPlaceholder, string.Join(Constants.NewLine, filePaths.ConvertAll(p => $"public static {contentTypeName} {Path.GetFileNameWithoutExtension(p)} {{ get; set; }} = null!;")).IndentCode(1));

		context.AddSource(className, SourceBuilderUtils.Build(sourceBuilder));
	}
}
