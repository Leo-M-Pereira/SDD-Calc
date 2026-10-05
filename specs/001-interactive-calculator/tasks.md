# Tarefas: Calculadora interativa de console

**Branch**: `001-interactive-calculator`
**Entrada**: `specs/001-interactive-calculator/spec.md`

**Pré-requisitos**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/console.md` e `quickstart.md` estão disponíveis. Os checklists de requisitos e de qualidade foram revisados antes desta decomposição.

**Testes**: Incluídos porque a constituição exige testes automatizados para as regras de cálculo e o usuário pediu explicitamente tarefas de implementação e testes.

**Organização**: Fases por história do usuário; testes são planejados antes da implementação correspondente. `[P]` indica tarefas em arquivos distintos sem dependência entre si.

## Fase 1: Preparação do projeto

**Objetivo**: Criar, durante a implementação, a aplicação e o projeto de testes conforme o plano.

- [X] T001 [P] Criar o projeto de console C# com alvo `net10.0` e ponto de entrada inicial em `src/Calculator/Calculator.csproj` e `src/Calculator/Program.cs`.
- [X] T002 [P] Criar o projeto MSTest para `net10.0` usando VSTest em `tests/Calculator.Tests/Calculator.Tests.csproj` e `tests/Calculator.Tests/Test1.cs`.
- [X] T003 Adicionar referência do projeto de testes ao executável e remover o arquivo de exemplo em `tests/Calculator.Tests/Calculator.Tests.csproj` e `tests/Calculator.Tests/Test1.cs`.

## Fase 2: Tipos fundamentais

**Objetivo**: Definir os tipos de domínio compartilhados antes das histórias.

- [X] T004 [P] Definir os valores `Addition`, `Subtraction`, `Multiplication` e `Division` em `src/Calculator/Domain/Operation.cs`.
- [X] T005 [P] Definir `CalculationResult` e `CalculationError` (`None`, `InvalidOperation`, `InvalidOperand`, `DivisionByZero`, `ResultOutOfRange`) com valor presente somente em sucesso em `src/Calculator/Domain/CalculationResult.cs`.

**Marco**: Concluir T001–T005 antes das histórias. Não adicionar estrutura de infraestrutura sem necessidade demonstrada.

## Fase 3: História do usuário 1 — Realizar um cálculo (Prioridade: P1) 🎯 MVP

**Objetivo**: Calcular adição, subtração, multiplicação e divisão para dois operandos válidos e apresentar a saída acordada.

**Teste independente**: Cobrir as quatro operações, a ordem dos operandos, negativos, zero válido, decimais aceitos, limites inclusivos, seis casas de entrada e a apresentação de seis casas com vírgula.

### Testes da história 1

- [X] T006 [P] [US1] Escrever testes parametrizados das quatro operações, ordem, negativos, zero, resultados inteiros/decimais e operandos nos extremos inclusivos em `tests/Calculator.Tests/CalculatorEngineTests.cs`.
- [X] T007 [P] [US1] Escrever testes de parsing para inteiros e decimais com vírgula, sinais, espaços externos, zeros finais e operandos válidos nos limites em `tests/Calculator.Tests/NumberParserTests.cs`.
- [X] T008 [P] [US1] Escrever testes de apresentação para até seis casas, desempate positivo e negativo para longe de zero, remoção de zeros finais e normalização de zero em `tests/Calculator.Tests/ResultFormatterTests.cs`.
- [X] T009 [P] [US1] Escrever testes da interação de um cálculo válido por operação, opções exibidas e encerramento antes do primeiro cálculo em `tests/Calculator.Tests/ConsoleSessionTests.cs`.

### Implementação da história 1

- [X] T010 [US1] Implementar `NumberPolicy` para validar operandos entre -1000000000 e 1000000000 inclusive e até seis casas decimais efetivas sem arredondar em `src/Calculator/Domain/NumberPolicy.cs`.
- [X] T011 [P] [US1] Implementar parsing da gramática do contrato, `Trim`, zeros fracionários finais, cultura `pt-BR` explícita e classificação de formato, precisão e intervalo em `src/Calculator/Presentation/NumberParser.cs` e `src/Calculator/Presentation/InputError.cs`.
- [X] T012 [P] [US1] Implementar cálculo das quatro operações sobre `decimal`, preservando operandos originais e retornando resultado tipado em `src/Calculator/Domain/CalculatorEngine.cs`.
- [X] T013 [P] [US1] Implementar formatação `pt-BR` com seis casas máximas, arredondamento `AwayFromZero`, sem zeros finais e zero sem sinal em `src/Calculator/Presentation/ResultFormatter.cs`.
- [X] T014 [US1] Implementar a interação inicial para apresentar o menu, ler operação e dois operandos, exibir um resultado e aceitar encerramento no menu, conectando leitura/escrita em `src/Calculator/Presentation/ConsoleSession.cs` e `src/Calculator/Program.cs`.

**Marco**: A história 1 oferece um cálculo válido e tem testes automatizados independentes.

- [X] T024 Validar o incremento da história 1 antes do respectivo commit: compilar o projeto de produção e executar todos os testes existentes, corrigindo falhas em `src/Calculator/Calculator.csproj`, `tests/Calculator.Tests/Calculator.Tests.csproj` e nos arquivos afetados.

## Fase 4: História do usuário 2 — Entender e corrigir erros (Prioridade: P1)

**Objetivo**: Explicar entradas e operações recusadas e recuperar o ponto correto sem perder dados válidos nem encerrar a sessão inesperadamente.

**Dependência**: Requer a interação e os cálculos válidos da história 1.

**Teste independente**: Cobrir cada categoria de erro, sua mensagem, o próximo pedido, preservação de operandos, divisão por zero, fronteiras do resultado e conclusão de novo cálculo após a recuperação.

### Testes da história 2

- [X] T015 [P] [US2] Acrescentar testes para operação inválida, operando inválido no domínio, divisor zero e resultados brutos fora do intervalo: adição/subtração nos limites inclusivos e valores externos, multiplicação de ±1000000000 por ±1 nos limites e por ±1,000001 fora, divisão de ±1000000000 por ±1 nos limites e por ±0,999999 fora; em `tests/Calculator.Tests/CalculatorEngineTests.cs`.
- [X] T016 [P] [US2] Acrescentar testes para texto vazio/malformado, formatos proibidos, precisão excedida, limite de entrada e precedência de classificação em `tests/Calculator.Tests/NumberParserTests.cs`.
- [X] T017 [P] [US2] Acrescentar testes de sessão para menu repetido após operação inválida, repetição somente do operando inválido, preservação do operando válido, repetição do divisor e retorno ao menu após resultado excessivo em `tests/Calculator.Tests/ConsoleSessionTests.cs`.

### Implementação da história 2

- [X] T018 [P] [US2] Implementar erros específicos para operação/operandos inválidos, divisão por zero e resultado fora do intervalo bruto; após rejeitar divisor zero, na divisão verificar `abs(first) > 1000000000 * abs(second)` antes de executar `/`, aceitar a igualdade no limite e validar o resultado bruto antes do arredondamento em `src/Calculator/Domain/CalculatorEngine.cs`.
- [X] T019 [P] [US2] Mapear erros de parsing e domínio para mensagens em português e aplicar recuperação aprovada: repetir menu, repetir somente o operando, repetir divisor ou descartar tentativa e voltar ao menu em `src/Calculator/Presentation/NumberParser.cs` e `src/Calculator/Presentation/ConsoleSession.cs`.

**Marco**: Todos os erros previstos orientam o próximo pedido, preservam dados conforme FR-014 e permitem continuar na mesma sessão.

- [X] T025 Validar o incremento da história 2 antes do respectivo commit: compilar o projeto de produção e executar todos os testes existentes, incluindo os casos de limite das quatro operações, corrigindo falhas em `src/Calculator/Calculator.csproj`, `tests/Calculator.Tests/Calculator.Tests.csproj` e nos arquivos afetados.

## Fase 5: História do usuário 3 — Repetir cálculos e encerrar (Prioridade: P2)

**Objetivo**: Concluir vários cálculos na mesma sessão e encerrar com confirmação antes do primeiro cálculo, após um cálculo ou depois de uma opção inválida.

**Dependência**: Requer o menu e tratamento de opção inválida da história 2.

**Teste independente**: Realizar dez cálculos válidos alternados sem reutilizar dados e cobrir saída imediata, saída após resultado e a sequência opção `9` inválida seguida de `0` para encerrar sem pedidos de operandos.

### Testes da história 3

- [ ] T020 [US3] Acrescentar testes para dois ou mais cálculos consecutivos, ausência de reutilização de operandos, dez cálculos, confirmação de saída e sequência `9` → erro/menu → `0` → encerramento sem novas entradas; testar EOF no menu, no primeiro operando e no segundo operando, confirmando encerramento sem laço infinito, cálculo parcial ou resultado numérico em `tests/Calculator.Tests/ConsoleSessionTests.cs`.

### Implementação da história 3

- [ ] T021 [US3] Estender a sessão com laço de novos cálculos, limpar dados após cada resultado e encerrar imediatamente quando a leitura retornar EOF no menu ou em qualquer operando, sem repetir a leitura em laço infinito, calcular com dados ausentes ou exibir resultado indevido; preservar o descarte de falhas e o encerramento sem leituras posteriores em `src/Calculator/Presentation/ConsoleSession.cs` e `src/Calculator/Program.cs`.

**Marco**: A sessão permanece ativa entre cálculos e termina somente pela escolha de encerramento ou fim defensivo do fluxo de entrada.

- [ ] T026 Validar o incremento da história 3 antes do respectivo commit: compilar o projeto de produção e executar todos os testes existentes, incluindo os três cenários de EOF, corrigindo falhas em `src/Calculator/Calculator.csproj`, `tests/Calculator.Tests/Calculator.Tests.csproj` e nos arquivos afetados.

## Fase 6: Revisão final

**Objetivo**: Confirmar a entrega contra a especificação, os documentos de projeto e a constituição.

- [ ] T022 Construir e executar a suíte automatizada após restaurar dependências, corrigindo falhas em `tests/Calculator.Tests/Calculator.Tests.csproj`, `src/Calculator/Calculator.csproj` e nos arquivos de origem e teste afetados.
- [ ] T023 Percorrer os roteiros de aceitação e os comandos de validação do guia; corrigir divergências na implementação ou no próprio guia em `specs/001-interactive-calculator/quickstart.md`, `src/Calculator/Presentation/ConsoleSession.cs` e `tests/Calculator.Tests/ConsoleSessionTests.cs`.

## Dependências e ordem de execução

### Dependências entre fases

- **Preparação (Fase 1)**: T001 e T002 podem iniciar juntos; T003 depende de ambos.
- **Tipos fundamentais (Fase 2)**: T004 e T005 dependem da preparação; podem ser feitos em paralelo.
- **História 1 (Fase 3)**: os testes T006–T009 dependem da Fase 2 e podem ser escritos em paralelo. A implementação começa após esses testes; T010 precede T011–T013. T011, T012 e T013 alteram arquivos distintos e podem seguir em paralelo após T010. T014 depende do parser, motor e formatador; T024 depende de T014 e de T006–T009 e deve passar antes do commit da história 1.
- **História 2 (Fase 4)**: depende da história 1 e do gate aprovado T024. T015–T017 são testes em arquivos distintos e podem ser escritos em paralelo. Depois deles, T018 e T019 alteram arquivos distintos e podem ser implementados em paralelo; T025 depende de T018, T019 e T015–T017 e deve passar antes do commit da história 2.
- **História 3 (Fase 5)**: depende da história 2 e do gate aprovado T025 para cobrir a opção inválida seguida de saída. T020 deve preceder T021; T026 depende de T021 e de todos os testes existentes e deve passar antes do commit da história 3.
- **Revisão final (Fase 6)**: T022 continua obrigatório após as três histórias e repete a compilação e todos os testes como validação final; T023 depende de compilação e testes aprovados em T022.

### Oportunidades de paralelismo

- Na preparação: **T001 ∥ T002**.
- Nos tipos fundamentais: **T004 ∥ T005**.
- Nos testes iniciais: **T006 ∥ T007 ∥ T008 ∥ T009**.
- Após `NumberPolicy`: **T011 ∥ T012 ∥ T013**.
- Nos testes de erro: **T015 ∥ T016 ∥ T017**.
- Após os testes de erro: **T018 ∥ T019**.

Tarefas marcadas `[P]` usam arquivos diferentes e não dependem de outra tarefa incompleta. As demais compartilham arquivos ou dependem de resultados anteriores.

## Estratégia de implementação

### MVP

1. Concluir preparação e tipos fundamentais.
2. Completar a história 1 para demonstrar cada cálculo válido, números acordados e formatação.
3. Parar no marco da história 1 para revisar e validar esse incremento antes das histórias seguintes.

### Entrega incremental

1. Adicionar a história 2 e validar mensagens, limites e recuperação sem regressões nos cálculos válidos.
2. Adicionar a história 3 e validar repetição, limpeza de operandos e todas as saídas.
3. Ao concluir cada história, executar compilação e todos os testes existentes antes do respectivo commit (T024, T025 e T026).
4. Após as histórias, manter a validação final de compilação e suíte automatizada (T022) e percorrer o guia de aceitação (T023).

Cada incremento deve permanecer pequeno, passar por revisão de conformidade e ser registrado em commit próprio ou grupo lógico, conforme o princípio VIII. Esta lista planeja o trabalho; não registra execução.
