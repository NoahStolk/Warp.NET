using Warp.NET.Extensions;

namespace Warp.NET.Tests.ExtensionTests;

[TestClass]
public class VectorBinaryTests
{
	[DataTestMethod]
	[DataRow(-1, 2)]
	[DataRow(0, 0)]
	[DataRow(1, 2)]
	[DataRow(-49.5f, 5.2f)]
	[DataRow(float.MinValue, float.MinValue)]
	[DataRow(float.MaxValue, float.MaxValue)]
	public void TestBinaryConversion_Vector2(float x, float y)
	{
		Vector2 vector = new(x, y);

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(vector);

		ms.Position = 0;
		using BinaryReader br = new(ms);
		Assert.AreEqual(vector, br.ReadVector2());
	}

	[DataTestMethod]
	[DataRow(-1, 2, 3)]
	[DataRow(0, 0, 0)]
	[DataRow(1, 2, 3)]
	[DataRow(-49.5f, 5.2f, 60)]
	[DataRow(float.MinValue, float.MinValue, float.MinValue)]
	[DataRow(float.MaxValue, float.MaxValue, float.MaxValue)]
	public void TestBinaryConversion_Vector3(float x, float y, float z)
	{
		Vector3 vector = new(x, y, z);

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(vector);

		ms.Position = 0;
		using BinaryReader br = new(ms);
		Assert.AreEqual(vector, br.ReadVector3());
	}

	[DataTestMethod]
	[DataRow(-1, 2)]
	[DataRow(0, 0)]
	[DataRow(1, 2)]
	[DataRow(-49, 5)]
	[DataRow(int.MinValue, int.MinValue)]
	[DataRow(int.MaxValue, int.MaxValue)]
	public void TestBinaryConversion_IntegralVector2(int x, int y)
	{
		// Signed
		TestBinaryConversionInt32(new(x, y));
		TestBinaryConversionInt16(new((short)x, (short)y));
		TestBinaryConversionSByte(new((sbyte)x, (sbyte)y));

		// Unsigned
		TestBinaryConversionUInt32(new((uint)x, (uint)y));
		TestBinaryConversionUInt16(new((ushort)x, (ushort)y));
		TestBinaryConversionByte(new((byte)x, (byte)y));

		static void TestBinaryConversionInt32(Vector2i<int> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector2Int32());

		static void TestBinaryConversionInt16(Vector2i<short> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector2Int16());

		static void TestBinaryConversionSByte(Vector2i<sbyte> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector2SByte());

		static void TestBinaryConversionUInt32(Vector2i<uint> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector2UInt32());

		static void TestBinaryConversionUInt16(Vector2i<ushort> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector2UInt16());

		static void TestBinaryConversionByte(Vector2i<byte> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector2Byte());

		static void TestBinaryConversion<T>(Vector2i<T> vector, Action<BinaryWriter, Vector2i<T>> writer, Func<BinaryReader, Vector2i<T>> reader)
			where T : IBinaryInteger<T>, IMinMaxValue<T>
		{
			using MemoryStream ms = new();
			using BinaryWriter bw = new(ms);
			writer(bw, vector);

			ms.Position = 0;
			using BinaryReader br = new(ms);
			Assert.AreEqual(vector, reader(br));
		}
	}

	[DataTestMethod]
	[DataRow(-1, 2, 3)]
	[DataRow(0, 0, 0)]
	[DataRow(1, 2, 3)]
	[DataRow(-49, 5, 60)]
	[DataRow(int.MinValue, int.MinValue, int.MinValue)]
	[DataRow(int.MaxValue, int.MaxValue, int.MaxValue)]
	public void TestBinaryConversion_IntegralVector3(int x, int y, int z)
	{
		// Signed
		TestBinaryConversionInt32(new(x, y, z));
		TestBinaryConversionInt16(new((short)x, (short)y, (short)z));
		TestBinaryConversionSByte(new((sbyte)x, (sbyte)y, (sbyte)z));

		// Unsigned
		TestBinaryConversionUInt32(new((uint)x, (uint)y, (uint)z));
		TestBinaryConversionUInt16(new((ushort)x, (ushort)y, (ushort)z));
		TestBinaryConversionByte(new((byte)x, (byte)y, (byte)z));

		static void TestBinaryConversionInt32(Vector3i<int> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector3Int32());

		static void TestBinaryConversionInt16(Vector3i<short> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector3Int16());

		static void TestBinaryConversionSByte(Vector3i<sbyte> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector3SByte());

		static void TestBinaryConversionUInt32(Vector3i<uint> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector3UInt32());

		static void TestBinaryConversionUInt16(Vector3i<ushort> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector3UInt16());

		static void TestBinaryConversionByte(Vector3i<byte> vector)
			=> TestBinaryConversion(vector, (bw, v) => bw.Write(v), br => br.ReadVector3Byte());

		static void TestBinaryConversion<T>(Vector3i<T> vector, Action<BinaryWriter, Vector3i<T>> writer, Func<BinaryReader, Vector3i<T>> reader)
			where T : IBinaryInteger<T>, IMinMaxValue<T>
		{
			using MemoryStream ms = new();
			using BinaryWriter bw = new(ms);
			writer(bw, vector);

			ms.Position = 0;
			using BinaryReader br = new(ms);
			Assert.AreEqual(vector, reader(br));
		}
	}
}
