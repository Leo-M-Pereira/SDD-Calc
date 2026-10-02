using System.IO;
using Calculator.Domain;

namespace Calculator.Presentation;

public sealed class ConsoleSession
{
    private readonly TextReader _input;
    private readonly TextWriter _output;
    private readonly CalculatorEngine _engine = new();

    public ConsoleSession(TextReader input, TextWriter output)
    {
        _input = input;
        _output = output;
    }

    public void Run()
    {
        WriteMenu();
        _output.Write("Escolha uma opção: ");
        var choice = _input.ReadLine()?.Trim();

        if (choice == "0")
        {
            _output.WriteLine("Programa encerrado.");
            return;
        }

        if (!TryGetOperation(choice, out var operation))
        {
            return;
        }

        _output.Write("Informe o primeiro número: ");
        if (!TryReadNumber(out var first))
        {
            return;
        }

        _output.Write("Informe o segundo número: ");
        if (!TryReadNumber(out var second))
        {
            return;
        }

        var result = _engine.Calculate(operation, first, second);
        if (result.IsSuccess && result.Value is decimal value)
        {
            _output.WriteLine($"Resultado: {ResultFormatter.Format(value)}");
        }
    }

    private void WriteMenu()
    {
        _output.WriteLine("Calculadora");
        _output.WriteLine("1 - Adição");
        _output.WriteLine("2 - Subtração");
        _output.WriteLine("3 - Multiplicação");
        _output.WriteLine("4 - Divisão");
        _output.WriteLine("0 - Encerrar");
    }

    private static bool TryGetOperation(string? choice, out Operation operation)
    {
        operation = choice switch
        {
            "1" => Operation.Addition,
            "2" => Operation.Subtraction,
            "3" => Operation.Multiplication,
            "4" => Operation.Division,
            _ => default
        };

        return choice is "1" or "2" or "3" or "4";
    }

    private bool TryReadNumber(out decimal value)
    {
        var input = _input.ReadLine();
        return NumberParser.TryParse(input, out value, out _);
    }
}
