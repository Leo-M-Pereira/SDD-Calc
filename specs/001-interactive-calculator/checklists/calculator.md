# Checklist de qualidade dos requisitos: Calculadora interativa de console

**Finalidade**: Revisar clareza, completude, consistência, mensurabilidade e cobertura dos requisitos.
**Criado em**: 2026-10-02
**Funcionalidade**: [Especificação esclarecida](../spec.md)
**Profundidade**: Padrão, com atenção às regras numéricas e à recuperação de erros.
**Público e momento**: Autor e revisor dos requisitos, após o planejamento e antes das tarefas.

**Nota**: Checklist personalizado gerado por `$speckit-checklist` conforme o escopo solicitado.
**Responsabilidade da revisão**: Os marcadores pertencem ao revisor; cada aprovação deve ser
sustentada pela documentação citada.
**Significado dos marcadores**: `[x]` indica qualidade documental aprovada pelo revisor, não
funcionalidade implementada nem teste executado. Os marcadores abaixo registram a revisão documental; `[x]` aprova somente a qualidade do requisito.

## Referências

- **Spec**: [spec.md](../spec.md); FR identifica requisito funcional e SC identifica critério de sucesso.
- **Plano**: [plan.md](../plan.md).
- **Contrato**: [contracts/console.md](../contracts/console.md).
- **Modelo**: [data-model.md](../data-model.md).
- **Guia**: [quickstart.md](../quickstart.md).
- **Pesquisa**: [research.md](../research.md), justificativas das decisões de planejamento.
- Referências § indicam identificadores ou títulos de seção dos documentos acima.
- `[Gap]` indica possível lacuna a avaliar, sem afirmar que ela já foi constatada.

## Completude dos requisitos

- [x] CHK001 Os formatos aceitos e rejeitados estão definidos para ambos os operandos, incluindo vírgula, sinais, espaços, milhares e notação científica? [Completude, Spec §FR-002/FR-012, Contrato §Números]
- [x] CHK002 Entradas vazias, sinais repetidos e partes decimais incompletas têm aceitação ou rejeição explicitamente documentada? [Completude, Spec §História 2, Contrato §Números]
- [x] CHK003 O limite de seis casas na entrada está definido após desconsiderar zeros finais, separadamente das casas exibidas no resultado? [Completude, Spec §FR-013, Contrato §Números]
- [x] CHK004 O intervalo inclusivo de -1000000000 a 1000000000 está especificado separadamente para operandos e resultado bruto? [Completude, Spec §FR-013]
- [x] CHK005 As categorias operação inválida, formato inválido, precisão excedida, entrada excessiva, divisão por zero e resultado excessivo têm mensagens e recuperação definidas? [Completude, Spec §FR-005/FR-006/FR-007/FR-010/FR-014, Contrato §Mensagens e saída]
- [x] CHK006 A disponibilidade de novo cálculo ou encerramento após resultado válido ou excessivo está definida com o destino dos dados da tentativa anterior? [Completude, Spec §FR-008/FR-009/FR-014, Modelo §Tentativa e sessão]

## Clareza das regras numéricas e mensagens

- [x] CHK007 O arredondamento está definido para o valor mais próximo, com empate para longe de zero em ambos os sinais, somente na apresentação? [Clareza, Spec §FR-013, Contrato §Resultados numéricos]
- [x] CHK008 A omissão de zeros finais, a apresentação de inteiros e o zero sem sinal estão definidos sem interpretações alternativas? [Clareza, Spec §FR-013/História 1]
- [x] CHK009 A rejeição de entradas excessivamente precisas está definida sem arredondamento para torná-las válidas? [Clareza, Spec §FR-013, Contrato §Números]
- [x] CHK010 A precedência das mensagens para entradas com múltiplas violações está documentada? [Clareza, Spec §FR-006/FR-013, Contrato §Números]
- [x] CHK011 A expressão “mensagem clara” está concretizada pelo problema identificado e pelo próximo pedido, distinguindo conteúdo obrigatório de redação exemplificativa? [Clareza, Spec §FR-011/SC-006, Contrato §Mensagens e saída]

## Consistência entre documentos

