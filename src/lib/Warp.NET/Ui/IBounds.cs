using Warp.NET.Numerics;

namespace Warp.NET.Ui;

public interface IBounds
{
	int X1 { get; }

	int Y1 { get; }

	int X2 { get; }

	int Y2 { get; }

	public Vector2i<int> TopLeft => new(X1, Y1);

	public Vector2i<int> Size => new(X2 - X1, Y2 - Y1);
}
