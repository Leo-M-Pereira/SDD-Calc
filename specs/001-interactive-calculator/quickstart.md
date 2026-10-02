# Guia de validação: Calculadora interativa de console

**Estado atual**: Somente planejamento. Os comandos abaixo destinam-se à etapa de implementação
e à validação posterior; não foram executados nesta etapa e os projetos ainda não existem.

## Pré-requisitos

- SDK .NET 10 disponível no terminal; usar `dotnet --info` para conferir.
- Checkout desta branch e implementação dos caminhos previstos em [plan.md](plan.md).
- Acesso ao feed NuGet na primeira restauração dos pacotes de testes.
- Contrato de menu e entrada em [contracts/console.md](contracts/console.md); invariantes e
  transições em [data-model.md](data-model.md).

## Preparação futura dos projetos

Executar na raiz do repositório somente durante a implementação, se os projetos ainda não
existirem. O projeto de testes referencia diretamente o executável. Não é necessária solução
na raiz para os comandos de validação explícitos abaixo.

```sh
dotnet new console --name Calculator --output src/Calculator --framework net10.0
dotnet new mstest --name Calculator.Tests --output tests/Calculator.Tests --framework net10.0 --test-runner VSTest
dotnet reference add src/Calculator/Calculator.csproj --project tests/Calculator.Tests/Calculator.Tests.csproj
```

A implementação deve fixar as versões estáveis dos pacotes, manter VSTest e remover cobertura
adicional se gerada pelo template. A criação dos projetos por si só não implementa os cenários.
Referência oficial: [Templates do SDK](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates).

## Compilação, testes e execução após implementar

```sh
dotnet --info
dotnet restore tests/Calculator.Tests/Calculator.Tests.csproj
dotnet build tests/Calculator.Tests/Calculator.Tests.csproj --no-restore
dotnet test tests/Calculator.Tests/Calculator.Tests.csproj --no-build --no-restore
dotnet run --project src/Calculator/Calculator.csproj --no-build
```

Esperado: compilação sem erros; testes descobertos e executados, todos passando; aplicação
apresentando o menu. Uma suíte vazia ou testes ignorados não comprova as regras de cálculo.
Os testes automatizados devem cobrir a matriz de [plan.md](plan.md), independentemente da
verificação manual abaixo. Referência: [dotnet test](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test).

## Roteiros de aceitação

Cada linha indica uma sequência de entradas, uma por pedido. Ao terminar o cálculo, a próxima
entrada é novamente uma opção do menu. Não enviar as sequências como uma única linha.

| Roteiro | Entradas em sequência | Resultado esperado |
|---|---|---|
| Adição decimal | `1`; `0,1`; `0,2`; `0` | `Resultado: 0,3`, menu novamente e encerramento |
| Subtração em ordem | `2`; `2`; `5`; `0` | `Resultado: -3` e encerramento |
| Multiplicação negativa | `3`; `-4`; `3`; `0` | `Resultado: -12` e encerramento |
| Divisão periódica | `4`; `1`; `3`; `0` | `Resultado: 0,333333` e encerramento |
| Opção inválida | `9`; `1`; `2`; `3`; `0` | Mensagem de operação inválida, menu, depois resultado 5 |
| Primeiro número inválido | `1`; `abc`; `2`; `3`; `0` | Repetir primeiro número, manter adição, resultado 5 |
| Segundo número inválido | `1`; `2`; `abc`; `3`; `0` | Repetir segundo número, preservar primeiro 2, resultado 5 |
| Divisor zero | `4`; `5`; `0`; `2`; `0` | Erro de divisão por zero, repetir divisor, resultado 2,5 |
| Entrada excessiva | `1`; `1000000000,000001`; `2`; `3`; `0` | Erro de intervalo, repetir primeiro número, resultado 5 |
| Precisão excessiva | `1`; `0,1234567`; `0,1234560`; `0`; `0` | Erro de precisão, repetir operando, resultado 0,123456 |
| Resultado excessivo | `1`; `1000000000`; `0,000001`; `2`; `5`; `2`; `0` | Erro de resultado, menu, novo cálculo sem dados antigos, resultado 3 |
| Saída imediata | `0` | Confirmação de encerramento sem pedir operandos |
| Encerramento após opção inválida | `9`; `0` | Erro e menu reapresentado após `9`; confirmação de encerramento após `0`, sem operandos ou novas entradas |

## Validações adicionais

- Repetir dez cálculos em uma única execução, alternando operações; conferir SC-003.
- Executar os casos de empate positivo/negativo, zero sem sinal e fronteiras do contrato.
- Informar ponto decimal, milhares, notação científica e espaços internos; verificar rejeição.
- Verificar que espaços externos e zeros finais excedentes são aceitos conforme o contrato.
- Confirmar que cada erro mantém o ponto de retomada de FR-014, inclusive erros consecutivos.
- Executar com ambiente de cultura diferente; entrada e saída continuam usando vírgula.

## Critério de conclusão posterior

Revisar todos os critérios SC-001 a SC-006 e os cenários da especificação, registrar evidências
e corrigir falhas antes de concluir a implementação. O presente guia não afirma que esses
critérios já foram executados ou satisfeitos pela aplicação.
