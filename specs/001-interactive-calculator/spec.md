# Especificação da funcionalidade: Calculadora interativa de console

**Branch atual**: `001-interactive-calculator`

**Criado em**: 2026-10-02

**Status**: Esclarecida — decisões aprovadas incorporadas aos critérios de aceitação

**Entrada**: Calculadora interativa de console que permite escolher adição, subtração,
multiplicação ou divisão; informar dois números, incluindo decimais e negativos; visualizar
resultados; receber mensagens claras para entradas inválidas e divisão por zero; realizar
novos cálculos sem reiniciar; e escolher quando encerrar.

## Clarifications

### Session 2026-10-02

- Q: Quais formatos de números a calculadora deve aceitar e qual separador decimal deve usar nos resultados? → A: Somente vírgula decimal na entrada e na saída; inteiros e negativos aceitos; espaços nas extremidades ignorados; espaços internos, separadores de milhares e notação científica rejeitados (opção A).

- Q: Quantas casas decimais o resultado deve exibir e como deve ser arredondado? → A: Até seis casas decimais, com arredondamento somente na apresentação; empates arredondados para longe de zero; zeros finais omitidos e inteiros sem casas decimais (opção B).

- Q: Quais limites a calculadora deve aceitar para os números informados e os resultados antes do arredondamento? → A: Intervalo inclusivo de -1000000000 a 1000000000 para entradas e resultados antes do arredondamento; entradas com até seis casas decimais após a vírgula, desconsiderando zeros finais; entradas excessivas rejeitadas sem arredondamento; resultado fora do intervalo gera mensagem específica de limite excedido, sem resultado numérico e sem encerrar a sessão (opção A).

- Q: Após uma entrada inválida, divisão por zero ou limite numérico excedido, de onde a interação deve recomeçar e quais dados devem ser preservados? → A: Operação inválida repete a escolha; número inválido repete somente esse operando, preservando os dados válidos; divisão por zero repete o divisor; resultado fora do limite retorna à escolha de operação e descarta todos os dados da tentativa (opção A).

## Cenários do usuário e testes *(obrigatório)*

### História do usuário 1 — Realizar um cálculo (Prioridade: P1)

Como usuário, quero escolher uma das quatro operações e informar dois números para visualizar
o resultado, inclusive ao trabalhar com valores decimais e negativos.

**Motivo da prioridade**: É o valor central da calculadora.

**Teste independente**: Executar cada operação com pares de números e comparar o resultado
exibido com o resultado matemático esperado, usando vírgula como separador decimal.

**Cenários de aceitação**:

1. **Dado** o início de uma sessão, **quando** o usuário consulta as opções, **então** encontra
   adição, subtração, multiplicação, divisão e a opção de encerrar.
2. **Dada** a adição selecionada, **quando** informa 2 e 3, **então** visualiza o resultado 5.
3. **Dada** a subtração selecionada, **quando** informa 2 e 5 nessa ordem, **então** visualiza -3.
4. **Dada** a multiplicação selecionada, **quando** informa -4 e 3, **então** visualiza -12.
5. **Dada** a divisão selecionada, **quando** informa 7 e 2 nessa ordem, **então** visualiza
   o resultado `3,5`.
6. **Dada** a adição selecionada, **quando** informa `1,5` e `2,25`, **então** visualiza
   o resultado `3,75`.
7. **Dada** a multiplicação selecionada, **quando** informa -2 e -3, **então** visualiza 6.
8. **Dada** uma operação válida com dois números válidos, **quando** o cálculo termina,
   **então** o resultado fica identificado como resultado do cálculo atual.

9. **Dada** a divisão selecionada, **quando** informa 1 e 3, **então** visualiza `0,333333`.
10. **Dada** a divisão selecionada, **quando** informa 2469133 e 2000000, **então** visualiza
    `1,234567`; com o primeiro operando negativo, visualiza `-1,234567`, demonstrando
    o arredondamento de empate para longe de zero.
11. **Dada** a adição selecionada, **quando** informa `1,50` e `2,50`, **então** visualiza `4`,
    sem separador decimal nem zeros finais.
12. **Dada** a divisão selecionada, **quando** informa 1 e 10000000, **então** visualiza `0`,
    sem sinal negativo ou casas decimais; com numerador -1, também visualiza `0`.

13. **Dada** a adição selecionada, **quando** informa `1000000000` e `0`, **então** visualiza
    `1000000000`; com primeiro operando `-1000000000`, visualiza `-1000000000`.
14. **Dada** a adição selecionada, **quando** informa `0,1234560` e `0`, **então** visualiza
    `0,123456`, pois o zero final não conta para o limite de casas decimais da entrada.

