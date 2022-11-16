namespace Warp.NET.Tests.Utils;

public static class AssertionUtils
{
	public static void AreVectorsEqual(Vector3? a, Vector3? b, float delta = 0)
	{
		if (a == null || b == null)
			Assert.AreEqual(a, b);
		else
			AreVectorsEqual(a.Value, b.Value, delta);
	}

	private static void AreVectorsEqual(Vector3 a, Vector3 b, float delta = 0)
	{
		Assert.AreEqual(a.X, b.X, delta);
		Assert.AreEqual(a.Y, b.Y, delta);
		Assert.AreEqual(a.Z, b.Z, delta);
	}
}
