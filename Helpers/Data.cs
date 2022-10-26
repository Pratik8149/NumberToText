namespace NumberToText.Helpers;

public abstract class Data
{
	public static readonly Dictionary<int, string> Store = new()
	{
		{0, "ZERO"},
		{1, "ONE"},
		{2, "TWO"},
		{3, "THREE"},
		{4, "FOUR"},
		{5, "FIVE"},
		{6, "SIX"},
		{7, "SEVEN"},
		{8, "EIGHT"},
		{9, "NINE"},
		{10, "TEN"},
		{11, "ELEVEN"},
		{12, "TWELVE"},
		{13, "THIRTEEN"},
		{15, "FIFTEEN"},
		{18, "EIGHTEEN"},
		{20, "TWENTY"},
		{30, "THIRTY"},
		{50, "FIFTY"},
		{80, "EIGHTY"},
	};
}