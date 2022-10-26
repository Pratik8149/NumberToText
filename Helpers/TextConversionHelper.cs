using System.Globalization;

namespace NumberToText.Helpers;

/// <summary>
/// TextConversionHelper
/// </summary>
public static class TextConversionHelper
{
	/// <summary>
	/// CovertNumberToTextFormat
	/// </summary>
	/// <param name="numberToConvert"></param>
	/// <returns></returns>
	public static string CovertNumberToTextFormat(float numberToConvert)
	{
		var unconvertedString = numberToConvert.ToString(CultureInfo.InvariantCulture);
		var separatedDecimal = unconvertedString.Split(".");

		var dollars = int.Parse(separatedDecimal[0]);
		var cents = separatedDecimal.Length > 1 ? int.Parse(separatedDecimal[1]) : 0;

		var convertedStringForDollars = GetConvertedString(dollars);
		if (convertedStringForDollars.Equals("INVALID"))
		{
			return "INVALID INPUT! \nAMOUNT SHOULD BE POSITIVE!";
		}

		var convertedStringForCents = GetConvertedString(cents);
		if (convertedStringForCents.Equals("INVALID"))
		{
			return "INVALID INPUT! \nENTER ONLY TWO DIGITS AFTER THE DECIMAL POINT!";
		}

		var convertedStringForAmount = convertedStringForDollars + " DOLLARS AND " + convertedStringForCents + " CENTS";

		return convertedStringForAmount;
	}

	/// <summary>
	/// ConvertDollarsToStringFormat
	/// </summary>
	/// <param name="amount"></param>
	/// <param name="isCents"></param>
	/// <returns></returns>
	private static string GetConvertedString(int amount, bool isCents = false)
	{
		if (amount < 0 || (isCents && amount.ToString().Length > 2))
		{
			return "INVALID";
		}

		var _ = Data.Store;
		var convertedDollarsFormat = amount.ToString().Length switch
		{
			1 => _.ContainsKey(amount) ? _[amount] : "INVALID",
			2 => _.ContainsKey(amount) ? _[amount] : DoubleDigitDollarsConversion(amount),
			_ => "INFINITE"
		};

		return convertedDollarsFormat;
	}

	/// <summary>
	/// DoubleDigitDollarsConversion
	/// </summary>
	/// <param name="dollars"></param>
	/// <returns></returns>
	private static string DoubleDigitDollarsConversion(int dollars)
	{
		var _ = Data.Store;
		var tensPlace = dollars / 10;
		var unitsPlace = dollars % 10;

		if (tensPlace == 1)
		{
			return _[unitsPlace] + "TEEN";
		}

		if (_.ContainsKey(tensPlace * 10)) return _[tensPlace * 10];

		var conversionToCombine = _[tensPlace] + "TY";
		if (unitsPlace != 0)
		{
			conversionToCombine = conversionToCombine + "-" + _[unitsPlace];
		}

		return conversionToCombine;
	}
}