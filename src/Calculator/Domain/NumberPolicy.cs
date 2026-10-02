namespace Calculator.Domain;

public static class NumberPolicy
{
    public const decimal MinimumValue = -1_000_000_000m;
    public const decimal MaximumValue = 1_000_000_000m;
    public const int MaximumFractionalDigits = 6;

    public static bool IsInRange(decimal value) =>
        value >= MinimumValue && value <= MaximumValue;

    public static bool HasAllowedPrecision(decimal value) =>
        value == decimal.Truncate(value * 1_000_000m) / 1_000_000m;

    public static bool IsValidOperand(decimal value) =>
        IsInRange(value) && HasAllowedPrecision(value);
}
