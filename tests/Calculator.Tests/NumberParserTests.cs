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
}
