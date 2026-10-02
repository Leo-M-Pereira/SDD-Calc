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
}
