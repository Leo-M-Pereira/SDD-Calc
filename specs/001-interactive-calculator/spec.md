# Especificação da funcionalidade: Calculadora interativa de console

**Branch atual**: `001-interactive-calculator`

**Criado em**: 2026-10-02

**Status**: Rascunho — com ambiguidades registradas para `$speckit-clarify`

**Entrada**: Calculadora interativa de console que permite escolher adição, subtração,
multiplicação ou divisão; informar dois números, incluindo decimais e negativos; visualizar
resultados; receber mensagens claras para entradas inválidas e divisão por zero; realizar
novos cálculos sem reiniciar; e escolher quando encerrar.

## Cenários do usuário e testes *(obrigatório)*

### História do usuário 1 — Realizar um cálculo (Prioridade: P1)

Como usuário, quero escolher uma das quatro operações e informar dois números para visualizar
o resultado, inclusive ao trabalhar com valores decimais e negativos.

**Motivo da prioridade**: É o valor central da calculadora.

**Teste independente**: Executar cada operação com pares de números e comparar o resultado
exibido com o resultado matemático esperado. A grafia dos decimais seguirá a decisão Q1.

**Cenários de aceitação**:

1. **Dado** o início de uma sessão, **quando** o usuário consulta as opções, **então** encontra
   adição, subtração, multiplicação, divisão e a opção de encerrar.
2. **Dada** a adição selecionada, **quando** informa 2 e 3, **então** visualiza o resultado 5.
3. **Dada** a subtração selecionada, **quando** informa 2 e 5 nessa ordem, **então** visualiza -3.
4. **Dada** a multiplicação selecionada, **quando** informa -4 e 3, **então** visualiza -12.
5. **Dada** a divisão selecionada, **quando** informa 7 e 2 nessa ordem, **então** visualiza o
   valor correspondente a três e meio.
6. **Dada** a adição selecionada, **quando** informa os valores decimais um e meio e dois e
   vinte e cinco centésimos, **então** visualiza o valor correspondente a três e setenta e
   cinco centésimos.
7. **Dada** a multiplicação selecionada, **quando** informa -2 e -3, **então** visualiza 6.
8. **Dada** uma operação válida com dois números válidos, **quando** o cálculo termina,
   **então** o resultado fica identificado como resultado do cálculo atual.

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
5. **Dada** uma tentativa recusada por qualquer um desses erros, **quando** segue a orientação
   de recuperação e informa uma operação e números válidos, **então** consegue concluir um
   novo cálculo na mesma sessão. O ponto de retomada fica pendente em Q3.

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

### Casos de borda

- Zero é um operando válido nas quatro operações, exceto como divisor.
- Dividir zero por um número diferente de zero produz zero.
- A ordem dos operandos deve ser respeitada na subtração e na divisão.
- Números negativos e decimais podem aparecer em qualquer um dos dois operandos.
- Operação inexistente, texto não numérico e entrada vazia não produzem um cálculo válido.
- Erros consecutivos não devem encerrar a sessão nem apresentar o resultado de um cálculo
  anterior como se fosse o resultado da tentativa atual.
- Separadores decimais, agrupamento de milhares e notação científica dependem de Q1.
- Divisões com resultado não exato, como 1 dividido por 3, e valores além dos limites aceitos
  dependem de Q2; não há política de arredondamento ou limite presumida.

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
- **FR-010**: Operação inválida, número inválido e divisão por zero NÃO DEVEM encerrar a sessão
  inesperadamente. Após a recuperação, um novo cálculo válido DEVE ser possível.
- **FR-011**: Solicitações, resultados e mensagens de erro ao usuário DEVEM estar em português
  do Brasil e identificar a ação esperada, sem exigir interpretação de exceções técnicas.
- **FR-012**: Os formatos aceitos para números e sua apresentação DEVEM ser definidos em Q1.
- **FR-013**: A precisão, o arredondamento e os limites numéricos observáveis DEVEM ser definidos
  em Q2, incluindo a mensagem e a recuperação para valores ou resultados fora desses limites.
- **FR-014**: O ponto de retomada e a preservação de dados após cada erro DEVEM ser definidos
  em Q3, mantendo as garantias de FR-005, FR-006, FR-007 e FR-010.

### Entidades principais

- **Operação**: Uma das quatro escolhas de cálculo permitidas.
- **Operandos**: Primeiro e segundo números de uma tentativa, cuja ordem tem significado.
- **Tentativa de cálculo**: Operação e operandos atuais, com resultado válido ou erro identificado.
- **Sessão**: Período de interação que permite várias tentativas até o encerramento escolhido.

## Critérios de sucesso *(obrigatório)*

### Resultados mensuráveis

- **SC-001**: Os cenários de cálculo da história 1 produzem 100% dos valores matemáticos
  esperados, conforme as regras de formato e precisão definidas na clarificação.
- **SC-002**: Em 100% dos cenários de operação inválida, número inválido e divisão por zero,
  o usuário recebe uma mensagem que identifica o problema e orienta a próxima ação, sem
  encerramento inesperado nem resultado atribuído à tentativa inválida.
- **SC-003**: O usuário consegue concluir uma sequência de dez cálculos válidos na mesma sessão,
  alternando operações e números, sem reiniciar a aplicação e sem reutilização automática de dados.
- **SC-004**: Após cada uma das três categorias de erro, o usuário consegue concluir um cálculo
  válido na mesma sessão seguindo as instruções apresentadas.
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
- Os exemplos decimais descrevem valores matemáticos, não uma grafia de entrada já aprovada.

## Ambiguidades registradas para clarificação

As decisões abaixo foram deliberadamente adiadas a pedido do usuário. Esta especificação ainda
não está pronta para planejamento: os critérios afetados devem ser completados após as respostas.

- **Q1 — Formatos numéricos**: [NEEDS CLARIFICATION: Quais separadores decimais serão aceitos e usados na saída, e serão permitidos agrupamento de milhares, espaços e notação científica?]
  Afeta FR-002, FR-006, FR-012, os cenários decimais e SC-001.
- **Q2 — Precisão e limites**: [NEEDS CLARIFICATION: Qual será a precisão exibida, a regra de arredondamento e o tratamento de números ou resultados fora dos limites aceitos?]
  Afeta FR-003, FR-004, FR-013, a divisão não exata e SC-001.
- **Q3 — Recuperação de erros**: [NEEDS CLARIFICATION: Após operação inválida, número inválido ou divisão por zero, a interação repete a entrada problemática ou retorna à escolha de operação, e quais dados já informados são preservados?]
  Afeta FR-005 a FR-007, FR-010, FR-014, a história 2 e SC-002/SC-004.
