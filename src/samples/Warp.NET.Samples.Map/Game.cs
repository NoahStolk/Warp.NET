using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Content;
using Warp.NET.Samples.Map.MapBuilder;
using Warp.NET.Samples.Map.Rendering;

namespace Warp.NET.Samples.Map;

[GenerateGame]
public sealed partial class Game : GameBase
{
	private readonly Camera _camera = new();
	private readonly List<MeshObject> _meshObjects = new();

	private Game()
	{
		Gl.Enable(EnableCap.DepthTest);
		Gl.Enable(EnableCap.Blend);

		List<Brush> brushes = MapBuilder.Map.Load("""C:\Users\NOAH\source\repos\Warp.NET\src\samples\Warp.NET.Samples.Map\Content\Maps\Test2.map""");
		foreach (Brush brush in brushes)
		{
			foreach (Polygon polygon in brush.Polygons)
			{
				List<Vertex> vertices = new();
				List<uint> indices = new();

				for (int i = 0; i < polygon.Vertices.Count; i++)
				{
					Vector3 position = polygon.Vertices[i];
					Vector2 texCoord = polygon.TextureScales.Count > i ? polygon.TextureScales[i] : Vector2.Zero;
					vertices.Add(new(position, texCoord, Vector3.One));
				}

				for (int i = 0; i < polygon.Vertices.Count - 2; i++)
				{
					indices.Add(0);
					indices.Add((uint)(i + 1));
					indices.Add((uint)(i + 2));
				}

				Mesh mesh = new(vertices.ToArray(), indices.ToArray());

				_meshObjects.Add(new(mesh, polygon.Texture));
			}
		}
	}

	protected override void Update()
	{
		base.Update();

		_camera.Update();
	}

	protected override void PrepareRender()
	{
		base.PrepareRender();

		_camera.PreRender();
	}

	protected override void Render()
	{
		Gl.ClearColor(0, 0, 0, 1);
		Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		Shaders.Mesh.Use();
		Shader.SetMatrix4x4(MeshUniforms.View, _camera.ViewMatrix);
		Shader.SetMatrix4x4(MeshUniforms.Projection, _camera.Projection);
		Shader.SetInt(MeshUniforms.TextureDiffuse, 0);

		foreach (MeshObject meshObject in _meshObjects)
			meshObject.Render();
	}
}
