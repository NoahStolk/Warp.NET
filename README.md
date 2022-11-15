# Warp.NET

Warp.NET is a simplistic game engine for .NET 7.

## Key features

- Built in .NET 7 (C# 11) using strict nullable reference types
- Cross-platform
- Flexible game loop
- Supports 2D and 3D
- Makes use of the `System.Numerics` types
- Automatic content builder and loader
- Source generator which generates strongly-typed mappings for game assets
- UI library
- Relies on [dotnet/Silk.NET](https://github.com/dotnet/Silk.NET) for OpenGL, OpenAL, and GLFW bindings
- No other external dependencies

## Planned features

- Level editors

## Purpose

The purpose of the engine is to create a simple interface to OpenGL and OpenAL, while also providing the basics every game needs, such as creating a window, a reliable game loop, building and loading content, etc. Warp does not make a difference between 2D and 3D games (neither does OpenGL).

## Packages

A game project would only need to depend on the **Warp** and **Warp.SourceGenerators** NuGet packages.

## Content

Warp automatically builds content files from a local path. These types of content are currently supported:

| **Name**                | **Extension** | **Specification**                                               |
|-------------------------|---------------|-----------------------------------------------------------------|
| Vertex shader           | .vert         | GLSL code (text)                                                |
| Geometry shader         | .geom         | GLSL code (text)                                                |
| Fragment shader         | .frag         | GLSL code (text)                                                |
| TARGA texture           | .tga          | https://en.wikipedia.org/wiki/Truevision_TGA                    |
| Sound                   | .wav          | http://soundfile.sapp.org/doc/WaveFormat/                       |
| Mesh                    | .obj          | https://en.wikipedia.org/wiki/Wavefront_.obj_file               |
| Model                   | .obj          | https://en.wikipedia.org/wiki/Wavefront_.obj_file               |

| **Type** | **Type Name**           | .vert | .geom | .frag | .tga  | .wav  | .obj  |
|----------|-------------------------|-------|-------|-------|-------|-------|-------|
| `0x00`   | VertexShader            |   X   |       |       |       |       |       |
| `0x01`   | GeometryShader          |       |   X   |       |       |       |       |
| `0x02`   | FragmentShader          |       |       |   X   |       |       |       |
| `0x03`   | TextureW1               |       |       |       |   X   |       |       |
| `0x04`   | TextureRgb24            |       |       |       |   X   |       |       |
| `0x05`   | TextureRgba32           |       |       |       |   X   |       |       |
| `0x06`   | TextureW8               |       |       |       |   X   |       |       |
| `0x07`   | TextureWa16             |       |       |       |   X   |       |       |
| `0x08`   | Sound                   |       |       |       |       |   X   |       |
| `0x09`   | Mesh                    |       |       |       |       |       |   X   |
| `0x0D`   | Model                   |       |       |       |       |       |   X   |

All these files are converted to one large binary file. Warp only builds this content file when compiling in Debug mode. When distributing a game, copy the file to the Release output.

To gain access easily to the content files in code, Warp contains a source generator project, which generates classes and properties based on the content files. In order for the source generator to find the right folder, add a _C# analyzer additional file_ named "Content" to the root of the content folder:

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
