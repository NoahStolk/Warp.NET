using Warp.NET.Numerics;

namespace Warp.NET.Editor.Rendering.BatchedData;

public readonly record struct RectangleTriangle(Vector2i<int> Scale, Vector2i<int> CenterPosition, float Depth, Color Color, Scissor? Scissor);
