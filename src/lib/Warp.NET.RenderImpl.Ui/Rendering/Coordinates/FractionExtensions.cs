namespace Warp.NET.RenderImpl.Ui.Rendering.Coordinates;

public static class FractionExtensions
{
	public static float ToFloat(this Fraction fraction) => fraction switch
	{
		Fraction.F00_01 => 0,
		Fraction.F01_01 => 1,

		Fraction.F01_02 => 01f / 02f,
		Fraction.F01_03 => 01f / 03f,
		Fraction.F01_04 => 01f / 04f,
		Fraction.F01_05 => 01f / 05f,
		Fraction.F01_06 => 01f / 06f,
		Fraction.F01_07 => 01f / 07f,
		Fraction.F01_08 => 01f / 08f,
		Fraction.F01_09 => 01f / 09f,
		Fraction.F01_10 => 01f / 10f,

		Fraction.F02_03 => 02f / 03f,
		Fraction.F02_04 => 02f / 04f,
		Fraction.F02_05 => 02f / 05f,
		Fraction.F02_06 => 02f / 06f,
		Fraction.F02_07 => 02f / 07f,
		Fraction.F02_08 => 02f / 08f,
		Fraction.F02_09 => 02f / 09f,
		Fraction.F02_10 => 02f / 10f,

		Fraction.F03_04 => 03f / 04f,
		Fraction.F03_05 => 03f / 05f,
		Fraction.F03_06 => 03f / 06f,
		Fraction.F03_07 => 03f / 07f,
		Fraction.F03_08 => 03f / 08f,
		Fraction.F03_09 => 03f / 09f,
		Fraction.F03_10 => 03f / 10f,

		Fraction.F04_05 => 04f / 05f,
		Fraction.F04_06 => 04f / 06f,
		Fraction.F04_07 => 04f / 07f,
		Fraction.F04_08 => 04f / 08f,
		Fraction.F04_09 => 04f / 09f,
		Fraction.F04_10 => 04f / 10f,

		Fraction.F05_06 => 05f / 06f,
		Fraction.F05_07 => 05f / 07f,
		Fraction.F05_08 => 05f / 08f,
		Fraction.F05_09 => 05f / 09f,
		Fraction.F05_10 => 05f / 10f,

		Fraction.F06_07 => 06f / 07f,
		Fraction.F06_08 => 06f / 08f,
		Fraction.F06_09 => 06f / 09f,
		Fraction.F06_10 => 06f / 10f,

		Fraction.F07_08 => 07f / 08f,
		Fraction.F07_09 => 07f / 09f,
		Fraction.F07_10 => 07f / 10f,

		Fraction.F08_09 => 08f / 09f,
		Fraction.F08_10 => 08f / 10f,

		Fraction.F09_10 => 09f / 10f,

		_ => throw new NotSupportedException($"Fraction '{fraction}' is not supported."),
	};
}
