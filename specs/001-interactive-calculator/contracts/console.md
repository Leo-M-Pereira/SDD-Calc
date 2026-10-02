# Contrato de interação de console

**Base normativa**: [spec.md](../spec.md), especialmente FR-001 a FR-014.
Este contrato concretiza o menu e a sintaxe para implementação e testes, preservando as
quatro decisões aprovadas. Não há API de rede, argumentos de cálculo ou formato de arquivo.

## Menu

| Entrada após remover espaços externos | Ação |
|---|---|
| `1` | Adição |
| `2` | Subtração |
| `3` | Multiplicação |
| `4` | Divisão |
| `0` | Encerrar |

Apresentar os cinco códigos e seus nomes em português. Qualquer outro código é operação inválida,
incluindo entrada vazia, `5`, `01` ou texto. Repetir o menu sem pedir operandos nessa falha.
A escolha por códigos curtos é uma decisão de desenho para tornar o exercício previsível.

## Números

Após `Trim`, a entrada deve ter sinal inicial opcional `+` ou `-`, um ou mais dígitos ASCII de
0 a 9 e, opcionalmente, uma vírgula seguida de um ou mais dígitos. A gramática equivale a
`[+-]?[0-9]+(,[0-9]+)?` aplicada à entrada inteira. Sinal explícito positivo e zeros iniciais são
aceitos por serem formas numéricas usuais; `,5` e `5,` são rejeitados por falta de uma das partes.

- Exemplos válidos: `2`, `-3`, `+2`, `001,5`, `  -2,25  `, `0,1234560` e os extremos ±1000000000.
- Exemplos inválidos por formato: vazio, somente espaços, `abc`, `1.5`, `1.000,5`, `1 000`,
  `1e3`, `--2`, `,5`, `5,`, `NaN`, `Infinity`.
- Após ignorar zeros finais da fração, mais de seis dígitos fracionários é erro de precisão.
  `0,1234560` é válido; `0,1234567` é inválido. Não arredondar a entrada para fazê-la caber.
- Valor fora do intervalo inclusivo ±1000000000 é erro de intervalo. Normalizar zeros
  redundantes não altera o valor nem autoriza formatos proibidos.
- Se uma entrada viola mais de uma regra, verificar formato, depois precisão, depois intervalo.
  Informar o primeiro erro nessa ordem. A recuperação é a mesma para os três tipos.

## Mensagens e saída

Mensagens devem identificar o problema e o próximo pedido. Os textos abaixo são referências
para a implementação; testes verificam o significado e a ordem, não espaços ou pontuação exatos.

| Situação | Conteúdo da mensagem | Recuperação |
|---|---|---|
| Opção inválida | “Operação inválida. Escolha uma das opções do menu.” | Repetir menu |
| Número malformado | “Número inválido. Use vírgula decimal e informe novamente este número.” | Repetir somente o operando |
| Precisão excedida | “Use até seis casas decimais, desconsiderando zeros finais.” | Repetir somente o operando |
| Entrada fora do intervalo | “Informe um número entre -1000000000 e 1000000000.” | Repetir somente o operando |
| Divisor zero | “Não é possível dividir por zero. Informe outro divisor.” | Repetir divisor |
| Resultado fora do intervalo | “O resultado excede o intervalo permitido. Escolha outra operação.” | Descartar tentativa e voltar ao menu |
| Resultado válido | “Resultado: ” seguido do número formatado | Voltar ao menu |
| Encerramento pelo menu | “Programa encerrado.” | Não solicitar novas entradas |

O primeiro pedido identifica “primeiro número”; o segundo identifica “segundo número” e, na
divisão, sua função como divisor. Erros de segundo operando preservam primeiro operando e operação.
Após resultado válido, os dois operandos devem ser informados de novo no próximo cálculo.

## Resultados numéricos

Verificar o intervalo do resultado bruto antes de arredondar. Apresentar no máximo seis casas,
com vírgula, sem milhares ou zeros finais; empate arredonda para longe de zero. Inteiros não
incluem parte decimal. Qualquer valor arredondado a zero aparece como `0`.

| Operação e valores | Saída numérica |
|---|---|
| 0,1 + 0,2 | `0,3` |
| 7 ÷ 2 | `3,5` |
| 1 ÷ 3 | `0,333333` |
| 2469133 ÷ 2000000 | `1,234567` |
| -2469133 ÷ 2000000 | `-1,234567` |
| 0,000001 ÷ 2 | `0,000001` |
| -0,000001 ÷ 2 | `-0,000001` |
| -1 ÷ 10000000 | `0` |
| 1000000000 + 0,000001 | Erro de limite; nenhuma saída numérica |

Fim de entrada não é um código de menu: parar a leitura sem resultado parcial. Esse cuidado
não amplia os cenários de saída voluntária definidos na especificação.
