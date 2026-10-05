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

    [TestMethod]
    public void Run_InvalidOperation_ExplainsAndRepeatsMenu()
    {
        using var input = new StringReader(string.Join(Environment.NewLine, "9", "1", "2", "3"));
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        var transcript = output.ToString();
        StringAssert.Contains(transcript, "Operação inválida.");
        StringAssert.Contains(transcript, "Resultado: 5");
        Assert.AreEqual(3, CountOccurrences(transcript, "Calculadora"));
    }

    [TestMethod]
    [DataRow(true, "abc", "Número inválido.", "Informe o primeiro número:", "Informe o segundo número:")]
    [DataRow(false, "1.5", "Número inválido.", "Informe o segundo número", "Informe o primeiro número:")]
    [DataRow(false, "0,1234567", "seis casas decimais", "Informe o segundo número", "Informe o primeiro número:")]
    [DataRow(true, "1000000000,000001", "entre -1000000000 e 1000000000", "Informe o primeiro número:", "Informe o segundo número:")]
    public void Run_InvalidNumber_ExplainsAndRepeatsOnlyThatOperand(
        bool invalidFirst,
        string invalidValue,
        string expectedMessage,
        string repeatedPrompt,
        string preservedPrompt)
    {
        var lines = invalidFirst
            ? new[] { "1", invalidValue, "2", "3" }
            : new[] { "1", "2", invalidValue, "3" };
        using var input = new StringReader(string.Join(Environment.NewLine, lines));
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        var transcript = output.ToString();
        StringAssert.Contains(transcript, expectedMessage);
        StringAssert.Contains(transcript, "Resultado: 5");
        Assert.AreEqual(2, CountOccurrences(transcript, repeatedPrompt));
        Assert.AreEqual(1, CountOccurrences(transcript, preservedPrompt));
        Assert.AreEqual(2, CountOccurrences(transcript, "Calculadora"));
    }

    [TestMethod]
    public void Run_DivisionByZero_ExplainsAndRepeatsOnlyDivisor()
    {
        using var input = new StringReader(string.Join(Environment.NewLine, "4", "5", "0", "2"));
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        var transcript = output.ToString();
        StringAssert.Contains(transcript, "Não é possível dividir por zero.");
        StringAssert.Contains(transcript, "Resultado: 2,5");
        Assert.AreEqual(1, CountOccurrences(transcript, "Informe o primeiro número:"));
        Assert.AreEqual(2, CountOccurrences(transcript, "Informe o segundo número"));
    }

    [TestMethod]
    [DataRow("1", "1000000000", "0,000001")]
    [DataRow("2", "-1000000000", "0,000001")]
    [DataRow("3", "1000000000", "1,000001")]
    [DataRow("4", "1000000000", "0,999999")]
    public void Run_ExcessiveResult_ExplainsDiscardsOperandsAndReturnsToMenu(
        string operation,
        string first,
        string second)
    {
        var lines = new[] { operation, first, second, "1", "2", "3" };
        using var input = new StringReader(string.Join(Environment.NewLine, lines));
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        var transcript = output.ToString();
        StringAssert.Contains(transcript, "O resultado excede o intervalo permitido.");
        StringAssert.Contains(transcript, "Resultado: 5");
        Assert.AreEqual(3, CountOccurrences(transcript, "Calculadora"));
        Assert.AreEqual(2, CountOccurrences(transcript, "Informe o primeiro número:"));
        Assert.AreEqual(2, CountOccurrences(transcript, "Informe o segundo número"));
        Assert.AreEqual(1, CountOccurrences(transcript, "Resultado:"));
    }

    [TestMethod]
    public void Run_ConsecutiveCalculations_UsesFreshOperandsForEachResult()
    {
        using var input = new StringReader(string.Join(Environment.NewLine, "1", "2", "3", "2", "5", "2", "0"));
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        var transcript = output.ToString();
        Assert.AreEqual(2, CountOccurrences(transcript, "Resultado:"));
        StringAssert.Contains(transcript, "Resultado: 5");
        StringAssert.Contains(transcript, "Resultado: 3");
        Assert.AreEqual(2, CountOccurrences(transcript, "Informe o primeiro número:"));
        Assert.AreEqual(2, CountOccurrences(transcript, "Informe o segundo número"));
        Assert.AreEqual(3, CountOccurrences(transcript, "Calculadora"));
        StringAssert.Contains(transcript, "Programa encerrado.");
    }

    [TestMethod]
    public void Run_TenCalculations_CompletesInOneSession()
    {
        var lines = Enumerable.Range(0, 10)
            .SelectMany(_ => new[] { "1", "1", "1" })
            .Append("0");
        using var input = new StringReader(string.Join(Environment.NewLine, lines));
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        var transcript = output.ToString();
        Assert.AreEqual(10, CountOccurrences(transcript, "Resultado: 2"));
        Assert.AreEqual(10, CountOccurrences(transcript, "Informe o primeiro número:"));
        Assert.AreEqual(10, CountOccurrences(transcript, "Informe o segundo número:"));
        Assert.AreEqual(11, CountOccurrences(transcript, "Calculadora"));
        StringAssert.Contains(transcript, "Programa encerrado.");
    }

    [TestMethod]
    public void Run_InvalidOptionThenExit_ConfirmsWithoutRequestingOperands()
    {
        using var input = new StringReader(string.Join(Environment.NewLine, "9", "0"));
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        var transcript = output.ToString();
        StringAssert.Contains(transcript, "Operação inválida.");
        StringAssert.Contains(transcript, "Programa encerrado.");
        Assert.AreEqual(2, CountOccurrences(transcript, "Calculadora"));
        Assert.IsFalse(transcript.Contains("primeiro número", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(transcript.Contains("segundo número", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void Run_EndOfInputAtMenu_StopsAfterSingleReadWithoutResult()
    {
        using var input = new CountingTextReader();
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        Assert.AreEqual(1, input.ReadCount);
        AssertNoPartialResultOrExitConfirmation(output.ToString());
    }

    [TestMethod]
    public void Run_EndOfInputAtFirstOperand_StopsWithoutPartialCalculation()
    {
        using var input = new CountingTextReader("1");
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        Assert.AreEqual(2, input.ReadCount);
        AssertNoPartialResultOrExitConfirmation(output.ToString());
    }

    [TestMethod]
    public void Run_EndOfInputAtSecondOperand_StopsWithoutPartialCalculation()
    {
        using var input = new CountingTextReader("1", "2");
        using var output = new StringWriter(CultureInfo.GetCultureInfo("pt-BR"));
        var session = new ConsoleSession(input, output);

        session.Run();

        Assert.AreEqual(3, input.ReadCount);
        AssertNoPartialResultOrExitConfirmation(output.ToString());
    }

    private static void AssertNoPartialResultOrExitConfirmation(string transcript)
    {
        Assert.IsFalse(transcript.Contains("Resultado:", StringComparison.Ordinal));
        Assert.IsFalse(transcript.Contains("Programa encerrado.", StringComparison.Ordinal));
    }

    private sealed class CountingTextReader(params string[] lines) : TextReader
    {
        private readonly Queue<string> _lines = new(lines);

        public int ReadCount { get; private set; }

        public override string? ReadLine()
        {
            ReadCount++;
            return _lines.TryDequeue(out var line) ? line : null;
        }
    }

    private static int CountOccurrences(string text, string value) =>
        (text.Length - text.Replace(value, string.Empty, StringComparison.Ordinal).Length) / value.Length;
}
