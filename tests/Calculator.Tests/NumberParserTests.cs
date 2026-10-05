using Calculator.Presentation;

namespace Calculator.Tests;

[TestClass]
public sealed class NumberParserTests
{
    [TestMethod]
    [DataRow("2", 2.0)]
    [DataRow("-3", -3.0)]
    [DataRow("+2", 2.0)]
    [DataRow("001,5", 1.5)]
    [DataRow("  -2,25  ", -2.25)]
    [DataRow("0,1234560", 0.123456)]
    [DataRow("1000000000", 1000000000.0)]
    [DataRow("-1000000000", -1000000000.0)]
    public void TryParse_ValidContractNumber_ReturnsDecimal(string input, double expected)
    {
        var success = NumberParser.TryParse(input, out var value, out var error);

        Assert.IsTrue(success);
        Assert.AreEqual((decimal)expected, value);
        Assert.AreEqual(InputError.None, error);
    }

    [TestMethod]
    [DataRow("", InputError.InvalidFormat)]
    [DataRow("   ", InputError.InvalidFormat)]
    [DataRow("abc", InputError.InvalidFormat)]
    [DataRow("1.5", InputError.InvalidFormat)]
    [DataRow("1.000,5", InputError.InvalidFormat)]
    [DataRow("1 000", InputError.InvalidFormat)]
    [DataRow("1e3", InputError.InvalidFormat)]
    [DataRow(",5", InputError.InvalidFormat)]
    [DataRow("5,", InputError.InvalidFormat)]
    public void TryParse_InvalidFormat_ReturnsFormatError(string input, InputError expectedError)
    {
        var success = NumberParser.TryParse(input, out _, out var error);

        Assert.IsFalse(success);
        Assert.AreEqual(expectedError, error);
    }

    [TestMethod]
    [DataRow("0,1234567", InputError.PrecisionExceeded)]
    [DataRow("0,12345670", InputError.PrecisionExceeded)]
    [DataRow("1000000000,000001", InputError.OutOfRange)]
    [DataRow("-1000000000,000001", InputError.OutOfRange)]
    [DataRow("999999999999999999999999999999999999", InputError.OutOfRange)]
    public void TryParse_PrecisionAndRangeViolations_ReturnExpectedError(
        string input,
        InputError expectedError)
    {
        var success = NumberParser.TryParse(input, out _, out var error);

        Assert.IsFalse(success);
        Assert.AreEqual(expectedError, error);
    }

    [TestMethod]
    public void TryParse_MalformedTextWithExcessPrecision_PrioritizesFormatError()
    {
        var success = NumberParser.TryParse("1.000,0000001", out _, out var error);

        Assert.IsFalse(success);
        Assert.AreEqual(InputError.InvalidFormat, error);
    }

    [TestMethod]
    public void TryParse_OutOfRangeValueWithExcessPrecision_PrioritizesPrecisionError()
    {
        var success = NumberParser.TryParse("1000000000,0000001", out _, out var error);

        Assert.IsFalse(success);
        Assert.AreEqual(InputError.PrecisionExceeded, error);
    }
}
