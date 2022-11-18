using System.Numerics;
using Warp.NET.Editor.Rendering.BatchedData;
using Warp.NET.Extensions;
using Warp.NET.Numerics;
using Warp.NET.Text;

namespace Warp.NET.Editor.Rendering;

public static class RenderBatchCollector
{
	private static Scissor? _contextScissor;

	public static List<RectangleTriangle> RectangleTriangles { get; } = new();
	public static List<CircleLine> CircleLines { get; } = new();
	public static List<MonoSpaceText> MonoSpaceTexts { get; } = new();
	public static List<Sprite> Sprites { get; } = new();

	/// <summary>
	/// Clears the operations, should be done after every frame.
	/// </summary>
	public static void Clear()
	{
		RectangleTriangles.Clear();
		CircleLines.Clear();
		MonoSpaceTexts.Clear();
		Sprites.Clear();
	}

	/// <summary>
	/// Sets the scissor test for future render calls. Future render calls will be batched with this scissor test.
	/// </summary>
	public static void SetScissor(Scissor scissor)
	{
		_contextScissor = scissor;
	}

	/// <summary>
	/// Unsets the scissor test for future render calls. Future render calls will not be batched with a scissor test.
	/// </summary>
	public static void UnsetScissor()
	{
		_contextScissor = null;
	}

	/// <summary>
	/// Schedules a rendering operation which will render a rectangle.
	/// </summary>
	public static void RenderRectangleTopLeft(Vector2i<int> scale, Vector2i<int> topLeft, float depth, Color color)
	{
		RenderRectangleCenter(scale, topLeft + scale / 2, depth, color);
	}

	/// <summary>
	/// Schedules a rendering operation which will render a rectangle.
	/// </summary>
	public static void RenderRectangleCenter(Vector2i<int> scale, Vector2i<int> center, float depth, Color color)
	{
		RectangleTriangles.Add(new(scale, center, depth, color, _contextScissor));
	}

	/// <summary>
	/// Schedules a rendering operation which will render a circle.
	/// </summary>
	public static void RenderCircleCenter(Vector2i<int> center, float radius, float depth, Color color)
	{
		CircleLines.Add(new(center, radius, depth, color, _contextScissor));
	}

	/// <summary>
	/// Schedules a rendering operation which will render mono-space text.
	/// </summary>
	public static void RenderMonoSpaceText(Vector2i<int> scale, Vector2i<int> position, float depth, Color color, object? obj, TextAlign textAlign)
	{
		string? text = obj?.ToString();
		if (string.IsNullOrWhiteSpace(text))
			return;

		MonoSpaceTexts.Add(new(scale, position, depth, color, text, textAlign, _contextScissor));
	}

	/// <summary>
	/// Schedules a rendering operation which will render a sprite.
	/// </summary>
	public static void RenderSprite(Vector2 scale, Vector2i<int> centerPosition, float depth, Texture texture, Color color)
	{
		RenderSprite(scale, centerPosition.ToVector2(), depth, texture, color);
	}

	/// <summary>
	/// Schedules a rendering operation which will render a sprite.
	/// </summary>
	public static void RenderSprite(Vector2 scale, Vector2 centerPosition, float depth, Texture texture, Color color)
	{
		Sprites.Add(new(scale, centerPosition, depth, texture, color, _contextScissor));
	}
}
