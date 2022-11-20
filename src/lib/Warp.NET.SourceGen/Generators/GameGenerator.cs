using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using Warp.NET.SourceGen.Extensions;
using Warp.NET.SourceGen.Generators.Data;
using Warp.NET.SourceGen.Utils;

namespace Warp.NET.SourceGen.Generators;

[Generator]
public class GameGenerator : IIncrementalGenerator
{
	private const string _contentRootAdditionalFileName = "Content";

	private const string _namespace = $"%{nameof(_namespace)}%";
	private const string _singletonProperties = $"%{nameof(_singletonProperties)}%";
	private const string _gameObjectListFields = $"%{nameof(_gameObjectListFields)}%";
	private const string _gameObjectListProperties = $"%{nameof(_gameObjectListProperties)}%";
	private const string _gameObjectListAdds = $"%{nameof(_gameObjectListAdds)}%";
	private const string _gameObjectListRemoves = $"%{nameof(_gameObjectListRemoves)}%";

	private const string _gameTemplate = $$"""
		using System;
		using System.Collections.Generic;
		using System.Text;
		using Warp.NET;

		namespace {{_namespace}};

		public partial class Game : IGameBase<Game>
		{
			private static Game? _self;

			{{_gameObjectListFields}}

			public static Game Self
			{
				get => _self ?? throw new InvalidOperationException("Game is not initialized.");
				set
				{
					if (_self != null)
						throw new InvalidOperationException("Game is already initialized.");

					_self = value;
				}
			}

			{{_gameObjectListProperties}}

			{{_singletonProperties}}

			public static Game Construct(GameParameters gameParameters)
			{
				return new(gameParameters);
			}

			protected override void HandleAdds({{Constants.RootNamespace}}.GameObjects.IGameObject gameObject)
			{
				base.HandleAdds(gameObject);

				{{_gameObjectListAdds}}
			}

			protected override void HandleRemoves({{Constants.RootNamespace}}.GameObjects.IGameObject gameObject)
			{
				base.HandleRemoves(gameObject);

				{{_gameObjectListRemoves}}
			}
		}
		""";

	private static readonly TypeName _generateSingletonAttributeTypeName = new("GenerateSingletonAttribute");
	private static readonly TypeName _generateGameObjectListAttributeTypeName = new("GenerateGameObjectListAttribute");
	private static readonly TypeName _generateGameAttributeTypeName = new("GenerateGameAttribute");

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		// TODO: Generate unique attribute per assembly. (Internal?)
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(_generateSingletonAttributeTypeName.Type, SourceBuilderUtils.GenerateAttribute(AttributeTargets.Class, _generateSingletonAttributeTypeName.Type)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(_generateGameObjectListAttributeTypeName.Type, SourceBuilderUtils.GenerateAttribute(AttributeTargets.Class | AttributeTargets.Interface, _generateGameObjectListAttributeTypeName.Type)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(_generateGameAttributeTypeName.Type, SourceBuilderUtils.GenerateAttribute(AttributeTargets.Class, _generateGameAttributeTypeName.Type)));

		// ! LINQ query filters out null values.
		IncrementalValuesProvider<ClassDeclarationSyntax> singletonTypeDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (sn, _) => sn is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
				transform: static (ctx, _) => ctx.GetTypeWithAttribute<ClassDeclarationSyntax>(_generateSingletonAttributeTypeName.FullName))
			.Where(static m => m is not null)!;

