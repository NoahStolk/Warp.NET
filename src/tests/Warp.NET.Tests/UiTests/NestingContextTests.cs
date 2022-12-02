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
		_nc = new(new NormalizedBounds(0, 0, 0.1f, 0.1f));
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
			: base(new NormalizedBounds(0, 0, 0.1f, 0.1f))
		{
		}
	}
}
