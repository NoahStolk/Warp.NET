using Warp.NET.Numerics;

namespace Warp.NET.Ui;

public interface IBounds
{
	int X1 { get; }

	int Y1 { get; }

	int X2 { get; }

	int Y2 { get; }

	Vector2i<int> TopLeft { get; }

	Vector2i<int> Size { get; }

	bool Contains(int x, int y);

	bool Contains(Vector2i<int> position);

	bool IntersectsOrContains(IBounds other);

	bool IntersectsOrContains(int x1, int y1, int x2, int y2);

	static abstract IBounds Move(IBounds original, Vector2i<int> offset);
}
