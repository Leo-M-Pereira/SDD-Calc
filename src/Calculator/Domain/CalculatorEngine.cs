namespace Calculator.Domain;

public sealed class CalculatorEngine
{
    public CalculationResult Calculate(Operation operation, decimal first, decimal second)
    {
        if (!Enum.IsDefined(operation))
        {
            return CalculationResult.Failed(CalculationError.InvalidOperation);
        }

        if (!NumberPolicy.IsValidOperand(first) || !NumberPolicy.IsValidOperand(second))
        {
            return CalculationResult.Failed(CalculationError.InvalidOperand);
        }

        if (operation == Operation.Division && second == 0m)
        {
            return CalculationResult.Failed(CalculationError.DivisionByZero);
        }

        try
        {
            var value = operation switch
            {
                Operation.Addition => first + second,
                Operation.Subtraction => first - second,
                Operation.Multiplication => first * second,
                Operation.Division => first / second,
                _ => throw new ArgumentOutOfRangeException(nameof(operation))
            };

            return CalculationResult.Succeeded(value);
        }
        catch (OverflowException)
        {
            return CalculationResult.Failed(CalculationError.ResultOutOfRange);
        }
    }
}
