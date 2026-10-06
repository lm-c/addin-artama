# Proposta Comercial / Orçamento

**Cliente:** MMC - Metalúrgica
**Solução:** Addin Axion + Axion Connect
**Data:** 01/10/2026
**Validade da proposta:** 30 dias
**Responsável comercial:** Carlos Martins

---

## 1. Apresentação

Esta proposta descreve a implantação da solução **Axion**, composta por dois módulos integrados que conectam o setor de projetos (SolidWorks) à engenharia de produto e à produção da MMC - Metalúrgica:

| Módulo            | Onde é utilizado               | Usuário principal           |
| ----------------- | ------------------------------ | --------------------------- |
| **Addin Axion**   | Dentro do SolidWorks           | Projetistas / Engenharia    |
| **Axion Connect** | Aplicação desktop independente | Engenharia de produto / PCP |

**Objetivo:** eliminar a digitação manual de listas de materiais e cadastros de produto, padronizar as informações do projeto e garantir que aquilo que foi desenhado seja exatamente o que chega à fábrica.

---

## 2. Visão Geral do Fluxo

1. O projetista finaliza o modelo 3D / conjunto no SolidWorks.
2. Pelo **Addin Axion**, aplica matéria-prima, valida propriedades e gera a lista de materiais estruturada.
3. O Addin Axion valida as informações (peso, comprimento, espessura, itens fantasmas etc.) e bloqueia o envio quando há inconsistências, avisando o projetista.
4. Produtos, componentes e processos são enviados à engenharia via API.
5. No **Axion Connect**, a engenharia confere a estrutura de produto, vincula operações e sequências de fabricação e libera os dados para a produção.

---

## 3. Módulo 1 — Addin Axion (SolidWorks)

Add-in nativo que roda dentro do SolidWorks, acessível por painel de tarefas, sem necessidade de sair do CAD.

### 3.1 Login e controle de acesso

- Autenticação por usuário e senha, com alteração de senha pelo próprio usuário.
- Cadastro de usuários, **perfis** e **permissões** por funcionalidade.
- Permite restringir, por exemplo, quem pode enviar produtos para a engenharia ou alterar cadastros.

### 3.2 Aplicação de matéria-prima

- Associação da matéria-prima e do material a cada peça do modelo (chapas, perfis, tubos etc.).
- Gravação automática das propriedades no arquivo (código do material, espessura, largura/comprimento da chapa, descrição).
- Atualização do produto ativo ao alterar um código de chapa, sem precisar recarregar toda a estrutura.

### 3.3 Propriedades do arquivo

- Leitura, edição e padronização das propriedades personalizadas do SolidWorks (código, denominação, material, massa, configuração etc.).
- Tela de manutenção de propriedades em lote, reduzindo retrabalho.

### 3.4 Desenhos

- Atualização de formatos/folhas de desenho conforme o padrão da empresa.
- Preenchimento automático de legenda e dados do desenho a partir das propriedades do modelo.

### 3.5 Exportação

- Exportação de **DXF** (corte) e **PDF** (desenho técnico), individual ou em lote.
- Geração de relatórios diretamente pelo Addin Axion.

### 3.6 Cadastros de apoio

- Materiais e matérias-primas, com suas características de cadastro.
- Usuários, perfis e permissões.

### 3.7 Relatórios

- **Lista de materiais (BOM)** com colunas configuráveis: posição/nível, quantidade, código, componente, código do produto, material, denominação, espessura, largura, comprimento, configuração, operação, massa e indicação de item fantasma.
- **Processo de fabricação**.

### 3.8 Integrador com a engenharia

- **Importação de produto**: envia a estrutura do projeto (produto, componentes, insumos) para o Axion Connect / sistema de produção.
- **Cadastro de processos e operações**: vincula cada item a uma sequência operacional (corte, dobra, solda, pintura, terceiros etc.).
- **Regras de validação:** interrompe o envio e avisa o projetista quando peso ou comprimento estiverem zerados; itens fantasmas não recebem processo; tratamento de itens de grande volume de estrutura.
- Configuração da conexão com o sistema de destino.

---

## 4. Módulo 2 — Axion Connect (Engenharia de Produto)

Aplicação desktop para a engenharia/PCP que recebe os dados do Addin Axion e completa o cadastro técnico do produto.

