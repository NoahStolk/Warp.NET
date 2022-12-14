using Silk.NET.OpenGL;
using System.Numerics;
using Warp.NET.Numerics;
using Warp.NET.Text;

namespace Warp.NET.Samples.Text.Renderers;

public class MonoSpaceFontRenderer
{
	private readonly uint _vao;
	private readonly MonoSpaceFont _font;

	private readonly List<MonoSpaceText> _collection = new();

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

	public void Schedule(Vector2i<int> scale, Vector2i<int> position, string text, TextAlign textAlign)
	{
		_collection.Add(new(scale, position, text, textAlign));
	}

	public void Render()
	{
		_font.Texture.Use();

		Gl.BindVertexArray(_vao);

		foreach (MonoSpaceText mst in _collection)
		{
			int charWidth = _font.Texture.Width / _font.CharAmount;
			int charHeight = _font.Texture.Height;
			int scaledCharWidth = mst.Scale.X * charWidth;
			int scaledCharHeight = mst.Scale.Y * charHeight;
			Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scaledCharWidth, scaledCharHeight, 1);

			Vector2i<int> textSize = _font.MeasureText(mst.Text) * mst.Scale;
			Vector2i<int> relativePosition = mst.TextAlign switch
			{
				TextAlign.Middle => new Vector2i<int>(scaledCharWidth / 2, scaledCharHeight / 2) - textSize / 2,
				TextAlign.Right => new Vector2i<int>(scaledCharWidth / 2, scaledCharHeight / 2) - textSize with { Y = 0 },
				_ => new(scaledCharWidth / 2, scaledCharHeight / 2),
			};

			int originX = relativePosition.X;

			foreach (char c in mst.Text)
			{
				float? offset = _font.GetTextureOffset(c);
				if (offset.HasValue)
				{
					Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(mst.Position.X + relativePosition.X, mst.Position.Y + relativePosition.Y, 0);
					Shader.SetMatrix4x4(FontUniforms.Model, scaleMatrix * translationMatrix);
					Shader.SetFloat(FontUniforms.Offset, offset.Value);
					Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
				}

				_font.AdvancePosition(c, ref relativePosition, originX, scaledCharWidth, scaledCharHeight);
			}
		}

		Gl.BindVertexArray(0);

		_collection.Clear();
	}

	private readonly record struct MonoSpaceText(Vector2i<int> Scale, Vector2i<int> Position, string Text, TextAlign TextAlign);
}
