# Pesquisa técnica: Calculadora interativa de console

**Data**: 2026-10-02 | **Base**: [spec.md](spec.md) e constituição v1.0.0.

## Representação numérica

**Decisão**: Usar `decimal` em todo o caminho numérico, sem conversão intermediária para `double`.

**Justificativa**: A representação decimal é adequada às entradas limitadas a seis casas e evita
surpresas binárias em exemplos como 0,1 + 0,2. A documentação descreve precisão de 28–29 dígitos;
a divisão periódica continua sendo uma aproximação finita, não uma fração exata.
Fonte: [Tipos numéricos de C#](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types).

**Análise dos limites (inferência do projeto)**: Operandos admitidos são múltiplos de 0,000001,
com magnitude até um bilhão. Soma/subtração são exatas; um produto dentro do intervalo tem
no máximo dez posições inteiras e doze fracionárias e cabe exatamente em `decimal`.
Produtos enormes podem sofrer redução de precisão interna, mas continuam muito além do limite
funcional e são recusados. O menor divisor não nulo é 0,000001, portanto o maior quociente bruto
é 10 elevado a 15, bem abaixo do limite do tipo. A precisão disponível é suficiente para a saída
prescrita; incluir testes de empate e fronteira, sem afirmar exatidão de toda divisão.

**Decisão de fronteira**: Antes da divisão, após recusar zero, comparar a magnitude do dividendo
com limite vezes magnitude do divisor. Essa multiplicação é exata nesse domínio e permite
recusar um quociente excessivo antes de sua aproximação. Comparar também o resultado bruto das
outras operações antes de formatar. Essa proteção usa somente `decimal` e não introduz racionais.

**Alternativas consideradas**: `double` é simples, mas exige lidar com aproximações binárias;
inteiros escalados exigem tratamento adicional de divisão e produtos; `BigInteger` e tipos
racionais tornam o exercício maior sem necessidade demonstrada pelos limites acordados.

## Validação textual e cultura

**Decisão**: Validar texto e precisão antes de `decimal.TryParse`; converter com cultura `pt-BR`
e `NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint`. Não permitir milhares.

**Justificativa**: O parser pode arredondar números excessivamente precisos. A checagem textual
impede isso, enquanto a cultura explícita torna o comportamento independente do computador.
Zeros finais da fração são removidos antes de contar as casas e converter, sem alterar o valor.
Fontes: [Decimal.TryParse](https://learn.microsoft.com/en-us/dotnet/api/system.decimal.tryparse?view=net-10.0)
e [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles?view=net-10.0).

**Alternativas consideradas**: Cultura do sistema produziria formatos diferentes; `NumberStyles.Number`
aceitaria agrupamento de milhares; substituir vírgula por ponto sem validar aceitaria formatos
proibidos. Apenas converter antes de verificar a precisão perderia a distinção entre entrada
válida e entrada arredondada automaticamente.

## Formatação

**Decisão**: Usar `Math.Round` com seis casas e `MidpointRounding.AwayFromZero`; depois formatar
com `0.######` e cultura `pt-BR`, tratando zero explicitamente.

**Justificativa**: Faz o desempate aprovado ser explícito e elimina zeros finais sem agrupamento.
Fontes: [Math.Round](https://learn.microsoft.com/en-us/dotnet/api/system.math.round?view=net-10.0)
e [Formatos personalizados](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings).

**Alternativas consideradas**: `F6` fixa zeros finais; o arredondamento padrão para o par não
corresponde à decisão aprovada; formatar antes de validar o resultado poderia esconder um erro.

## Estrutura e recuperação

**Decisão**: Um executável com pastas `Domain` e `Presentation`, mais um projeto de testes.
Usar classes concretas e laços de leitura. O domínio retorna um resultado tipado sem escrever
mensagens; apresentação transforma os erros em mensagens e pedidos específicos.

**Justificativa**: Separação de responsabilidades suficiente para ensinar e testar as regras.
`TextReader`/`TextWriter` permitem entrada e saída em memória sem interfaces inventadas.
Fontes: [StringReader](https://learn.microsoft.com/en-us/dotnet/api/system.io.stringreader?view=net-10.0)
e [StringWriter](https://learn.microsoft.com/en-us/dotnet/api/system.io.stringwriter?view=net-10.0).

**Alternativas consideradas**: Misturar cálculo e console dificulta testes; terceiro projeto de
biblioteca, contêiner de DI e mocks não são necessários para este escopo. Recursão para repetir
cálculos acumularia chamadas desnecessariamente.

## Testes e ferramentas

**Decisão**: MSTest com runner VSTest, sem mocks ou cobertura obrigatória. Usar o template
`mstest` do SDK 10 com runner explícito. O alvo da aplicação e dos testes será `net10.0`.

**Justificativa**: MSTest oferece testes automatizados e casos parametrizados; VSTest mantém
`dotnet test` sem configuração adicional de runner. O tutorial oficial apresenta a criação
pelo template e referência ao projeto testado.
Fontes: [MSTest com C#](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-csharp-with-mstest),
[Seleção do runner](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test)
e [Templates do SDK](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates).

**Alternativas consideradas**: xUnit/NUnit também atenderiam, mas não há motivo para acrescentar
comparações de frameworks ao exercício; MSTest.Sdk/MTP são alternativas válidas, porém exigem
alinhar configuração adicional. Remover pacote de cobertura se o template o gerar, pois nenhuma
meta de cobertura foi acordada. Registrar versões exatas dos pacotes na implementação.

## Conclusão da pesquisa

As decisões técnicas foram resolvidas. Nenhuma clarificação adicional é necessária para este
plano. Não foram instaladas dependências nem executados testes de aplicação: a entrega é documental.
