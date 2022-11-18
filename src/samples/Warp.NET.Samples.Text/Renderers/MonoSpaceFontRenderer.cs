using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Numerics;
using Warp.NET.Text;

namespace Warp.NET.Samples.Text.Renderers;

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

	public void Render(Vector2i<int> scale, Vector2 position, object? obj, bool center = false)
	{
		string? text = obj?.ToString();
		if (string.IsNullOrWhiteSpace(text))
			return;

		_font.Texture.Use();

		Gl.BindVertexArray(_vao);

		int charHeight = _font.Texture.Height;
		int halfCharHeight = (int)(charHeight / 2f);
		int scaledCharWidth = scale.X * charHeight;
		int scaledCharHeight = scale.Y * charHeight;
		Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scaledCharWidth, scaledCharHeight, 1);

		Vector2i<int> relativePosition = new(halfCharHeight, halfCharHeight);
		if (center)
		{
			Vector2i<int> textSize = _font.MeasureText(text) * scale;
			relativePosition -= textSize / 2;
		}

		foreach (char c in text)
		{
			Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(position.X + relativePosition.X, position.Y + relativePosition.Y, 0);
			Shader.SetMatrix4x4(FontUniforms.Model, scaleMatrix * translationMatrix);
			Shader.SetFloat(FontUniforms.Offset, _font.GetTextureOffset(c));
			Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);

			_font.AdvancePosition(c, ref relativePosition, halfCharHeight, scaledCharWidth, scaledCharHeight);
		}

		Gl.BindVertexArray(0);
	}
}
