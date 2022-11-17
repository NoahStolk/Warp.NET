namespace Warp.NET.Tests.Utils;

public static class ResourceUtils
{
	public static string GetResourcePath(string fileName)
	{
		return Path.Combine("Resources", fileName);
	}
}
