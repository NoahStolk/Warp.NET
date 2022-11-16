using Warp.NET.Extensions;

namespace Warp.NET.Utils;

public static class RandomUtils
{
	private static readonly Random _random = new();

	/// <summary>
	/// Returns a random <see cref="byte"/> that is greater than or equal to 0, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="byte"/>.</returns>
	public static byte RandomByte(byte maxValue)
		=> _random.RandomByte(maxValue);

	/// <summary>
	/// Returns a random <see cref="byte"/> that is greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="byte"/>.</returns>
	public static byte RandomByte(byte minValue, byte maxValue)
		=> _random.RandomByte(minValue, maxValue);

	/// <summary>
	/// Returns a random <see cref="int"/> that is greater than or equal to 0, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="int"/>.</returns>
	public static int RandomInt(int maxValue)
		=> _random.RandomInt(maxValue);

	/// <summary>
	/// Returns a random <see cref="int"/> that is greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="int"/>.</returns>
	public static int RandomInt(int minValue, int maxValue)
		=> _random.RandomInt(minValue, maxValue);

	/// <summary>
	/// Returns a random <see cref="float"/> that is greater than or equal to 0.0f, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="float"/>.</returns>
	public static float RandomFloat(float maxValue)
		=> _random.RandomFloat(maxValue);

	/// <summary>
	/// Returns a random <see cref="float"/> that is greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="float"/>.</returns>
	public static float RandomFloat(float minValue, float maxValue)
		=> _random.RandomFloat(minValue, maxValue);

	/// <summary>
	/// Returns a random <see cref="double"/> that is greater than or equal to 0.0, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="double"/>.</returns>
	public static double RandomDouble(double maxValue)
		=> _random.RandomDouble(maxValue);

	/// <summary>
	/// Returns a random <see cref="double"/> that is greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="double"/>.</returns>
	public static double RandomDouble(double minValue, double maxValue)
		=> _random.RandomDouble(minValue, maxValue);

	/// <summary>
	/// Returns a random item from the given <paramref name="options"/> array.
	/// </summary>
	/// <typeparam name="T">The type of the items in the array.</typeparam>
	/// <param name="options">The array to choose from.</param>
	/// <returns>The randomly chosen item.</returns>
	public static T Choose<T>(params T[] options)
		=> _random.Choose(options);

	/// <summary>
	/// Returns a random item from the given <paramref name="options"/> span.
	/// </summary>
	/// <typeparam name="T">The type of the items in the span.</typeparam>
	/// <param name="options">The span to choose from.</param>
	/// <returns>The randomly chosen item.</returns>
	public static T Choose<T>(Span<T> options)
		=> _random.Choose(options);

	/// <summary>
	/// Returns true or false based on the <paramref name="percentage"/> parameter.
	/// </summary>
	/// <param name="percentage">The percentage ranging from 0 to 100.</param>
	public static bool Chance(float percentage)
		=> _random.Chance(percentage);

