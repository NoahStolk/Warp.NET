# Warp.NET

[![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Warp.NET.svg)](https://www.nuget.org/packages/NoahStolk.Warp.NET/)
[![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Warp.NET.SourceGen.svg)](https://www.nuget.org/packages/NoahStolk.Warp.NET.SourceGen/)

Warp.NET is a simplistic game engine using .NET 7 and OpenGL, aimed at 2D and 3D games for Windows, Mac, and Linux. It is currently in development.

## Key features

- Built in .NET 7 (C# 11) using strict nullable reference types
- Supports Windows, Mac, and Linux
- Supports 2D and 3D
- Game loop using fixed time step with automatic frame interpolation to support high frame rates
- Makes use of the `System.Numerics` types
- Automatic content builder and loader
- Source generator which generates strongly-typed mappings for game assets
- UI library
- Relies on [dotnet/Silk.NET](https://github.com/dotnet/Silk.NET) for OpenGL, OpenAL, and GLFW bindings
- No other external dependencies

## Planned features

- Level editors

## Purpose

The purpose of the engine is to create a simple interface to OpenGL and OpenAL, while also providing the basics every game needs, such as creating a window, a reliable game loop, building and loading content, etc. Warp.NET does not make a difference between 2D and 3D games (neither does OpenGL).

## Content

Warp.NET automatically builds content files from a local path. These types of content are currently supported:

| **Name** | **Extension(s)**    | **Specification**                                 |
|----------|---------------------|---------------------------------------------------|
| Model    | .obj                | https://en.wikipedia.org/wiki/Wavefront_.obj_file |
| Shader   | .vert, .geom, .frag | GLSL code (text)                                  |
| Sound    | .wav                | http://soundfile.sapp.org/doc/WaveFormat/         |
| Texture  | .tga                | https://en.wikipedia.org/wiki/Truevision_TGA      |

All these files are converted to one large binary file. Warp.NET only builds this content file when compiling in Debug mode. When distributing a game, copy the file to the Release output.

To gain access easily to the content files in code, Warp.NET contains a source generator project, which generates classes and properties based on the content files. In order for the source generator to find the right folder, add a _C# analyzer additional file_ named "Content" to the root of the content folder:

```
<ItemGroup>
  <AdditionalFiles Include="Content\Content" />
</ItemGroup>
```

The directory should look something like this:

- 📁 Content
	- 📁 Meshes
		- 📃 Cube.obj
	- 📁 Textures
		- 📃 Stone.tga
	- 📃 Content