### 4.1 Inicialização e login

- Tela inicial com autenticação de usuários e seleção do ambiente/empresa.

### 4.2 Engenharia de produto

- Tela central para conferência da estrutura de produto recebida do CAD (níveis, quantidades, materiais, medidas).
- Ajuste e complemento dos dados antes da liberação para produção.
- Apoio à manutenção de engenharia de itens pré-cadastrados e verificação de componentes não seriados.

### 4.3 Operações, máquinas e sequências

- Cadastro e vínculo de **operações** e **grupos de máquinas** aos itens da estrutura.
- Seleção da **sequência operacional** de cada componente.
- Tratamento de operações de terceiros.

### 4.4 Camada de integração (API)

- Comunicação com o Addin Axion por API, cobrindo: produto/engenharia, operações, máquinas e itens genéricos.
- Garante que a estrutura desenhada seja a mesma que a produção recebe, sem retrabalho de digitação.

---

## 5. Adequações Previstas para a MMC - Metalúrgica

A solução parte de uma base já validada em operação. Para a MMC estão previstos ajustes pontuais, **a confirmar na reunião de levantamento**:

| Item | Descrição                                                                        | Módulo                      |
| ---- | -------------------------------------------------------------------------------- | --------------------------- |
| A1   | Adequação do padrão de codificação de itens e materiais da MMC                   | Addin Axion / Axion Connect |
| A2   | Ajuste das colunas e layout da lista de materiais e demais relatórios            | Addin Axion                 |
| A3   | Configuração do parque de máquinas, grupos de máquinas e operações da MMC        | Axion Connect               |
| A4   | Parametrização das propriedades personalizadas do SolidWorks conforme padrão MMC | Addin Axion                 |
| A5   | Adequação de formatos de desenho e legenda ao padrão MMC                         | Addin Axion                 |
| A6   | Identidade visual (logotipo, nomes e textos)                                     | Addin Axion / Axion Connect |

> Qualquer necessidade fora desta lista será tratada como escopo adicional, com orçamento à parte.

---

## 6. Requisitos do Ambiente

- Estações de projeto com **SolidWorks** instalado (versão a confirmar) para o Addin Axion.
- Estação ou servidor com **Windows** para o Axion Connect (.NET Framework 4.8).
- Servidor de banco de dados **MySQL** acessível pela rede.
- Acesso de rede entre estações de projeto, Axion Connect e banco de dados.
- Disponibilidade de um responsável da MMC para validação das etapas.

---

## 7. Investimento

Axion Addin e Axion Connect são licenciados como produtos independentes, ambos operando sobre o mesmo **Axion Licensing Service**, responsável pelo controle de ativações por licença/terminal.

### 7.1 Setup (pagamento único)

| Item  | Descrição                                                                                    | Valor          |
| ----- | --------------------------------------------------------------------------------------------- | -------------- |
| Setup | Instalação, configuração do ambiente, adequações (seção 5) e treinamento remoto da equipe     | **R$ 5.000,00** |

### 7.2 Licenciamento mensal

| Produto           | Licenciamento                 |      Valor |
| ------------------ | ------------------------------ | ---------: |
| **Axion Addin**   | por licença/uso no SolidWorks | **R$ 500,00** |
| **Axion Connect** | por terminal                  | **R$ 800,00** |

*Valor mensal calculado conforme a quantidade de licenças/terminais ativos na MMC - Metalúrgica, a ser confirmada na implantação.*

---

## 8. Condições Comerciais

- **Forma de pagamento:** setup de R$ 5.000,00 (pagamento único, no início do projeto) + mensalidade por licença/terminal ativo, conforme tabela de licenciamento da seção 7.2.
- **Prazo de entrega:** 90 dias após a aprovação da proposta.
- **Garantia:** 90 dias a partir da instalação e homologação, para correção de falhas do escopo entregue.
- **Validade:** conforme cabeçalho.
- Alterações de escopo após a aprovação serão orçadas separadamente.

---

## 9. Próximos Passos

1. Reunião de alinhamento para validar as adequações da seção 5.
2. Fechamento de escopo, prazos e valores.
3. Aprovação desta proposta.
4. Início do desenvolvimento.

---

_Proposta sujeita à confirmação do escopo após reunião de levantamento com a MMC - Metalúrgica._
