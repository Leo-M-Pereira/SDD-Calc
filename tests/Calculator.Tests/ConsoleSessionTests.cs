using System.Globalization;
using System.IO;
using Calculator.Presentation;

namespace Calculator.Tests;

[TestClass]
public sealed class ConsoleSessionTests
{
    [TestMethod]
    [DataRow("1", "2", "3", "5")]
    [DataRow("2", "2", "5", "-3")]
    [DataRow("3", "-4", "3", "-12")]
    [DataRow("4", "7", "2", "3,5")]
    public void Run_ValidCalculation_DisplaysMenuAndResult(
        string option,
        string first,
        string second,
        string expected)
    {
        using var input = new StringReader($"{option}{Environment.NewLine}{first}{Environment.NewLine}{second}");
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        var transcript = output.ToString();
        StringAssert.Contains(transcript, "Adição");
        StringAssert.Contains(transcript, "Subtração");
        StringAssert.Contains(transcript, "Multiplicação");
        StringAssert.Contains(transcript, "Divisão");
        StringAssert.Contains(transcript, $"Resultado: {expected}");
    }

    [TestMethod]
    public void Run_ExitAtInitialMenu_ConfirmsWithoutRequestingOperands()
    {
        using var input = new StringReader($"0{Environment.NewLine}");
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        var transcript = output.ToString();
        StringAssert.Contains(transcript, "Programa encerrado.");
        Assert.IsFalse(transcript.Contains("primeiro número", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(transcript.Contains("segundo número", StringComparison.OrdinalIgnoreCase));
    }
}
