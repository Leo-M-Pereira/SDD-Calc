using System.Globalization;
using Calculator.Domain;

namespace Calculator.Tests;

[TestClass]
public sealed class CalculatorEngineTests
{
    [TestMethod]
    [DataRow(Operation.Addition, 2.0, 3.0, 5.0)]
    [DataRow(Operation.Subtraction, 2.0, 5.0, -3.0)]
    [DataRow(Operation.Multiplication, -4.0, 3.0, -12.0)]
    [DataRow(Operation.Multiplication, -2.0, -3.0, 6.0)]
    [DataRow(Operation.Division, 7.0, 2.0, 3.5)]
    [DataRow(Operation.Addition, 1.5, 2.25, 3.75)]
    [DataRow(Operation.Addition, 0.0, 3.0, 3.0)]
    [DataRow(Operation.Subtraction, 0.0, 5.0, -5.0)]
    [DataRow(Operation.Subtraction, 5.0, 0.0, 5.0)]
    [DataRow(Operation.Multiplication, 0.0, -4.0, 0.0)]
    [DataRow(Operation.Multiplication, -4.0, 0.0, 0.0)]
    [DataRow(Operation.Division, 0.0, 2.0, 0.0)]
    [DataRow(Operation.Addition, 1000000000.0, 0.0, 1000000000.0)]
    [DataRow(Operation.Addition, -1000000000.0, 0.0, -1000000000.0)]
    public void Calculate_ValidOperands_ReturnsExpectedValue(
        Operation operation,
        double first,
        double second,
        double expected)
    {
        var engine = new CalculatorEngine();

        var result = engine.Calculate(operation, (decimal)first, (decimal)second);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual((decimal)expected, result.Value);
        Assert.AreEqual(CalculationError.None, result.Error);
    }

    [TestMethod]
    public void Calculate_PeriodicDivision_RetainsRawDecimalForPresentation()
    {
        var result = new CalculatorEngine().Calculate(Operation.Division, 1m, 3m);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(0.3333333333333333333333333333m, result.Value);
    }

    [TestMethod]
    [DataRow((Operation)99, "2", "3", CalculationError.InvalidOperation)]
    [DataRow(Operation.Addition, "1000000000.000001", "3", CalculationError.InvalidOperand)]
    [DataRow(Operation.Division, "5", "0", CalculationError.DivisionByZero)]
    [DataRow(Operation.Division, "0", "0", CalculationError.DivisionByZero)]
    public void Calculate_InvalidDomainConditions_ReturnsSpecificError(
        Operation operation,
        string first,
        string second,
        CalculationError expectedError)
    {
        var firstValue = decimal.Parse(first, CultureInfo.InvariantCulture);
        var secondValue = decimal.Parse(second, CultureInfo.InvariantCulture);
        var result = new CalculatorEngine().Calculate(operation, firstValue, secondValue);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNull(result.Value);
        Assert.AreEqual(expectedError, result.Error);
    }

    [TestMethod]
    [DataRow(Operation.Addition, 1000000000.0, 0.000001, CalculationError.ResultOutOfRange, 0.0)]
    [DataRow(Operation.Addition, -1000000000.0, -0.000001, CalculationError.ResultOutOfRange, 0.0)]
    [DataRow(Operation.Addition, 1000000000.0, 0.0, CalculationError.None, 1000000000.0)]
    [DataRow(Operation.Addition, -1000000000.0, 0.0, CalculationError.None, -1000000000.0)]
    [DataRow(Operation.Subtraction, -1000000000.0, 0.000001, CalculationError.ResultOutOfRange, 0.0)]
    [DataRow(Operation.Subtraction, 1000000000.0, -0.000001, CalculationError.ResultOutOfRange, 0.0)]
    [DataRow(Operation.Subtraction, 1000000000.0, 0.0, CalculationError.None, 1000000000.0)]
    [DataRow(Operation.Subtraction, -1000000000.0, 0.0, CalculationError.None, -1000000000.0)]
    [DataRow(Operation.Multiplication, 1000000000.0, 1.0, CalculationError.None, 1000000000.0)]
    [DataRow(Operation.Multiplication, -1000000000.0, 1.0, CalculationError.None, -1000000000.0)]
    [DataRow(Operation.Multiplication, 1000000000.0, -1.0, CalculationError.None, -1000000000.0)]
    [DataRow(Operation.Multiplication, -1000000000.0, -1.0, CalculationError.None, 1000000000.0)]
    [DataRow(Operation.Multiplication, 1000000000.0, 1.000001, CalculationError.ResultOutOfRange, 0.0)]
    [DataRow(Operation.Multiplication, -1000000000.0, 1.000001, CalculationError.ResultOutOfRange, 0.0)]
    [DataRow(Operation.Division, 1000000000.0, 1.0, CalculationError.None, 1000000000.0)]
    [DataRow(Operation.Division, -1000000000.0, 1.0, CalculationError.None, -1000000000.0)]
    [DataRow(Operation.Division, 1000000000.0, -1.0, CalculationError.None, -1000000000.0)]
    [DataRow(Operation.Division, -1000000000.0, -1.0, CalculationError.None, 1000000000.0)]
    [DataRow(Operation.Division, 1000000000.0, 0.999999, CalculationError.ResultOutOfRange, 0.0)]
    [DataRow(Operation.Division, -1000000000.0, 0.999999, CalculationError.ResultOutOfRange, 0.0)]
    public void Calculate_ResultBoundaries_ReturnsExpectedBoundaryOrError(
        Operation operation,
        double first,
        double second,
        CalculationError expectedError,
        double expectedValue)
    {
        var result = new CalculatorEngine().Calculate(operation, (decimal)first, (decimal)second);

        Assert.AreEqual(expectedError, result.Error);
        if (expectedError == CalculationError.None)
        {
            Assert.AreEqual((decimal)expectedValue, result.Value);
        }
        else
        {
            Assert.IsNull(result.Value);
        }
    }
}
