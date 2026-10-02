# Modelo de dados e estados: Calculadora interativa

**Base**: [spec.md](spec.md) | **Contrato**: [contracts/console.md](contracts/console.md).

## Operação — `Operation`

Enumeração `Addition`, `Subtraction`, `Multiplication`, `Division`. Cada tentativa tem exatamente
uma operação; a opção de encerrar pertence ao menu, não é operação de cálculo. Valores de enumeração
não definidos são recusados defensivamente pelo domínio.

## Operandos — `first` e `second`

Dois valores `decimal`, mantidos em variáveis locais; não precisam de classe própria. Intervalo
inclusivo ±1000000000 e até seis casas efetivas após a vírgula. O primeiro operando é preservado
quando o segundo é inválido ou zero na divisão. A conversão textual pertence à apresentação;
a regra de intervalo/precisão numérica pertence a `NumberPolicy` e também é verificada pelo motor.

## Resultado — `CalculationResult`

| Campo | Representação proposta | Regra |
|---|---|---|
| `Value` | `decimal?` | Presente somente em sucesso; ainda não arredondado para apresentação |
| `Error` | `CalculationError` | `None` em sucesso; erro específico nas falhas |

`CalculationError` terá `None`, `InvalidOperation`, `InvalidOperand`, `DivisionByZero` e
`ResultOutOfRange`. Definir a enumeração no mesmo arquivo do resultado evita arquivos sem
necessidade. Sucesso implica valor presente e erro `None`; falha implica valor ausente e erro
não `None`. O motor não devolve um número de fallback, evitando confundir erro com zero válido.
Erros de operação/operando no motor são defesa de chamadas diretas; a sessão evita esses casos
pela validação anterior. São condições específicas deste domínio, sem tipo genérico de resultado.

## Entrada recusada — `InputError`

`InvalidFormat`, `PrecisionExceeded`, `OutOfRange`, além de `None` no parsing bem-sucedido.
Um resultado de parsing pode ser um retorno booleano com valor e erro de saída; não é entidade
persistida. O chamador só usa o valor quando o retorno indica sucesso. A mensagem de cada erro
é escolhida na apresentação, nunca guardada como regra de negócio.

## Tentativa e sessão

Tentativa = operação, primeiro operando e segundo operando. A sessão mantém somente os valores
já aceitos da tentativa atual. Não criar objetos persistentes, identificadores, timestamps,
repositórios nem coleção de histórico. `ConsoleSession` coordena essa evolução.

| Estado conceitual | Evento | Próximo estado | Efeito nos dados |
|---|---|---|---|
| Escolha de operação | Opção inválida | Escolha de operação | Nenhum operando atribuído |
| Escolha de operação | Operação válida | Leitura do primeiro número | Seleciona operação |
| Escolha de operação | Encerrar | Encerrado | Nenhuma nova leitura |
| Leitura do primeiro número | Formato, precisão ou intervalo inválido | Leitura do primeiro número | Preserva operação |
| Leitura do primeiro número | Número válido | Leitura do segundo número | Preserva primeiro número |
| Leitura do segundo número | Formato, precisão ou intervalo inválido | Leitura do segundo número | Preserva operação e primeiro número |
| Leitura do segundo número | Zero como divisor | Leitura do segundo número | Preserva divisão e primeiro número |
| Leitura do segundo número | Número válido | Cálculo | Usa os dois operandos atuais |
| Cálculo | Resultado excessivo | Escolha de operação | Descarta toda a tentativa |
| Cálculo | Resultado válido | Escolha de operação | Exibe resultado e descarta toda a tentativa |

Os estados descrevem o comportamento para revisão e testes; não exigem enumeração de estados
nem framework de máquina de estados. Laços locais implementam as mesmas transições.
Fim do fluxo de entrada interrompe as leituras sem cálculo parcial, como proteção técnica.

## Invariantes

- O motor é independente de I/O, mensagens e cultura de apresentação.
- Divisor zero é verificado antes de dividir, incluindo zero dividido por zero.
- Limite do resultado é verificado antes do arredondamento de apresentação.
- Depois de um resultado válido ou excessivo, os valores antigos não participam do próximo cálculo.
- Não há resultado numérico em erro; zero pode ser um resultado válido.
- Nenhum dado é persistido ou enviado para outro sistema.
