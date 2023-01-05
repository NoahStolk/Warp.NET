using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Content;
using Texture = Warp.NET.Content.Texture;

namespace Warp.NET.Samples.Map.Rendering;

public class MeshObject
{
	private readonly uint _vao;
	private readonly Mesh _mesh;
	private readonly Texture _texture;

	public unsafe MeshObject(Mesh mesh, Texture texture)
	{
		_vao = CreateVao(mesh);

		static uint CreateVao(Mesh mesh)
		{
			uint vao = Gl.GenVertexArray();
			Gl.BindVertexArray(vao);

			uint vbo = Gl.GenBuffer();
			Gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

			fixed (Vertex* v = &mesh.Vertices[0])
				Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(mesh.Vertices.Length * sizeof(Vertex)), v, BufferUsageARB.StaticDraw);

			Gl.EnableVertexAttribArray(0);
			Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)0);

			Gl.EnableVertexAttribArray(1);
			Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(3 * sizeof(float)));

			// TODO: We don't do anything with normals here.
			Gl.EnableVertexAttribArray(2);
			Gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(5 * sizeof(float)));

			Gl.BindVertexArray(0);

			Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);

			return vao;
		}

		_mesh = mesh;
		_texture = texture;
	}

	public unsafe void Render()
	{
		_texture.Use();

		Shader.SetMatrix4x4(MeshUniforms.Model, Matrix4x4.Identity);

		Gl.BindVertexArray(_vao);
		fixed (uint* i = &_mesh.Indices[0])
			Gl.DrawElements(PrimitiveType.Triangles, (uint)_mesh.Indices.Length, DrawElementsType.UnsignedInt, i);
		Gl.BindVertexArray(0);
	}
}
