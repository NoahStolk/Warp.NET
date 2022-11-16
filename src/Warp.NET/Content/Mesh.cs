namespace Warp.NET.Content;

public class Mesh
{
	public Mesh(Vertex[] vertices, uint[] indices, TriangleRenderMode triangleRenderMode)
	{
		Vertices = vertices;
		Indices = indices;
		TriangleRenderMode = triangleRenderMode;
	}

	public Vertex[] Vertices { get; }
	public uint[] Indices { get; }
	public TriangleRenderMode TriangleRenderMode { get; } // TODO: Remove.
}