	/// <summary>
	/// Returns a random <see cref="Vector2"/> greater than or equal to <see cref="Vector2.Zero"/>, and less than a <see cref="Vector2"/> with axes that do not exceed <paramref name="maxValue"/>. Both axes for the returned <see cref="Vector2"/> are equal.
	/// </summary>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector2"/>.</returns>
	public static Vector2 RandomEqualVector2(float maxValue)
		=> _random.RandomEqualVector2(maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector2"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>. Both axes for the returned <see cref="Vector2"/> are equal.
	/// </summary>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector2"/>.</returns>
	public static Vector2 RandomEqualVector2(float minValue, float maxValue)
		=> _random.RandomEqualVector2(minValue, maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector2"/> greater than or equal to <see cref="Vector2.Zero"/>, and less than a <see cref="Vector2"/> with axes that do not exceed <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector2"/>.</returns>
	public static Vector2 RandomVector2(float maxValue)
		=> _random.RandomVector2(maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector2"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector2"/>.</returns>
	public static Vector2 RandomVector2(float minValue, float maxValue)
		=> _random.RandomVector2(minValue, maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector2"/> with axes that are greater than or equal to the corresponding min parameters, and less than the corresponding max parameters.
	/// </summary>
	/// <param name="minValueX">The minimum X value for the <see cref="Vector2"/>.</param>
	/// <param name="maxValueX">The maximum X value for the <see cref="Vector2"/>.</param>
	/// <param name="minValueY">The minimum Y value for the <see cref="Vector2"/>.</param>
	/// <param name="maxValueY">The maximum Y value for the <see cref="Vector2"/>.</param>
	/// <returns>The random <see cref="Vector2"/>.</returns>
	public static Vector2 RandomVector2(float minValueX, float maxValueX, float minValueY, float maxValueY)
		=> _random.RandomVector2(minValueX, maxValueX, minValueY, maxValueY);

	/// <summary>
	/// Returns a random <see cref="Vector3"/> greater than or equal to <see cref="Vector3.Zero"/>, and less than a <see cref="Vector3"/> with axes that do not exceed <paramref name="maxValue"/>. All axes for the returned <see cref="Vector3"/> are equal.
	/// </summary>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomEqualVector3(float maxValue)
		=> _random.RandomEqualVector3(maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector3"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>. All axes for the returned <see cref="Vector3"/> are equal.
	/// </summary>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomEqualVector3(float minValue, float maxValue)
		=> _random.RandomEqualVector3(minValue, maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector3"/> greater than or equal to <see cref="Vector3.Zero"/>, and less than a <see cref="Vector3"/> with axes that do not exceed <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomVector3(float maxValue)
		=> _random.RandomVector3(maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector3"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomVector3(float minValue, float maxValue)
		=> _random.RandomVector3(minValue, maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector3"/> greater than or equal to a <see cref="Vector3"/> with axes {-<paramref name="x"/>, -<paramref name="y"/>, -<paramref name="z"/>}, and less than a <see cref="Vector3"/> with axes {<paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/>}.
	/// </summary>
	/// <param name="x">The X value for the <see cref="Vector3"/>.</param>
	/// <param name="y">The Y value for the <see cref="Vector3"/>.</param>
	/// <param name="z">The Z value for the <see cref="Vector3"/>.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomVector3(float x, float y, float z)
		=> _random.RandomVector3(x, y, z);

	/// <summary>
	/// Returns a random <see cref="Vector3"/> with axes that are greater than or equal to the corresponding min parameters, and less than the corresponding max parameters.
	/// </summary>
	/// <param name="minValueX">The minimum X value for the <see cref="Vector3"/>.</param>
	/// <param name="maxValueX">The maximum X value for the <see cref="Vector3"/>.</param>
	/// <param name="minValueY">The minimum Y value for the <see cref="Vector3"/>.</param>
	/// <param name="maxValueY">The maximum Y value for the <see cref="Vector3"/>.</param>
	/// <param name="minValueZ">The minimum Z value for the <see cref="Vector3"/>.</param>
	/// <param name="maxValueZ">The maximum Z value for the <see cref="Vector3"/>.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomVector3(float minValueX, float maxValueX, float minValueY, float maxValueY, float minValueZ, float maxValueZ)
		=> _random.RandomVector3(minValueX, maxValueX, minValueY, maxValueY, minValueZ, maxValueZ);

	/// <summary>
	/// Returns a random <see cref="Vector4"/> greater than or equal to <see cref="Vector4.Zero"/>, and less than a <see cref="Vector4"/> with axes that do not exceed <paramref name="maxValue"/>. All axes for the returned <see cref="Vector4"/> are equal.
	/// </summary>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomEqualVector4(float maxValue)
		=> _random.RandomEqualVector4(maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector4"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>. All axes for the returned <see cref="Vector4"/> are equal.
	/// </summary>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomEqualVector4(float minValue, float maxValue)
		=> _random.RandomEqualVector4(minValue, maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector4"/> greater than or equal to <see cref="Vector4.Zero"/>, and less than a <see cref="Vector4"/> with axes that do not exceed <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomVector4(float maxValue)
		=> _random.RandomVector4(maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector4"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomVector4(float minValue, float maxValue)
		=> _random.RandomVector4(minValue, maxValue);

	/// <summary>
	/// Returns a random <see cref="Vector4"/> greater than or equal to a <see cref="Vector4"/> with axes {-<paramref name="x"/>, -<paramref name="y"/>, -<paramref name="z"/>, -<paramref name="w"/>}, and less than a <see cref="Vector4"/> with axes {<paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/>, <paramref name="w"/>}.
	/// </summary>
	/// <param name="x">The X value for the <see cref="Vector4"/>.</param>
	/// <param name="y">The Y value for the <see cref="Vector4"/>.</param>
	/// <param name="z">The Z value for the <see cref="Vector4"/>.</param>
	/// <param name="w">The W value for the <see cref="Vector4"/>.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomVector4(float x, float y, float z, float w)
		=> _random.RandomVector4(x, y, z, w);

	/// <summary>
	/// Returns a random <see cref="Vector4"/> with axes that are greater than or equal to the corresponding min parameters, and less than the corresponding max parameters.
	/// </summary>
	/// <param name="minValueX">The minimum X value for the <see cref="Vector4"/>.</param>
	/// <param name="maxValueX">The maximum X value for the <see cref="Vector4"/>.</param>
	/// <param name="minValueY">The minimum Y value for the <see cref="Vector4"/>.</param>
	/// <param name="maxValueY">The maximum Y value for the <see cref="Vector4"/>.</param>
	/// <param name="minValueZ">The minimum Z value for the <see cref="Vector4"/>.</param>
	/// <param name="maxValueZ">The maximum Z value for the <see cref="Vector4"/>.</param>
	/// <param name="minValueW">The minimum W value for the <see cref="Vector4"/>.</param>
	/// <param name="maxValueW">The maximum W value for the <see cref="Vector4"/>.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomVector4(float minValueX, float maxValueX, float minValueY, float maxValueY, float minValueZ, float maxValueZ, float minValueW, float maxValueW)
		=> _random.RandomVector4(minValueX, maxValueX, minValueY, maxValueY, minValueZ, maxValueZ, minValueW, maxValueW);

	/// <summary>
	/// Randomly flips the axes of the <see cref="Vector2"/>.
	/// </summary>
	/// <param name="vector">The vector.</param>
	/// <returns>The randomly flipped <see cref="Vector2"/>.</returns>
	public static Vector2 RandomFlip(Vector2 vector)
		=> _random.RandomFlip(vector);

	/// <summary>
	/// Randomly flips the axes of the <see cref="Vector3"/>.
	/// </summary>
	/// <param name="vector">The vector.</param>
	/// <returns>The randomly flipped <see cref="Vector3"/>.</returns>
	public static Vector3 RandomFlip(Vector3 vector)
		=> _random.RandomFlip(vector);

	/// <summary>
	/// Randomly flips the axes of the <see cref="Vector4"/>.
	/// </summary>
	/// <param name="vector">The vector.</param>
	/// <returns>The randomly flipped <see cref="Vector4"/>.</returns>
	public static Vector4 RandomFlip(Vector4 vector)
		=> _random.RandomFlip(vector);

	public static Quaternion RandomQuaternion()
		=> _random.RandomQuaternion();

	public static Quaternion RandomAxisAlignedQuaternion()
		=> _random.RandomAxisAlignedQuaternion();
}