		// ! LINQ query filters out null values.
		IncrementalValuesProvider<TypeDeclarationSyntax> gameObjectListTypeDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (sn, _) => sn is ClassDeclarationSyntax { AttributeLists.Count: > 0 } or InterfaceDeclarationSyntax { AttributeLists.Count: > 0 },
				transform: static (ctx, _) => ctx.GetTypeWithAttribute<TypeDeclarationSyntax>(_generateGameObjectListAttributeTypeName.FullName))
			.Where(static m => m is not null)!;

		// ! LINQ query filters out null values.
		IncrementalValuesProvider<ClassDeclarationSyntax> gameTypeDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (sn, _) => sn is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
				transform: static (ctx, _) => ctx.GetTypeWithAttribute<ClassDeclarationSyntax>(_generateGameAttributeTypeName.FullName))
			.Where(static m => m is not null)!;

		IncrementalValueProvider<
			(
				(
					(
						(
							Compilation Compilation,
							ImmutableArray<ClassDeclarationSyntax> Singletons
						) Left,
						ImmutableArray<TypeDeclarationSyntax> GameObjectLists
					) Left,
					ImmutableArray<ClassDeclarationSyntax> Games
				) Left,
				ImmutableArray<AdditionalText> AdditionalTexts
			)
		> compilation =
			context.CompilationProvider
				.Combine(singletonTypeDeclarations.Collect())
				.Combine(gameObjectListTypeDeclarations.Collect())
				.Combine(gameTypeDeclarations.Collect())
				.Combine(context.AdditionalTextsProvider.Where(at => Path.GetFileName(at.Path) == _contentRootAdditionalFileName).Collect());

		context.RegisterSourceOutput(
			compilation,
			static (spc, source) => Execute(source.Left.Left.Left.Compilation, source.Left.Left.Left.Singletons, source.Left.Left.GameObjectLists, source.Left.Games, spc));

		static void Execute(
			Compilation compilation,
			ImmutableArray<ClassDeclarationSyntax> singletonDeclarations,
			ImmutableArray<TypeDeclarationSyntax> gameObjectListDeclarations,
			ImmutableArray<ClassDeclarationSyntax> gameDeclarations,
			SourceProductionContext context)
		{
			if (gameDeclarations.Length != 1)
				return;

			string? gameNamespace = compilation.AssemblyName;
			if (gameNamespace == null)
				return;

			List<Singleton> singletons = new();
			if (!singletonDeclarations.IsDefaultOrEmpty)
				singletons = compilation.GetTypeDataFromName(singletonDeclarations.Distinct(), s => new Singleton(s), context.CancellationToken);

			List<GameObjectList> gameObjectLists = new();
			if (!gameObjectListDeclarations.IsDefaultOrEmpty)
				gameObjectLists = compilation.GetTypeDataFromName(gameObjectListDeclarations.Distinct(), s => new GameObjectList(s), context.CancellationToken);

			string sourceBuilder = _gameTemplate
				.Replace(_namespace, gameNamespace)
				.Replace(_singletonProperties, string.Join(Constants.NewLine, singletons.ConvertAll(s => $"public {s.FullTypeName} {s.PropertyName} {{ get; }} = new();")).IndentCode(1))
				.Replace(_gameObjectListFields, string.Join(Constants.NewLine, gameObjectLists.ConvertAll(s => $"private readonly List<{s.FullTypeName}> {s.FieldName} = new();")).IndentCode(1))
				.Replace(_gameObjectListProperties, string.Join(Constants.NewLine, gameObjectLists.ConvertAll(s => $"public IReadOnlyList<{s.FullTypeName}> {s.PropertyName} => {s.FieldName};")).IndentCode(1))
				.Replace(_gameObjectListAdds, string.Join(Constants.NewLine, gameObjectLists.ConvertAll(s => GenerateHandles(s, "Add"))).IndentCode(2))
				.Replace(_gameObjectListRemoves, string.Join(Constants.NewLine, gameObjectLists.ConvertAll(s => GenerateHandles(s, "Remove"))).IndentCode(2));

			context.AddSource("Game", SourceBuilderUtils.Build(sourceBuilder));
		}

		static string GenerateHandles(GameObjectList gameObjectList, string handleMethodName)
		{
			return $$"""
				if (gameObject is {{gameObjectList.FullTypeName}} {{gameObjectList.VariableName}})
					{{gameObjectList.FieldName}}.{{handleMethodName}}({{gameObjectList.VariableName}});
				""";
		}
	}
}
