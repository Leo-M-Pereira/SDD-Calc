namespace Calculator.Domain;

public enum CalculationError
{
    None,
    InvalidOperation,
    InvalidOperand,
    DivisionByZero,
    ResultOutOfRange
}

public sealed class CalculationResult
{
    private CalculationResult(decimal? value, CalculationError error)
    {
        Value = value;
        Error = error;
    }

    public decimal? Value { get; }

    public CalculationError Error { get; }

    public bool IsSuccess => Error == CalculationError.None && Value.HasValue;

    public static CalculationResult Succeeded(decimal value) =>
        new(value, CalculationError.None);

    public static CalculationResult Failed(CalculationError error)
    {
        if (error == CalculationError.None)
        {
            throw new ArgumentException("A failed result must specify an error.", nameof(error));
        }

        return new(null, error);
    }
}
