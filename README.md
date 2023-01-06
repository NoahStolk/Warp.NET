# Warp.NET

Warp.NET is a simplistic game engine using .NET 7 and OpenGL, aimed at 2D and 3D games for Windows, Mac, and Linux. It is currently in development.

## Key features

- Built in .NET 7 (C# 11) using strict nullable reference types
- Supports Windows, Mac, and Linux
- Supports 2D and 3D
- Game loop using fixed time step with automatic frame interpolation to support high frame rates
- Automatic content builder and loader
- Source generator which generates strongly-typed mappings for game assets
- Makes use of the `System.Numerics` types
- No use of `System.Reflection`
- UI library
- Relies on [dotnet/Silk.NET](https://github.com/dotnet/Silk.NET) for OpenGL, OpenAL, and GLFW bindings
- No other external dependencies

## Planned features

- Level editors

## Purpose

The purpose of the engine is to create a simple interface to OpenGL and OpenAL, while also providing the basics every game needs, such as creating a window, a reliable game loop, building and loading content, etc. Warp.NET does not make a difference between 2D and 3D games (neither does OpenGL).

## NuGet packages

The libraries are distributed as NuGet packages. Note that the engine is in development and there will be breaking changes. The development is a highly iterative process and things will change rapidly until the v1.0.0 release.

| **Package**              | **Download**                                                                                                                                              | **Description**                                                  |
|--------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------|
| `Warp.NET`               | [![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Warp.NET.svg)](https://www.nuget.org/packages/NoahStolk.Warp.NET/)                             | The main library                                                 |
| `Warp.NET.SourceGen`     | [![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Warp.NET.SourceGen.svg)](https://www.nuget.org/packages/NoahStolk.Warp.NET.SourceGen/)         | Source generators for game projects                              |
| `Warp.NET.RenderImpl.Ui` | [![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Warp.NET.RenderImpl.Ui.svg)](https://www.nuget.org/packages/NoahStolk.Warp.NET.RenderImpl.Ui/) | An optional library containing an implementation of UI rendering |

## Content

Warp.NET automatically builds content files from a local path. These types of content are currently supported:

| **Name**           | **Extension(s)**    | **Specification**                                               |
|--------------------|---------------------|-----------------------------------------------------------------|
| Blob               | .bin                | Raw binary                                                      |
| Charset            | .txt                | Text file containing the charset on a single line               |
| Map (Valve format) | .map                | https://book.leveldesignbook.com/appendix/resources/formats/map |
| Model              | .obj                | https://en.wikipedia.org/wiki/Wavefront_.obj_file               |
| Shader             | .vert, .geom, .frag | GLSL code                                                       |
| Sound              | .wav                | http://soundfile.sapp.org/doc/WaveFormat/                       |
| Texture            | .tga                | https://en.wikipedia.org/wiki/Truevision_TGA                    |

All these files are converted to one large binary file using the `Bootstrapper` class. The `Bootstrapper` class will only build this content file when the original assets are present in the file system. It is common practice to always build the content file when running in Debug mode for a good development experience. When distributing a game, one can copy the file from the Debug output to the Release output.

To gain access easily to the content files in code, Warp.NET contains a source generator project, which, among other things, generates classes and properties based on the content files. In order for the source generator to find the right folder, add a _C# analyzer additional file_ named "Content" to the root of the content folder:

```
<ItemGroup>
  <AdditionalFiles Include="Content\Content" />
</ItemGroup>
```

The directory should look something like this:

- 📁 Content
	- 📁 Models
		- 📃 Cube.obj
	- 📁 Textures
		- 📃 Stone.tga
	- 📃 Content

### Common content initialization implementation

```cs
// Build the content file only in Debug mode, as the source files should only be present during development
#if DEBUG
const string? inputPathToContentRootDirectory = @"..\..\..\Content";
#else
const string? inputPathToContentRootDirectory = null;
#endif

// Build and decompile the content file
// If the content root directory does not exist or is null, the file will not be generated
// If the content file does not exist, an exception is thrown as the game will not be able to run without its assets
DecompiledContentFile decompiledContentFile = Bootstrapper.GetDecompiledContent(inputPathToContentRootDirectory, outputPathForGeneratedContentFile);

// Then assign the decompiled content to the properties in the classes
// These classes and their properties are fully generated based on the path to the content root directory (which is located using the "Content" additional file)
// If you add, rename, or remove an asset, the source generation will kick in and immediately update the properties
Charsets.Initialize(decompiledContentFile.Charsets);
Shaders.Initialize(decompiledContentFile.Shaders);
Textures.Initialize(decompiledContentFile.Textures);

// You can now refer to game assets using these properties
// In the example directory listed above, there is a texture named "Stone.tga" in the "Textures" subdirectory
// This means you can now refer to this asset using the Textures.Stone property

// Strongly-typed shader uniforms are also generated for every shader in the generated Shaders class
// For example, if you have a shader named "Sprite" containing a uniform named "spriteColor", you can refer to it using the SpriteUniforms.SpriteColor property which holds the uniform location value as a 32-bit integer
ShaderUniformInitializer.Initialize();
```

You can use multiple content directories and files if needed (for example, from libraries or other assemblies). Each assembly will have its own generated types to access its content.
