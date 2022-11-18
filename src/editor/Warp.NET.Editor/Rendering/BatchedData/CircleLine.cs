using Warp.NET.Numerics;

namespace Warp.NET.Editor.Rendering.BatchedData;

public readonly record struct CircleLine(Vector2i<int> CenterPosition, float Radius, float Depth, Color Color, Scissor? Scissor);