### História do usuário 2 — Entender e corrigir erros (Prioridade: P1)

Como usuário, quero entender por que uma entrada ou cálculo foi recusado e poder continuar
usando a calculadora sem um encerramento inesperado.

**Motivo da prioridade**: Entradas inválidas fazem parte do uso interativo e não podem impedir
os próximos cálculos.

**Teste independente**: Informar operação inexistente, texto em lugar de número e divisor zero;
verificar a mensagem correspondente e concluir um cálculo válido depois de cada erro.

**Cenários de aceitação**:

1. **Dada** a escolha de operação, **quando** o usuário informa uma opção não oferecida,
   **então** recebe uma mensagem que identifica a operação inválida e orienta como continuar,
   sem calcular um resultado nem encerrar a sessão.
2. **Dada** a solicitação do primeiro ou segundo número, **quando** informa `abc` ou uma
   entrada vazia, **então** recebe uma mensagem que identifica o número inválido e orienta
   como continuar, sem apresentar um resultado para a tentativa inválida.
3. **Dada** a divisão selecionada, **quando** informa 5 e 0 nessa ordem, **então** recebe
   uma mensagem específica de que não é possível dividir por zero, sem resultado numérico.
4. **Dada** a divisão selecionada, **quando** informa 0 e 0, **então** recebe o mesmo tratamento
   específico de divisão por zero.
5. **Dada** a adição com primeiro operando 2 e uma entrada inválida para o segundo operando,
   **quando** corrige somente o segundo operando para 3, **então** visualiza 5 sem escolher
   novamente a operação nem informar novamente o primeiro operando.
   Para a divisão de 5 por zero, ao corrigir somente o divisor para 2, visualiza `2,5`.

6. **Dada** a solicitação de um número, **quando** informa `  -2,25  `, **então** os espaços
   nas extremidades são ignorados e o valor negativo -2,25 é aceito.
7. **Dada** a solicitação de um número, **quando** informa `1.5`, `1.000,5`, `1 000`,
   `1e3` ou somente espaços, **então** recebe a mensagem de número inválido, sem resultado
   para a tentativa inválida.

8. **Dada** a solicitação de qualquer operando, **quando** informa `1000000000,000001`,
   `-1000000000,000001` ou `0,1234567`, **então** recebe uma mensagem de entrada fora do
   intervalo ou da precisão permitida, conforme o caso, sem arredondamento automático nem
   resultado para a tentativa. A orientação informa o limite que foi excedido.
9. **Dada** a adição selecionada, **quando** informa `1000000000` e `0,000001`, **então**
   recebe uma mensagem específica de resultado fora do intervalo permitido, sem resultado
   numérico, e retorna à escolha de operação, descartando todos os dados da tentativa.
10. **Dada** a subtração selecionada, **quando** informa `-1000000000` e `0,000001`, **então**
    recebe a mensagem específica de resultado fora do intervalo permitido, sem resultado numérico.
11. **Dada** uma entrada ou resultado que excede os limites, **quando** o usuário segue a
    orientação de recuperação, **então** consegue concluir um cálculo válido na mesma sessão.
    Entradas excessivas repetem somente o operando problemático; resultados excessivos retornam
    à escolha de operação e permitem iniciar outro cálculo ou encerrar.

12. **Dada** uma operação escolhida e uma entrada inválida para o primeiro operando,
    **quando** o usuário corrige essa entrada, **então** a operação permanece selecionada
    e a interação segue para o segundo operando.
13. **Dada** a escolha de operação, **quando** informa uma opção inválida, **então** a escolha
    é repetida, permitindo selecionar uma operação válida ou encerrar.
14. **Dada** a divisão de 0 por 0 recusada, **quando** informa somente um novo divisor 2,
    **então** visualiza `0`, preservando a operação e o primeiro operando.
15. **Dada** uma operação com um operando fora do intervalo ou da precisão permitida,
    **quando** corrige somente esse operando, **então** a operação e os operandos válidos já
    informados são preservados e o cálculo pode prosseguir.
16. **Dado** um resultado fora do intervalo permitido, **quando** a mensagem é apresentada,
    **então** a interação retorna à escolha de operação, descarta a operação e os dois
    operandos e permite iniciar outro cálculo ou encerrar.

### História do usuário 3 — Repetir cálculos e encerrar (Prioridade: P2)

Como usuário, quero realizar vários cálculos na mesma sessão e encerrar quando decidir.

**Motivo da prioridade**: Completa o ciclo de uso sem exigir reiniciar o programa.

**Teste independente**: Concluir dois cálculos consecutivos e escolher encerrar; em outra sessão,
escolher encerrar antes de realizar qualquer cálculo.

**Cenários de aceitação**:

