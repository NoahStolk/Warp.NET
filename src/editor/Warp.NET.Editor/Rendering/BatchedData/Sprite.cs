using System.Numerics;
using Warp.NET.Numerics;

namespace Warp.NET.Editor.Rendering.BatchedData;

public readonly record struct Sprite(Vector2 Scale, Vector2 CenterPosition, float Depth, Texture Texture, Color Color, Scissor? Scissor);
