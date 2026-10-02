# Plano de implementação: Calculadora interativa de console

**Branch**: `001-interactive-calculator` | **Data**: 2026-10-02 | **Especificação**: [spec.md](spec.md)

**Entrada**: Especificação esclarecida em `specs/001-interactive-calculator/spec.md`.

## Resumo

Planejar quatro operações sobre dois números, mensagens de erro, recuperação específica por erro,
repetição de cálculos e saída voluntária. Usar C#/.NET 10 e `decimal`, com domínio sem acesso ao
console e apresentação responsável por texto, conversão e fluxo. A pesquisa está em
[research.md](research.md); as regras observáveis estão em [contracts/console.md](contracts/console.md).
Esta entrega contém somente documentos; os caminhos de código abaixo são uma proposta futura.

## Contexto técnico

**Linguagem/versão**: C# com a versão padrão do SDK .NET 10; alvo `net10.0`.

**Dependências principais**: Biblioteca padrão na aplicação. Nos testes, MSTest com VSTest,
`Microsoft.NET.Test.Sdk`, `MSTest.TestFramework` e `MSTest.TestAdapter`. Fixar versões estáveis
compatíveis no arquivo do projeto quando ele for criado; não usar versões flutuantes. Se o template
agrupar essas dependências no pacote `MSTest`, manter a configuração equivalente sem duplicá-las.
Sem biblioteca de mocks, cobertura obrigatória ou pacotes em produção.

**Armazenamento**: Somente variáveis da tentativa atual; sem persistência.

**Testes**: MSTest, executados com `dotnet test` apontando explicitamente ao projeto de testes.

**Plataforma-alvo**: Terminal local em sistemas com SDK/runtime .NET 10, sem necessidade de rede
para os cálculos. Restauração inicial dos pacotes de testes requer acesso ao feed de pacotes.

**Tipo de projeto**: Aplicação de console interativa, um projeto de produção e um de testes.

**Objetivos de desempenho**: Operações escalares síncronas para um usuário; validar a sequência
SC-003 de dez cálculos. A especificação não impõe meta de latência; não acrescentar benchmark.

**Restrições**: Entrada e resultado bruto entre -1000000000 e 1000000000 inclusive; entradas
com até seis casas após a vírgula, ignorando zeros finais. Saída com até seis casas, empate para
longe de zero. Sem serviço, banco de dados, contêiner, injeção de dependência ou framework de CLI.

**Escala/escopo**: Um usuário, quatro operações, dois operandos, uma sessão; sem histórico,
expressões encadeadas ou reutilização automática dos operandos após resultado válido.

## Verificação da constituição

Verificação realizada antes da pesquisa e repetida após o desenho da fase 1.

| Princípio | Evidência no desenho | Antes | Depois |
|---|---|---|---|
| I. Simplicidade didática | Dois projetos, classes concretas pequenas, sem infraestrutura adicional | Conforme | Conforme |
| II. Código em inglês | Nomes propostos `Calculator`, `NumberParser`, `ConsoleSession` e testes em inglês | Conforme | Conforme |
| III. Documentação em português | Documentos e mensagens em português do Brasil | Conforme | Conforme |
| IV. Testes das regras | Matriz abaixo cobre operações, limites, arredondamento e regressões | Conforme | Conforme |
| V. Separação do console | Domínio puro; leitura e escrita somente na apresentação | Conforme | Conforme |
| VI. Entradas inválidas | Validação em etapas e transições de recuperação documentadas | Conforme | Conforme |
| VII. Dependências justificadas | Somente plataforma em produção; MSTest atende aos testes obrigatórios | Conforme | Conforme |
| VIII. Etapas revisáveis no Git | Planejamento limitado a documentos; commit posterior à revisão, não realizado agora | Conforme | Conforme |

Resultado: nenhum desvio. A implementação dependerá de checklist, tarefas e análise posteriores.

## Estrutura do projeto

### Documentação desta funcionalidade

```text
specs/001-interactive-calculator/
├── spec.md
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── console.md
└── checklists/
    └── requirements.md
```

`tasks.md` será produzido somente na etapa `$speckit-tasks`.

### Código-fonte na raiz — estrutura futura

```text
src/Calculator/
├── Calculator.csproj
├── Program.cs
├── Domain/
│   ├── Operation.cs
│   ├── NumberPolicy.cs
│   ├── CalculatorEngine.cs
│   └── CalculationResult.cs
└── Presentation/
    ├── NumberParser.cs
    ├── InputError.cs
    ├── ResultFormatter.cs
    └── ConsoleSession.cs

tests/Calculator.Tests/
├── Calculator.Tests.csproj
├── CalculatorEngineTests.cs
├── NumberParserTests.cs
├── ResultFormatterTests.cs
└── ConsoleSessionTests.cs
```

**Decisão estrutural**: Separação por responsabilidade dentro de um projeto de produção é
suficiente. O projeto de testes referencia o executável sem executar seu ponto de entrada.
`Program` apenas conecta `Console.In`, `Console.Out` e a sessão. O domínio não referencia
`Presentation` nem `Console`; recebe uma operação e números e retorna resultado ou erro.
Os componentes necessários aos testes são acessíveis no assembly, sem uma API de extensões.
Uma biblioteca de domínio separada não agrega valor neste escopo.