1. **Dado** um cálculo concluído, **quando** o usuário deseja continuar, **então** pode escolher
   novamente uma operação e informar um novo par de números sem reiniciar a aplicação.
2. **Dado** um segundo cálculo com operação e números diferentes, **quando** ele é concluído,
   **então** o resultado corresponde aos dados atuais, sem reutilização automática dos anteriores.
3. **Dada** a escolha de operação, no início ou após um cálculo, **quando** o usuário escolhe
   encerrar, **então** recebe uma confirmação de encerramento e não há novas solicitações.
4. **Dada** a escolha de operação, **quando** informa `9`, **então** recebe uma mensagem de
   operação inválida e o menu é apresentado novamente; **quando** escolhe `0` nesse menu,
   **então** recebe a confirmação de encerramento, sem solicitação de operandos nem novas entradas.

### Casos de borda

- Zero é um operando válido nas quatro operações, exceto como divisor.
- Dividir zero por um número diferente de zero produz zero.
- A ordem dos operandos deve ser respeitada na subtração e na divisão.
- Números negativos e decimais podem aparecer em qualquer um dos dois operandos.
- Operação inexistente, texto não numérico e entrada vazia não produzem um cálculo válido.
- Erros consecutivos não devem encerrar a sessão nem apresentar o resultado de um cálculo
  anterior como se fosse o resultado da tentativa atual.
- Somente a vírgula é aceita como separador decimal e usada na apresentação dos resultados.
- Espaços nas extremidades são ignorados; uma entrada somente de espaços é inválida.
- Ponto decimal, separadores de milhares, espaços internos e notação científica são inválidos.
- Resultados são apresentados com até seis casas decimais, arredondados para longe de zero
  em caso de empate, sem zeros finais; resultados arredondados a zero são apresentados como `0`.
- Os operandos válidos não são arredondados antes do cálculo. A precisão de entrada é validada
  sem arredondamento; o arredondamento do resultado aplica-se somente à apresentação.
- Os extremos -1000000000 e 1000000000 são aceitos como entradas e resultados.
- O intervalo do resultado é verificado antes do arredondamento de apresentação.
- Entradas com mais de seis casas decimais após a vírgula, desconsiderando zeros finais,
  são rejeitadas sem arredondamento automático.
- Resultados além do intervalo, inclusive por multiplicação ou divisão por um número pequeno,
  geram erro específico de limite excedido; não são apresentados como números válidos.

## Requisitos *(obrigatório)*

### Requisitos funcionais

- **FR-001**: A calculadora DEVE oferecer adição, subtração, multiplicação e divisão como
  operações distintas e identificáveis na interação de console.
- **FR-002**: Para cada cálculo, o usuário DEVE poder informar exatamente dois números em
  ordem, incluindo valores inteiros, decimais, negativos e zero.
- **FR-003**: A calculadora DEVE calcular a operação escolhida sobre os dois números informados,
  respeitando a ordem na subtração e na divisão.
- **FR-004**: Para um cálculo válido, a calculadora DEVE exibir o resultado identificado de
  forma que o usuário possa distingui-lo de solicitações e mensagens de erro.
- **FR-005**: Uma operação inválida DEVE gerar uma mensagem que identifique o problema e
  indique como continuar, sem resultado para a tentativa inválida.
- **FR-006**: Um número inválido, em qualquer posição, DEVE gerar uma mensagem que identifique
  o problema e indique como continuar, sem resultado para a tentativa inválida.
- **FR-007**: Divisão por zero DEVE gerar uma mensagem específica, distinta de entrada numérica
  inválida, sem apresentar um resultado numérico, inclusive para zero dividido por zero.
- **FR-008**: Após um resultado, a calculadora DEVE oferecer a escolha de nova operação ou
  encerramento na mesma sessão. Cada novo cálculo DEVE usar um novo par de números.
- **FR-009**: O usuário DEVE poder encerrar na escolha de operação, inclusive antes do primeiro
  cálculo. O encerramento DEVE ser confirmado e não solicitar novas entradas.
- **FR-010**: Operação inválida, número inválido, divisão por zero e limite numérico excedido
  NÃO DEVEM encerrar a sessão inesperadamente. Após a recuperação, um novo cálculo válido
  DEVE ser possível.
- **FR-011**: Solicitações, resultados e mensagens de erro ao usuário DEVEM estar em português
  do Brasil e identificar a ação esperada, sem exigir interpretação de exceções técnicas.
- **FR-012**: A calculadora DEVE aceitar inteiros e números decimais positivos ou negativos
  usando somente vírgula como separador decimal, e DEVE usar vírgula nos resultados decimais.
  Espaços nas extremidades DEVEM ser ignorados. Espaços internos, ponto decimal, separadores
  de milhares e notação científica DEVEM ser rejeitados como número inválido.
