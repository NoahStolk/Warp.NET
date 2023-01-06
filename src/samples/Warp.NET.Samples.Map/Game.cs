using Silk.NET.OpenGL;
using Warp.NET.Content;
using Warp.NET.Content.Conversion.Maps.GeometryCalculator;
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
		Gl.Enable(EnableCap.CullFace);

		List<(Mesh Mesh, Texture Texture)> meshes = MapGeometryCalculator.ToMap(Maps.Test3, TextureDictionary.Textures);
		foreach ((Mesh mesh, Texture texture) in meshes)
		{
			_meshObjects.Add(new(mesh, texture));
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
