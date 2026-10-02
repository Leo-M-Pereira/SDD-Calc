using System.Globalization;
using System.Text;
using Calculator.Domain;

namespace Calculator.Presentation;

public static class NumberParser
{
    private static readonly CultureInfo BrazilianCulture = CultureInfo.GetCultureInfo("pt-BR");
    private const NumberStyles NumberStyles =
        System.Globalization.NumberStyles.AllowLeadingSign |
        System.Globalization.NumberStyles.AllowDecimalPoint;

    public static bool TryParse(string? input, out decimal value, out InputError error)
    {
        value = 0m;
        error = InputError.None;

        if (input is null)
        {
            error = InputError.InvalidFormat;
            return false;
        }

        var text = input.Trim();
        if (text.Length == 0)
        {
            error = InputError.InvalidFormat;
            return false;
        }

        var index = 0;
        if (text[index] is '+' or '-')
        {
            index++;
        }

        var integerStart = index;
        while (index < text.Length && IsAsciiDigit(text[index]))
        {
            index++;
        }

        if (index == integerStart)
        {
            error = InputError.InvalidFormat;
            return false;
        }

        var fractionStart = -1;
        if (index < text.Length && text[index] == ',')
        {
            index++;
            fractionStart = index;
            while (index < text.Length && IsAsciiDigit(text[index]))
            {
                index++;
            }

            if (index == fractionStart)
            {
                error = InputError.InvalidFormat;
                return false;
            }
        }

        if (index != text.Length)
        {
            error = InputError.InvalidFormat;
            return false;
        }

        var fractionLength = fractionStart < 0 ? 0 : text.Length - fractionStart;
        while (fractionLength > 0 && text[fractionStart + fractionLength - 1] == '0')
        {
            fractionLength--;
        }

        if (fractionLength > NumberPolicy.MaximumFractionalDigits)
        {
            error = InputError.PrecisionExceeded;
            return false;
        }

        var integerLength = (fractionStart < 0 ? text.Length : fractionStart - 1) - integerStart;
        var significantIntegerStart = integerStart;
        while (significantIntegerStart < integerStart + integerLength - 1 &&
               text[significantIntegerStart] == '0')
        {
            significantIntegerStart++;
        }

        var normalized = new StringBuilder(text.Length);
        if (text[0] is '+' or '-')
        {
            normalized.Append(text[0]);
        }

        normalized.Append(text, significantIntegerStart, integerStart + integerLength - significantIntegerStart);
        if (fractionLength > 0)
        {
            normalized.Append(',');
            normalized.Append(text, fractionStart, fractionLength);
        }

        if (!decimal.TryParse(normalized.ToString(), NumberStyles, BrazilianCulture, out value) ||
            !NumberPolicy.IsInRange(value))
        {
            value = 0m;
            error = InputError.OutOfRange;
            return false;
        }

        return true;
    }

    private static bool IsAsciiDigit(char value) => value is >= '0' and <= '9';
}