- [x] CHK012 As quatro respostas aprovadas são consistentes com formatos, limites, arredondamento e recuperação nos requisitos e no contrato? [Consistência, Spec §Clarifications/FR-012/FR-013/FR-014, Contrato §Números/Mensagens e saída]
- [x] CHK013 As transições do modelo e do plano preservam os mesmos dados que FR-014 para cada erro? [Consistência, Spec §FR-014, Plano §Recuperação de erros e interação, Modelo §Tentativa e sessão]
- [x] CHK014 Os exemplos decimais e de arredondamento têm valores esperados coerentes entre especificação, contrato e guia? [Consistência, Spec §História 1, Contrato §Resultados numéricos, Guia §Roteiros de aceitação]
- [x] CHK015 Os códigos de menu, o sinal positivo explícito e os zeros iniciais do contrato estão identificados como decisões de desenho compatíveis com o escopo e as respostas aprovadas? [Consistência, Spec §Premissas/Clarifications, Contrato §Menu/Números]
- [x] CHK016 A exclusão de cancelamento durante os operandos e de saída por interrupção externa é coerente com o cuidado de fim de entrada previsto no plano, sem confundi-lo com saída voluntária? [Consistência, Spec §Premissas/FR-009, Plano §Recuperação de erros e interação]

## Qualidade dos critérios de aceitação

- [x] CHK017 Cada requisito FR-001 a FR-014 possui cenário ou resultado esperado identificável, incluindo as decisões da clarificação? [Rastreabilidade, Spec §Requisitos funcionais/Cenários do usuário e testes, Plano §Estratégia de testes e rastreabilidade]
- [x] CHK018 Os cenários das quatro operações definem ordem dos operandos e resultados esperados, incluindo negativos e decimais? [Mensurabilidade, Spec §FR-003/História 1/SC-001]
- [x] CHK019 Os critérios distinguem ausência de resultado numérico de um resultado zero válido sem depender de detalhes de implementação? [Mensurabilidade, Spec §FR-004/FR-007/FR-013/SC-002, Modelo §Invariantes]
- [x] CHK020 As metas de 100% dos cenários e dez cálculos estão vinculadas a um conjunto reconhecível de cenários e resultados esperados? [Mensurabilidade, Spec §SC-001/SC-002/SC-003, Guia §Roteiros de aceitação/Validações adicionais]

## Cobertura de recuperação, repetição e encerramento

- [x] CHK021 Os cenários de entrada inválida no primeiro e no segundo operando especificam a operação e os dados preservados em cada posição? [Cobertura, Spec §História 2/FR-014]
- [x] CHK022 A correção de divisor zero, inclusive em zero dividido por zero, está descrita com preservação do primeiro operando e repetição somente do divisor? [Cobertura, Spec §História 2/FR-007/FR-014]
- [x] CHK023 A recuperação de resultado excessivo está descrita como descarte da operação e dos dois operandos, com retorno à escolha de operação? [Cobertura, Spec §História 2/FR-014, Modelo §Tentativa e sessão]
- [x] CHK024 Erros consecutivos e ausência de reutilização automática de dados entre cálculos têm requisitos documentados para evitar lacunas entre tentativas? [Cobertura, Spec §Casos de borda/FR-008/SC-003]
- [x] CHK025 Os cenários de encerramento cobrem antes do primeiro cálculo, depois de um cálculo e após operação inválida, com confirmação e ausência de novos pedidos? [Cobertura, Spec §História 2/História 3/FR-009/SC-005]

## Casos de fronteira, premissas e lacunas

- [x] CHK026 Os requisitos contemplam os dois extremos inclusivos e valores imediatamente externos, distinguindo fronteiras de entrada e de resultado bruto? [Cobertura de fronteira, Spec §FR-013/História 1/História 2]
- [x] CHK027 Sexta versus sétima casa, zeros finais excedentes, empates dos dois sinais e valores arredondados a zero têm resultados de aceitação explícitos? [Cobertura de fronteira, Spec §História 1/História 2/Casos de borda, Contrato §Resultados numéricos]
- [x] CHK028 Multiplicação e divisão com resultado excessivo estão cobertas por critérios ou regra geral inequívoca, sem limitar a rejeição aos exemplos de adição e subtração? [Gap, Cobertura de fronteira, Spec §FR-013/Casos de borda, Plano §Estratégia de testes e rastreabilidade]
- [x] CHK029 As premissas de um usuário, ausência de persistência e operações limitadas estão explícitas e compatíveis com a finalidade didática? [Premissas, Spec §Premissas, Plano §Contexto técnico]
- [x] CHK030 A exigência de mensagens em português e a independência do formato numérico em relação ao ambiente estão definidas de maneira consistente, sem acrescentar metas não solicitadas? [Clareza, Spec §FR-011/FR-012, Contrato §Números/Mensagens e saída]

## Evidências da revisão

