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
        while (true)
        {
            WriteMenu();
            _output.Write("Escolha uma opção: ");
            var choice = _input.ReadLine()?.Trim();

            if (choice is null)
            {
                return;
            }

            if (choice == "0")
            {
                _output.WriteLine("Programa encerrado.");
                return;
            }

            if (!TryGetOperation(choice, out var operation))
            {
                _output.WriteLine("Operação inválida. Escolha uma das opções do menu.");
                continue;
            }

            if (!TryReadOperand("Informe o primeiro número: ", out var first))
            {
                return;
            }

            var secondPrompt = operation == Operation.Division
                ? "Informe o segundo número (divisor): "
                : "Informe o segundo número: ";

            var restartMenu = false;
            while (TryReadOperand(secondPrompt, out var second))
            {
                var result = _engine.Calculate(operation, first, second);
                if (result.Error == CalculationError.DivisionByZero)
                {
                    _output.WriteLine("Não é possível dividir por zero. Informe outro divisor.");
                    continue;
                }

                if (result.Error == CalculationError.ResultOutOfRange)
                {
                    _output.WriteLine("O resultado excede o intervalo permitido. Escolha outra operação.");
                    restartMenu = true;
                    break;
                }

                if (result.IsSuccess && result.Value is decimal value)
                {
                    _output.WriteLine($"Resultado: {ResultFormatter.Format(value)}");
                }

                return;
            }

            if (restartMenu)
            {
                continue;
            }

            return;
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

    private bool TryReadOperand(string prompt, out decimal value)
    {
        while (true)
        {
            _output.Write(prompt);
            var input = _input.ReadLine();
            if (input is null)
            {
                value = 0m;
                return false;
            }

            if (NumberParser.TryParse(input, out value, out var error))
            {
                return true;
            }

            WriteInputError(error);
        }
    }

    private void WriteInputError(InputError error)
    {
        var message = error switch
        {
            InputError.InvalidFormat => "Número inválido. Use vírgula decimal e informe novamente este número.",
            InputError.PrecisionExceeded => "Use até seis casas decimais, desconsiderando zeros finais.",
            InputError.OutOfRange => "Informe um número entre -1000000000 e 1000000000.",
            _ => string.Empty
        };

        if (message.Length > 0)
        {
            _output.WriteLine(message);
        }
    }
}