### Representação, validação e cálculo

- `NumberPolicy` centraliza limite 1000000000 e precisão de entrada 6; valida operandos também
  nas chamadas diretas ao domínio. Validar precisão numérica comparando o valor a sua truncagem
  em seis casas, sem substituir nem arredondar o operando original.
- `NumberParser` faz `Trim`, valida a gramática do contrato, remove zeros fracionários finais
  e valida a precisão textual antes da conversão. Remover zeros iniciais redundantes da parte
  inteira evita falhas por comprimento textual sem mudar o valor. Depois converter com
  `decimal.TryParse`, cultura `pt-BR` explícita e apenas sinal inicial e separador decimal.
  Uma entrada sintaticamente numérica grande demais é erro de intervalo, não erro de formato.
- `CalculatorEngine` valida operação e operandos, verifica divisor zero e executa as operações.
  Para divisão, comparar `abs(first) > 1000000000 * abs(second)` antes de dividir garante a
  fronteira do resultado sem depender de uma aproximação do quociente. Após calcular, validar
  o resultado bruto; não formatar nem arredondar no domínio.
- `CalculationResult` distingue sucesso de erros esperados. Exceções não são o fluxo usual de
  entrada inválida ou divisão por zero. Um `OverflowException` aritmético, se ocorrer de forma
  defensiva, é convertido em erro de limite de resultado; não usar captura genérica de exceções.
- `ResultFormatter` arredonda o `decimal` em seis casas com `MidpointRounding.AwayFromZero`,
  normaliza zero e apresenta com `0.######` e `pt-BR`. Não arredondar durante o cálculo.

### Recuperação de erros e interação

Implementar laços locais para escolha, primeiro operando e segundo operando; não usar recursão.
`ConsoleSession` recebe `TextReader` e `TextWriter`, tipos da plataforma, permitindo testes sem
alterar o estado global de `Console`. Não criar interface de console própria.

| Evento | Próximo pedido | Dados preservados |
|---|---|---|
| Operação inválida | Escolha de operação | Nenhum operando |
| Primeiro número inválido, excessivo ou impreciso | Primeiro número | Operação |
| Segundo número inválido, excessivo ou impreciso | Segundo número | Operação e primeiro número |
| Divisão por zero | Segundo número/divisor | Divisão e primeiro número |
| Resultado fora do intervalo | Escolha de operação | Nenhum dado da tentativa |
| Resultado válido | Escolha de operação | Nenhum dado reutilizado automaticamente |
| Encerrar no menu | Nenhum pedido | Sessão encerrada |

Fim da entrada no menu, no primeiro operando ou no segundo operando não será tratado como
entrada inválida em laço infinito: encerrar a sessão assim que a leitura retornar EOF, sem
tentar calcular com dados ausentes nem apresentar resultado parcial ou indevido. É um cuidado
técnico de leitura; o cenário de saída voluntária segue sendo exclusivamente a opção do menu,
conforme a especificação.

### Estratégia de testes e rastreabilidade

| Grupo | Verificações necessárias | Requisitos/critérios |
|---|---|---|
| Cálculo puro | Quatro operações, ordem, negativos, zero, frações exatas e periódicas | FR-002/003/007; SC-001 |
| Domínio e fronteiras | Extremos inclusivos e valores externos para adição, subtração, multiplicação e divisão; na divisão, confirmar rejeição pela magnitude antes de executar a divisão; precisão, entradas inválidas diretas e divisor zero | FR-007/013; SC-001/002 |
| Parsing | Vírgula, espaços externos, zeros finais, sintaxe proibida, seis versus sete casas, texto numérico enorme | FR-006/012/013; SC-002 |
| Formatação | `0,3`, `3,5`, `0,333333`, empates dos dois sinais, zeros finais e zero sem sinal | FR-004/013; SC-001/006 |
| Sessão em memória | Erros repetidos, preservação, retorno ao menu, dez cálculos, saídas nos momentos previstos e EOF no menu, primeiro operando e segundo operando, sem laço infinito ou resultado parcial | FR-001/005/006/008/009/010/011/014; SC-002 a SC-006 |

Testes devem verificar valores esperados independentes da implementação, mensagens relevantes
e ordem das solicitações; evitar snapshots completos de transcrições ou testes que copiem o
algoritmo. Usar tabelas de casos para variar operandos. Decimal esperado é comparado diretamente
quando exato; quocientes periódicos são verificados pela saída prescrita e pelo resultado bruto
sem truncagem em seis casas. Testar cultura explícita sem alterar a cultura global da suíte.
Correções de cálculo exigirão regressão automatizada conforme a constituição.

## Acompanhamento de complexidade

Nenhuma violação constitucional ou abstração excepcional foi identificada. Não há justificativas
de complexidade adicionais. Decisões, alternativas e fontes estão em [research.md](research.md).
