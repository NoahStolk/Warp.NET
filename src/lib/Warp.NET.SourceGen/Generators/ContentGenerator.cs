using Microsoft.CodeAnalysis;
using Warp.NET.SourceGen.Extensions;
using Warp.NET.SourceGen.Utils;

namespace Warp.NET.SourceGen.Generators;

[Generator]
public class ContentGenerator : ISourceGenerator
{
	private const string _namespace = $"%{nameof(_namespace)}%";
	private const string _className = $"%{nameof(_className)}%";
	private const string _contentFieldInitializers = $"%{nameof(_contentFieldInitializers)}%";
	private const string _contentFields = $"%{nameof(_contentFields)}%";
	private const string _contentProperties = $"%{nameof(_contentProperties)}%";
	private const string _contentType = $"%{nameof(_contentType)}%";

	private const string _template = $$"""
		using System;
		using System.Collections.Generic;
		using System.Linq;

		namespace {{_namespace}};

		public sealed class {{_className}} : Warp.NET.IContentContainer<{{_contentType}}>
		{
			public static void Initialize(IReadOnlyDictionary<string, {{_contentType}}> content)
			{
				{{_contentFieldInitializers}}
			}

			{{_contentFields}}

			{{_contentProperties}}
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

		// These files must always be generated.
		string? contentRootDirectory = Path.GetDirectoryName(context.AdditionalFiles.FirstOrDefault(at => Path.GetFileName(at.Path) == "Content")?.Path);
		CreateFile(context, gameNamespace, "Charsets", $"{Constants.RootNamespace}.Content.Charset", GetFiles(contentRootDirectory, "Charsets", "*.txt"));
		CreateFile(context, gameNamespace, "Models", $"{Constants.RootNamespace}.Content.Model", GetFiles(contentRootDirectory, "Models", "*.obj"));
		CreateFile(context, gameNamespace, "Shaders", $"{Constants.RootNamespace}.Content.Shader", GetFiles(contentRootDirectory, "Shaders", "*.vert"));
		CreateFile(context, gameNamespace, "Sounds", $"{Constants.RootNamespace}.Content.Sound", GetFiles(contentRootDirectory, "Sounds", "*.wav"));
		CreateFile(context, gameNamespace, "Textures", $"{Constants.RootNamespace}.Content.Texture", GetFiles(contentRootDirectory, "Textures", "*.tga"));
	}

	private static List<string> GetFiles(string? contentRootDirectory, string contentFolderName, string contentFileSearchPattern)
	{
		if (contentRootDirectory == null)
			return new();

		string path = Path.Combine(contentRootDirectory, contentFolderName);
		return Directory.Exists(path) ? Directory.GetFiles(path, contentFileSearchPattern, SearchOption.AllDirectories).Where(FileNameUtils.PathIsValid).ToList() : new();
	}

	private static void CreateFile(GeneratorExecutionContext context, string gameNamespace, string className, string contentTypeName, List<string> filePaths)
	{
		string sourceBuilder = _template
			.Replace(_namespace, gameNamespace)
			.Replace(_className, className)
			.Replace(_contentType, contentTypeName)
			.Replace(_contentFieldInitializers, string.Join(Constants.NewLine, filePaths.ConvertAll(p => $"{GetFieldNameFromFileName(p)} = content.TryGetValue(\"{GetPropertyNameFromFileName(p)}\", out {contentTypeName}? {GetLocalNameFromFileName(p)}) ? {GetLocalNameFromFileName(p)} : null;")).IndentCode(2))
			.Replace(_contentFields, string.Join(Constants.NewLine, filePaths.ConvertAll(p => $"private static {contentTypeName}? {GetFieldNameFromFileName(p)};")).IndentCode(1))
			.Replace(_contentProperties, string.Join(Constants.NewLine, filePaths.ConvertAll(p => $"public static {contentTypeName} {GetPropertyNameFromFileName(p)} => {GetFieldNameFromFileName(p)} ?? throw new InvalidOperationException(\"Content does not exist or has not been initialized.\");")).IndentCode(1));

		context.AddSource(className, SourceBuilderUtils.Build(sourceBuilder));
	}

	private static string GetLocalNameFromFileName(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath);
		return name.FirstCharToLowerCase();
	}

	private static string GetFieldNameFromFileName(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath);
		return $"_{name.FirstCharToLowerCase()}";
	}

	private static string GetPropertyNameFromFileName(string filePath)
	{
		return Path.GetFileNameWithoutExtension(filePath);
	}
}
