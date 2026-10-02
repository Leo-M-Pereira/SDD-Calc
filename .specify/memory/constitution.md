# Constituição da Calculadora Didática SDD

## Core Principles

### I. Simplicidade e finalidade didática

Toda decisão DEVE favorecer a compreensão do fluxo de SDD com Spec Kit. A aplicação DEVE
permanecer uma calculadora de console pequena, com requisitos explícitos e critérios de
aceitação verificáveis. Recursos sem necessidade demonstrada DEVEM ser adiados. O objetivo
é aprender o processo completo com mudanças fáceis de entender.

### II. Código e identificadores em inglês

Código, nomes de projetos, arquivos de código, namespaces, tipos, métodos, variáveis e testes
DEVEM usar inglês. Identificadores DEVEM expressar sua responsabilidade e seguir as convenções
de C# e .NET, mantendo consistência com o ecossistema técnico.

### III. Documentação e explicações em português do Brasil

Constituição, especificações, planos, checklists, tarefas, decisões e explicações DEVEM usar
português do Brasil. Comentários explicativos no código também DEVEM usar esse idioma.
Termos técnicos, identificadores, comandos e nomes oficiais PODEM permanecer em inglês.
A documentação DEVE permitir acompanhar o raciocínio de cada etapa.

### IV. Testes automatizados das regras de cálculo

Toda regra de cálculo implementada DEVE ter testes automatizados de seus critérios de aceitação,
casos usuais, limites relevantes e erros previstos na especificação. Correções de defeitos de
cálculo DEVEM incluir testes de regressão. Os testes DEVEM ser determinísticos e executáveis
sem interação com o console. Alterações de lógica somente podem ser consideradas concluídas
quando todos os testes de cálculo passam.

### V. Separação entre console e lógica de negócio

A interface de console DEVE cuidar de leitura, apresentação e coordenação da interação.
A lógica de negócio DEVE receber dados e produzir resultados ou erros definidos sem ler nem
escrever no console. Regras de cálculo e validações de domínio DEVEM ser testáveis de forma
independente da interface. A separação DEVE usar a menor estrutura necessária.

### VI. Tratamento claro de entradas inválidas

Entradas inválidas e operações matematicamente inválidas DEVEM produzir mensagens compreensíveis
e uma recuperação definida pela especificação, sem encerramento inesperado. O console DEVE
validar a conversão de entradas; a lógica de negócio DEVE validar as condições do cálculo.
Falhas esperadas NÃO DEVEM ser apresentadas apenas como exceções técnicas ou stack traces.
Os requisitos DEVEM definir formatos aceitos, condições de erro e comportamento posterior.

### VII. Dependências e abstrações justificadas

A implementação DEVE priorizar recursos da plataforma .NET. Toda dependência externa ou nova
abstração DEVE resolver uma necessidade atual, documentada no plano, com justificativa de sua
vantagem sobre uma solução simples. Um framework de testes PODE ser usado para atender ao
princípio IV. Camadas, interfaces genéricas e padrões sem uso concreto NÃO DEVEM ser introduzidos.

### VIII. Etapas pequenas, revisáveis e registradas no Git

Cada etapa DEVE produzir uma mudança de escopo limitado, passível de revisão e registrada em
commit próprio no Git antes do avanço à próxima etapa. Os commits DEVEM identificar a etapa
e sua finalidade. A implementação DEVE ser dividida em incrementos coerentes com as tarefas.
Alterações alheias à etapa NÃO DEVEM ser agrupadas no mesmo commit. O histórico DEVE permitir
reconstruir a evolução dos requisitos até a solução.

## Restrições do projeto

- A aplicação DEVE ser uma calculadora de console em C# sobre .NET 10.
- Operações, tipos numéricos, precisão e formato de interação DEVEM ser definidos na
  especificação e no planejamento; esta constituição não antecipa essas decisões.
- O projeto DEVE servir ao aprendizado de SDD com Spec Kit. Serviços, persistência e interfaces
  adicionais exigem requisito explícito e revisão de conformidade antes de ampliar o escopo.
- A etapa de constituição DEVE limitar-se à governança. Código da aplicação, testes e outros
  artefatos de implementação NÃO DEVEM ser criados nesta etapa.

## Fluxo de desenvolvimento e revisão

O projeto DEVE percorrer as etapas abaixo, mantendo os artefatos em português do Brasil:

1. `constitution`: estabelecer e revisar os princípios que governam o projeto.
2. `specify`: definir o comportamento esperado e os critérios de aceitação.
3. `clarify`: resolver ambiguidades e registrar as respostas na especificação.
4. `plan`: definir a solução técnica mínima e verificar sua conformidade com a constituição.
5. `checklist`: revisar a qualidade, clareza e completude dos requisitos.
6. `tasks`: decompor o plano em tarefas pequenas, ordenadas por dependências e verificáveis.
7. `analyze`: verificar a consistência entre especificação, plano e tarefas e resolver os
   problemas que impeçam a implementação.
8. `implement`: executar as tarefas, verificar os critérios de aceitação e rodar os testes.

Antes do commit de cada etapa, a revisão DEVE conferir seu escopo, a clareza do artefato e o
atendimento aos princípios aplicáveis. Uma revisão individual documentada no artefato da etapa
ou na descrição do commit é suficiente para este projeto didático. Etapas documentais NÃO exigem
testes de aplicação; incrementos de implementação DEVEM compilar e passar nos testes existentes.
Pendências que afetem a etapa seguinte DEVEM ser resolvidas ou explicitamente registradas com
sua consequência antes do avanço. A implementação só DEVE começar após a análise dos artefatos.

## Governance

Esta constituição prevalece sobre decisões locais de especificação, planejamento e implementação.
Cada revisão de artefato ou mudança de código DEVE verificar sua conformidade. Desvios DEVEM ser
corrigidos ou precedidos por uma alteração explícita da constituição; não há exceções silenciosas.

Propostas de alteração DEVEM registrar a motivação, os princípios afetados e o impacto nos
artefatos existentes. O responsável pelo projeto DEVE revisar e aceitar a alteração antes de
sua adoção. A constituição atualizada DEVE registrar a nova versão e a data da alteração, e ser
registrada em commit próprio. Artefatos afetados DEVEM ser revisados antes do próximo avanço.

A versão DEVE seguir `MAJOR.MINOR.PATCH`: MAJOR para remoção ou redefinição incompatível de
princípios; MINOR para inclusão de princípios ou ampliação material de regras; PATCH para
correções e esclarecimentos sem mudança de obrigação. A data de ratificação original DEVE ser
preservada. A data da última alteração DEVE refletir a modificação mais recente da constituição.

**Version**: 1.0.0 | **Ratified**: 2026-10-02 | **Last Amended**: 2026-10-02
