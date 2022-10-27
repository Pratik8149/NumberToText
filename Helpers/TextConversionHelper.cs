using System.Globalization;

namespace NumberToText.Helpers;

/// <summary>
/// TextConversionHelper
/// </summary>
public static class TextConversionHelper
{
	private const string ExceededLimitError = "INVALID INPUT! => THE AMOUNT SHOULD BE BELOW $100,000";
	private const string NegativeNumberError = "INVALID INPUT! => AMOUNT SHOULD BE POSITIVE!";
	private const string DecimalPlacesError = "INVALID INPUT! => ENTER ONLY TWO DIGITS AFTER THE DECIMAL POINT!";

	/// <summary>
	/// CovertNumberToTextFormat
	/// </summary>
	/// <param name="numberToConvert"></param>
	/// <returns></returns>
	public static string CovertNumberToTextFormat(float numberToConvert)
	{
		if (numberToConvert < 0)
		{
			return NegativeNumberError;
		}

		var unconvertedString = numberToConvert.ToString(CultureInfo.InvariantCulture);
		var separatedDecimal = unconvertedString.Split(".");

		var dollars = int.Parse(separatedDecimal[0]);
		var cents = separatedDecimal.Length > 1 ? int.Parse(separatedDecimal[1]) : 0;

		var convertedStringForDollars = GetConvertedString(dollars);
		switch (convertedStringForDollars)
		{
			case "INFINITE":
				return ExceededLimitError;
			case "INVALID":
				return NegativeNumberError;
		}

		var convertedStringForCents = GetConvertedString(cents);
		if (convertedStringForCents.Equals("INVALID"))
		{
			return DecimalPlacesError;
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

		var store = Data.Store;
		var convertedDollarsFormat = amount.ToString().Length switch
		{
			1 => store.ContainsKey(amount) ? store[amount] : "INVALID",
			2 => store.ContainsKey(amount) ? store[amount] : DoubleDigitDollarsConversion(amount, store),
			3 => ThreeDigitsDollarsConversion(amount, store),
			4 => FourDigitsDollarsConversion(amount, store),
			5 => FiveDigitsDollarsConversion(amount, store),
			_ => "INFINITE"
		};

		return convertedDollarsFormat;
	}

	/// <summary>
	/// DoubleDigitDollarsConversion
	/// </summary>
	/// <param name="dollars"></param>
	/// <param name="store"></param>
	/// <returns></returns>
	private static string DoubleDigitDollarsConversion(int dollars, IReadOnlyDictionary<int, string> store)
	{
		var tensPlace = dollars / 10;
		var unitsPlace = dollars % 10;

		if (tensPlace == 1)
		{
			return store[unitsPlace] + "TEEN";
		}

		var textConversionToCombine = store.ContainsKey(tensPlace * 10) ? store[tensPlace * 10] : store[tensPlace] + "TY";
		if (unitsPlace != 0)
		{
			textConversionToCombine = textConversionToCombine + "-" + store[unitsPlace];
		}

		return textConversionToCombine;
	}

	/// <summary>
	/// ThreeDigitsDollarsConversion
	/// </summary>
	/// <param name="dollars"></param>
	/// <param name="store"></param>
	/// <returns></returns>
	private static string ThreeDigitsDollarsConversion(int dollars, IReadOnlyDictionary<int, string> store)
	{
		var hundredsPlace = dollars / 100;
		if (!store.ContainsKey(hundredsPlace)) return "INVALID";

		var remainingNumbers = dollars % 100;
		var convertedTextForRemainingNumbers = DoubleDigitDollarsConversion(remainingNumbers, store);

		return store[hundredsPlace] + " HUNDRED AND " + convertedTextForRemainingNumbers;
	}

	/// <summary>
	/// FourDigitsDollarsConversion
	/// </summary>
	/// <param name="dollars"></param>
	/// <param name="store"></param>
	/// <returns></returns>
	private static string FourDigitsDollarsConversion(int dollars, IReadOnlyDictionary<int, string> store)
	{
		var hundredsPlace = dollars / 1000;
		if (!store.ContainsKey(hundredsPlace)) return "INVALID";

		var remainingNumbers = dollars % 1000;
		var convertedTextForRemainingNumbers = ThreeDigitsDollarsConversion(remainingNumbers, store);

		return store[hundredsPlace] + " THOUSAND " + convertedTextForRemainingNumbers;
	}

	/// <summary>
	/// FiveDigitsDollarsConversion
	/// </summary>
	/// <param name="dollars"></param>
	/// <param name="store"></param>
	/// <returns></returns>
	private static string FiveDigitsDollarsConversion(int dollars, IReadOnlyDictionary<int, string> store)
	{
		var firstTwoPlaces = dollars / 1000;
		var remainingNumbers = dollars % 1000;

		var convertFirstTwoPlaces = DoubleDigitDollarsConversion(firstTwoPlaces, store);
		var convertedTextForRemainingNumbers = ThreeDigitsDollarsConversion(remainingNumbers, store);

		return convertFirstTwoPlaces + " THOUSAND " + convertedTextForRemainingNumbers;
	}
}