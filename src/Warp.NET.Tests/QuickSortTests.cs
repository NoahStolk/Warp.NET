using Warp.NET.Sorting;

namespace Warp.NET.Tests;

[TestClass]
public class QuickSortTests
{
	[TestMethod]
	public void QuickSort_Reverse()
	{
		const int length = 30;
		float[] arr = Enumerable.Range(0, length).Reverse().Select(i => (float)i).ToArray();

		QuickSort.Sort(arr);

		for (int i = 0; i < length; i++)
			Assert.AreEqual(i, arr[i]);
	}

	[TestMethod]
	public void QuickSort_Random()
	{
		float[] arr = { 8.4f, 8.3333f, 1.2f, 44.5f, -123 };

		QuickSort.Sort(arr);

		Assert.AreEqual(-123, arr[0]);
		Assert.AreEqual(1.2f, arr[1]);
		Assert.AreEqual(8.3333f, arr[2]);
		Assert.AreEqual(8.4f, arr[3]);
		Assert.AreEqual(44.5f, arr[4]);
	}

	[TestMethod]
	public void QuickSort_Empty()
	{
		float[] arr = Array.Empty<float>();

		QuickSort.Sort(arr);

		Assert.AreEqual(0, arr.Length);
	}

	[TestMethod]
	public void QuickSort_Single()
	{
		float[] arr = { 4 };

		QuickSort.Sort(arr);

		Assert.AreEqual(4, arr[0]);
	}

	[TestMethod]
	public void QuickSort_Same()
	{
		float[] arr = { -4, -4, -4 };

		QuickSort.Sort(arr);

		Assert.AreEqual(-4, arr[0]);
		Assert.AreEqual(-4, arr[1]);
		Assert.AreEqual(-4, arr[2]);
	}
}
