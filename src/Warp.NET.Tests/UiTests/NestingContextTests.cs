using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.Tests.UiTests;

[TestClass]
public class NestingContextTests
{
	private readonly NestingContext _nc;
	private readonly Component _component1;
	private readonly Component _component2;
	private readonly Component _component3;

	public NestingContextTests()
	{
		_nc = new(Rectangle.At(0, 0, 100, 100));
		_component1 = new();
		_component2 = new();
		_component3 = new();
	}

	[TestMethod]
	public void TestValid()
	{
		_nc.Add(_component1);
		_nc.Add(_component2);

		_nc.Update(default);
		Assert.AreEqual(2, _nc.OrderedComponents.Count);
		Assert.IsTrue(_nc.OrderedComponents.Contains(_component1));
		Assert.IsTrue(_nc.OrderedComponents.Contains(_component2));
		Assert.IsFalse(_nc.OrderedComponents.Contains(_component3));

		_nc.Remove(_component1);
		_nc.Add(_component3);

		_nc.Update(default);
		Assert.AreEqual(2, _nc.OrderedComponents.Count);
		Assert.IsFalse(_nc.OrderedComponents.Contains(_component1));
		Assert.IsTrue(_nc.OrderedComponents.Contains(_component2));
		Assert.IsTrue(_nc.OrderedComponents.Contains(_component3));
	}

	[TestMethod]
	public void TestSwitch()
	{
		for (int i = 0; i < 2; i++)
		{
			_nc.Add(_component1);
			_nc.Update(default);
			Assert.AreEqual(1, _nc.OrderedComponents.Count);
			Assert.IsTrue(_nc.OrderedComponents.Contains(_component1));

			_nc.Remove(_component1);
			_nc.Update(default);
			Assert.AreEqual(0, _nc.OrderedComponents.Count);
		}
	}

	[TestMethod]
	public void TestDuplicateAdd()
	{
		_nc.Add(_component1);
		_nc.Add(_component1);
		Assert.ThrowsException<InvalidOperationException>(() => _nc.Update(default));
	}

	[TestMethod]
	public void TestDuplicateAdd_AfterUpdate()
	{
		_nc.Add(_component1);
		_nc.Update(default);
		_nc.Add(_component1);
		Assert.ThrowsException<InvalidOperationException>(() => _nc.Update(default));
	}

	[TestMethod]
	public void TestAddAndRemove()
	{
		_nc.Add(_component1);
		_nc.Remove(_component1);
		Assert.ThrowsException<InvalidOperationException>(() => _nc.Update(default));
	}

	private sealed class Component : AbstractComponent
	{
		public Component()
			: base(Rectangle.At(0, 0, 100, 100))
		{
		}
	}

	private sealed record Rectangle(int X1, int Y1, int X2, int Y2) : IBounds
	{
		public Vector2i<int> TopLeft => new(X1, Y1);

		public Vector2i<int> Size => new(X2 - X1, Y2 - Y1);

		public bool Contains(int x, int y)
		{
			return x >= X1 && x <= X2 && y >= Y1 && y <= Y2;
		}

		public bool Contains(Vector2i<int> position)
		{
			return Contains(position.X, position.Y);
		}

		public bool IntersectsOrContains(IBounds other)
		{
			return IntersectsOrContains(other.X1, other.Y1, other.X2, other.Y2);
		}

		public bool IntersectsOrContains(int x1, int y1, int x2, int y2)
		{
			Vector2i<int> a = new(x1, y1);
			Vector2i<int> b = new(x2, y1);
			Vector2i<int> c = new(x1, y2);
			Vector2i<int> d = new(x2, y2);

			return Contains(a) || Contains(b) || Contains(c) || Contains(d);
		}

		public static Rectangle At(int left, int top, int width, int height)
		{
			return new(left, top, left + width, top + height);
		}

		public static IBounds Move(IBounds original, Vector2i<int> offset)
		{
			return new Rectangle(original.X1 + offset.X, original.Y1 + offset.Y, original.X2 + offset.X, original.Y2 + offset.Y);
		}
	}
}
