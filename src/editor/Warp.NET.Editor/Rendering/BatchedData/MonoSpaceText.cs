using Warp.NET.Numerics;
using Warp.NET.Text;

namespace Warp.NET.Editor.Rendering.BatchedData;

public readonly record struct MonoSpaceText(Vector2i<int> Scale, Vector2i<int> Position, float Depth, Color Color, string Text, TextAlign TextAlign, Scissor? Scissor);