- **CHK001** — Spec §FR-012 e contrato §Números enumeram vírgula, sinais, espaços, milhares e notação científica.
- **CHK002** — História 2 e contrato §Números cobrem vazios, sinais repetidos e vírgula sem uma das partes.
- **CHK003** — Spec §FR-013 e contrato §Números separam seis casas efetivas na entrada de seis casas exibidas.
- **CHK004** — Spec §FR-013 define intervalo inclusivo para entradas e resultado antes do arredondamento.
- **CHK005** — Spec §FR-005 a §FR-007/§FR-014 e contrato §Mensagens e saída cobrem categorias e recuperação.
- **CHK006** — Spec §FR-008/§FR-009/§FR-014 e modelo §Tentativa e sessão definem continuidade, saída e descarte.
- **CHK007** — Spec §FR-013 e contrato §Resultados numéricos determinam empate para longe de zero, só na apresentação.
- **CHK008** — Spec §FR-013 e História 1 definem zeros finais, inteiros sem casas e zero sem sinal.
- **CHK009** — Spec §FR-013 e contrato §Números rejeitam precisão excessiva sem arredondar a entrada.
- **CHK010** — Contrato §Números ordena a validação por formato, precisão e intervalo.
- **CHK011** — Spec §FR-011/§SC-006 e contrato §Mensagens e saída definem problema, ação seguinte e textos de referência.
- **CHK012** — Respostas em Spec §Clarifications coincidem com FR-012 a FR-014 e contrato §Números/Mensagens e saída.
- **CHK013** — Tabela de recuperação do plano e transições do modelo reproduzem FR-014.
- **CHK014** — Exemplos de 3,5, 3,75 e 0,333333 coincidem na Spec §História 1, contrato e guia §Roteiros.
- **CHK015** — Contrato §Menu/Números define códigos, sinal positivo e zeros iniciais; sinaliza os códigos como decisão de desenho.
- **CHK016** — Premissas da Spec excluem cancelamento durante operandos e interrupção externa; plano trata fim de entrada como proteção, separado da saída pelo menu.
- **CHK017** — Spec §Cenários e §Requisitos oferece cenários ou resultados esperados para FR-001 a FR-014; plano §Estratégia liga-os aos testes previstos.
- **CHK018** — Spec §História 1 mostra as quatro operações, ordem, negativos, decimais e SC-001.
- **CHK019** — Spec §FR-004/§FR-007/§FR-013 e modelo §Invariantes distinguem erro sem número de zero válido.
- **CHK020** — Spec §SC-001 a §SC-003 define metas; guia liga roteiros e dez cálculos a esses critérios.
- **CHK021** — Spec §História 2 e FR-014 definem recuperação de primeiro e segundo operandos e preservação.
- **CHK022** — Spec §História 2, cenário 14, e FR-007/FR-014 cobrem zero sobre zero e repetição do divisor.
- **CHK023** — Spec §História 2, cenário 16, FR-014 e modelo definem descarte e retorno ao menu.
- **CHK024** — Spec §Casos de borda proíbe resultado antigo após erros consecutivos; FR-008/SC-003 vedam reutilização automática.
- **CHK025** — Spec §História 3, cenário 4, explicita `9` → erro e reapresentação do menu → `0` → confirmação sem operandos nem novas entradas; quickstart §Roteiros de aceitação repete a sequência.
- **CHK026** — Spec §História 1/2 e FR-013 incluem extremos e valores imediatamente fora para entrada e resultado bruto.
- **CHK027** — Spec §História 1/2 e contrato §Resultados numéricos especificam sexta/sétima casa, zeros finais, empates positivos/negativos e arredondamento a zero.
- **CHK028** — Spec §FR-013 e §Casos de borda aplicam limite a qualquer resultado; plano inclui fronteira e contrato menciona multiplicação/divisão.
- **CHK029** — Spec §Premissas delimita usuário, persistência e operações; plano §Contexto técnico preserva o escopo pequeno.
- **CHK030** — Spec §FR-011/FR-012 e contrato definem português e vírgula; plano/pesquisa fixam cultura `pt-BR`, e guia prevê verificar independência do ambiente.

## Resultado da revisão

- **Resultado documental**: 30/30 itens sustentados pela documentação após a complementação de CHK025.
- CHK025 foi aprovado com base no cenário da especificação e no roteiro do quickstart. A aprovação é documental; não declara implementação nem testes executados.
- `$speckit-implement` consulta o estado deste checklist como condição de entrada e não deve modificar os marcadores.
- O checklist interno [requirements.md](requirements.md) tem ciclo separado; não foi alterado.
