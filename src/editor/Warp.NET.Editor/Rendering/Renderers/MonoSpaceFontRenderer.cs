using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Editor.Rendering.BatchedData;
using Warp.NET.Numerics;
using Warp.NET.Text;

namespace Warp.NET.Editor.Rendering.Renderers;

public class MonoSpaceFontRenderer
{
	private readonly uint _vao;
	private readonly MonoSpaceFont _font;

	public unsafe MonoSpaceFontRenderer(MonoSpaceFont font)
	{
		_font = font;

		_vao = Gl.GenVertexArray();
		Gl.BindVertexArray(_vao);

		uint vbo = Gl.GenBuffer();
		Gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

		fixed (float* v = &_font.Vertices.ToArray()[0])
			Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)_font.Vertices.Length * sizeof(float), v, BufferUsageARB.StaticDraw);

		Gl.EnableVertexAttribArray(0);
		Gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), (void*)0);

		Gl.EnableVertexAttribArray(1);
		Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), (void*)(2 * sizeof(float)));

		Gl.BindVertexArray(0);
		Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
	}

	public void Render(List<MonoSpaceText> texts)
	{
		_font.Texture.Use();

		Gl.BindVertexArray(_vao);

		foreach (MonoSpaceText mst in texts)
		{
			ScissorActivator.SetScissor(mst.Scissor);

			int charHeight = _font.Texture.Height;
			int charWidth = _font.Texture.Width / _font.CharAmount;
			int scaledCharWidth = mst.Scale.X * charWidth;
			int scaledCharHeight = mst.Scale.Y * charHeight;
			Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scaledCharWidth, scaledCharHeight, 1);

			Vector2i<int> relativePosition = new(scaledCharWidth / 2, scaledCharHeight / 2);
			if (mst.TextAlign == TextAlign.Middle)
			{
				Vector2i<int> textSize = _font.MeasureText(mst.Text) * mst.Scale;
				relativePosition -= textSize / 2;
			}
			else if (mst.TextAlign == TextAlign.Right)
			{
				Vector2i<int> textSize = _font.MeasureText(mst.Text) * mst.Scale;
				relativePosition -= textSize with { Y = 0 };
			}

			int originX = relativePosition.X;

			Shader.SetVector4(FontUniforms.Color, mst.Color);
			foreach (char c in mst.Text)
			{
				Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(mst.Position.X + relativePosition.X, mst.Position.Y + relativePosition.Y, mst.Depth);
				Shader.SetMatrix4x4(FontUniforms.Model, scaleMatrix * translationMatrix);
				Shader.SetFloat(FontUniforms.Offset, _font.GetTextureOffset(c));
				Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);

				_font.AdvancePosition(c, ref relativePosition, originX, scaledCharWidth, scaledCharHeight);
			}
		}

		Gl.BindVertexArray(0);
	}
}
