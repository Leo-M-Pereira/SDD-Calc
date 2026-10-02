using System.Globalization;
using Calculator.Presentation;

namespace Calculator.Tests;

[TestClass]
public sealed class ResultFormatterTests
{
    [TestMethod]
    [DataRow("3.5", "3,5")]
    [DataRow("1.2345665", "1,234567")]
    [DataRow("-1.2345665", "-1,234567")]
    [DataRow("4.0", "4")]
    [DataRow("0.0000004", "0")]
    [DataRow("-0.0000004", "0")]
    public void Format_UsesAcceptedPrecisionAndPresentation(string value, string expected)
    {
        var number = decimal.Parse(value, CultureInfo.InvariantCulture);
        var formatted = ResultFormatter.Format(number);

        Assert.AreEqual(expected, formatted);
    }

    [TestMethod]
    public void Format_OneThird_DisplaysSixDecimalPlaces()
    {
        var formatted = ResultFormatter.Format(0.3333333333333333333333333333m);

        Assert.AreEqual("0,333333", formatted);
    }
}