- **FR-013**: O resultado DEVE ser apresentado com até seis casas decimais. O arredondamento
  DEVE ocorrer somente na apresentação, para o valor mais próximo; em caso de empate, DEVE
  ocorrer para longe de zero. Zeros finais DEVEM ser omitidos, inteiros DEVEM aparecer sem casas
  decimais e um resultado arredondado a zero DEVE aparecer como `0`, sem sinal negativo.
  Entradas e resultados antes do arredondamento DEVEM estar no intervalo inclusivo de
  -1000000000 a 1000000000. Entradas DEVEM ter até seis casas decimais após a vírgula,
  desconsiderando zeros finais. Entradas fora do intervalo ou dessa precisão DEVEM ser rejeitadas
  sem arredondamento, com mensagem que identifique o limite excedido. Resultados fora do
  intervalo DEVEM gerar mensagem específica de limite excedido, sem resultado numérico.
  A sessão DEVE permanecer ativa em todos esses casos.
- **FR-014**: Operação inválida DEVE repetir a escolha de operação. Número inválido, incluindo
  entrada fora do intervalo ou da precisão permitida, DEVE repetir somente esse operando,
  preservando a operação e os operandos válidos já informados. Divisão por zero DEVE repetir
  somente o divisor, preservando a divisão e o primeiro operando. Resultado fora do intervalo
  DEVE retornar à escolha de operação e descartar a operação e os dois operandos da tentativa.
  Todas as retomadas DEVEM preservar as garantias de FR-005 a FR-007 e FR-010.

### Entidades principais

- **Operação**: Uma das quatro escolhas de cálculo permitidas.
- **Operandos**: Primeiro e segundo números de uma tentativa, cuja ordem tem significado.
- **Tentativa de cálculo**: Operação e operandos atuais, com resultado válido ou erro identificado.
- **Sessão**: Período de interação que permite várias tentativas até o encerramento escolhido.

## Critérios de sucesso *(obrigatório)*

### Resultados mensuráveis

- **SC-001**: Os cenários de cálculo da história 1 produzem 100% dos valores matemáticos
  esperados, apresentados com vírgula decimal, até seis casas decimais, arredondamento de empate
  para longe de zero e sem zeros finais.
- **SC-002**: Em 100% dos cenários de operação inválida, número inválido, divisão por zero
  e limite numérico excedido, o usuário recebe uma mensagem que identifica o problema e orienta a próxima ação, sem
  encerramento inesperado nem resultado atribuído à tentativa inválida.
- **SC-003**: O usuário consegue concluir uma sequência de dez cálculos válidos na mesma sessão,
  alternando operações e números, sem reiniciar a aplicação e sem reutilização automática de dados.
- **SC-004**: Após cada categoria de erro — operação inválida, número inválido, divisão por
  zero e limite numérico excedido — o usuário consegue concluir um cálculo
  válido na mesma sessão seguindo as instruções apresentadas, com o ponto de retomada e
  os dados preservados definidos em FR-014.
- **SC-005**: Nos dois cenários de saída — antes do primeiro cálculo e após um cálculo — a escolha
  de encerrar termina a interação sem novas solicitações de entrada.
- **SC-006**: Todas as solicitações e mensagens dos cenários de aceitação permitem identificar,
  em português do Brasil, o dado solicitado, o resultado ou o erro e a próxima ação disponível.

## Premissas

- Há um único usuário interagindo por vez, sem contas, autenticação ou perfis.
- Cada cálculo envolve uma operação e dois números; expressões encadeadas, parênteses, memória,
  histórico persistente e operações adicionais ficam fora do escopo.
- A escolha de operação apresenta também a opção de encerrar. Como premissa inicial, não há
  comando de cancelamento durante a entrada dos operandos; essa premissa pode ser revisada
  na clarificação. O formato exato dos comandos de escolha não é fixado nesta especificação.
- Não há dependência funcional de serviços externos, rede, importação ou exportação de dados.
- Encerramento por interrupção externa ou fim do fluxo de entrada não integra os cenários de
  saída voluntária desta versão; é uma fronteira de escopo a revisar se necessário.
- Os exemplos de entrada decimal usam a grafia aprovada, com vírgula como separador decimal.

## Ambiguidades registradas para clarificação

Q1, Q2 e Q3 foram resolvidas por quatro respostas aprovadas, registradas na sessão de clarificação.
As decisões foram incorporadas aos requisitos, cenários e critérios de sucesso. Não restam
ambiguidades críticas registradas; as premissas e exclusões de escopo continuam explícitas.
