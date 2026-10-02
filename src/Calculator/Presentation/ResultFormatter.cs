using System.Globalization;

namespace Calculator.Presentation;

public static class ResultFormatter
{
    private static readonly CultureInfo BrazilianCulture = CultureInfo.GetCultureInfo("pt-BR");

    public static string Format(decimal value)
    {
        var rounded = decimal.Round(value, 6, MidpointRounding.AwayFromZero);
        if (rounded == 0m)
        {
            return "0";
        }

        return rounded.ToString("0.######", BrazilianCulture);
    }
}
