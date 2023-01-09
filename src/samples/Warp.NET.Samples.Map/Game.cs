using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Content;
using Warp.NET.Content.GeometryCalculator;

namespace Warp.NET.Samples.Map;

[GenerateGame]
public sealed partial class Game : GameBase
{
	private readonly Camera _camera = new();
	private readonly List<WorldMesh> _worldMeshes = new();

	private Game()
	{
		Gl.Enable(EnableCap.DepthTest);
		Gl.Enable(EnableCap.Blend);
		Gl.Enable(EnableCap.CullFace);
	}

	public void Initialize()
	{
		_camera.Add();

		LoadLevel(Maps.Test3);
	}

	private void LoadLevel(Content.Map map)
	{
		Texture fallbackTexture = new(2, 2, new byte[] { 255, 0, 0, 255, 0, 0, 255, 255, 255, 0, 0, 255, 0, 0, 255, 255 });
		List<(Mesh Mesh, Texture Texture)> meshes = MapGeometry.BuildGeometry(map, Textures.InternalContentDictionary, fallbackTexture, Vector3.One);
		foreach ((Mesh mesh, Texture texture) in meshes)
			_worldMeshes.Add(new(mesh, texture));
	}

	protected override void Render()
	{
		Gl.ClearColor(0, 0, 0, 1);
		Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		Shaders.Mesh.Use();
		Shader.SetMatrix4x4(MeshUniforms.View, _camera.ViewMatrix);
		Shader.SetMatrix4x4(MeshUniforms.Projection, _camera.Projection);
		Shader.SetInt(MeshUniforms.TextureDiffuse, 0);

		foreach (WorldMesh meshObject in _worldMeshes)
			meshObject.Render();
	}
}
