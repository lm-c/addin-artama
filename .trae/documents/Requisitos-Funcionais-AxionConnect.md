# Requisitos Funcionais - AxionConnect

**Cliente:** B.Lotti\
**Sistema:** AxionConnect\
**Data:** 29/09/2025\
**Versão:** 1.0

***

## Índice

* [RF001 - Sistema de Login](#rf001---sistema-de-login)

* [RF002 - Engenharia de Produto](#rf003---engenharia-de-produto)

***

## RF001 - Sistema de Login

### Informações Básicas

* **Código:** RF001

* **Nome:** Sistema de Login

* **Módulo:** AxionConnect

### Descrição Detalhada

O sistema de login do AxionConnect permite autenticação de usuários com controle de acesso baseado em credenciais. O sistema verifica automaticamente usuários já alocados no computador e permite login manual quando necessário.

### Elementos da Interface

**Campos de Entrada:**

* Campo Usuário: Campo de texto para inserir o login do usuário

* Campo Senha: Campo de senha para inserir a senha do usuário

**Botões:**

* Botão Login: Executa o processo de autenticação

**Elementos Visuais:**

* Indicador de Carregamento: Mostra progresso durante verificação automática

* Label de Status: Exibe mensagens de carregamento

### Comportamento da Interface

**Inicialização Automática:**
Ao abrir o sistema, verifica automaticamente se existe usuário alocado para o computador atual. Se encontrado, preenche automaticamente os campos e tenta fazer login.

**Login Manual:**
Quando não há usuário alocado ou o login automático falha, o usuário deve inserir manualmente suas credenciais.

**Validação de Campos:**
Todos os campos são obrigatórios e validados antes do envio.

### Fluxo de Processo

**Fluxo Automático:**

1. Sistema verifica conexão com internet
2. Busca usuário alocado no banco de dados
3. Se encontrado, preenche campos automaticamente
4. Executa login automaticamente
5. Redireciona para tela principal

**Fluxo Manual:**

1. Usuário insere login e senha
2. Sistema valida campos obrigatórios
3. Verifica credenciais no banco de dados
4. Valida se usuário está ativo
5. Cria/atualiza registro de usuário alocado
6. Carrega configurações do sistema
7. Redireciona para tela principal

### Regras de Negócio

1. **Verificação de Conectividade:** Sistema deve verificar conexão com internet antes de tentar autenticação
2. **Usuário Alocado:** Sistema mantém registro de usuários por computador/hostname para login automático
3. **Criptografia de Senha:** Senhas são criptografadas usando AES antes da validação
4. **Usuário Ativo:** Apenas usuários com status ativo podem fazer login
5. **Carregamento de Configurações:** Após login bem-sucedido, sistema carrega configurações de API, templates e sistema
6. **Sessão Única:** Sistema mantém uma instância única do formulário de login

### Validações

* Campos usuário e senha são obrigatórios

* Verificação de conectividade com internet

* Validação de credenciais no banco de dados

* Verificação de status ativo do usuário

* Validação de criptografia da senha

### Tratamento de Erros

* **Sem Internet:** Exibe toast de aviso "Sem Internet"

* **Credenciais Inválidas:** Exibe mensagem "Usuário ou Senha Inválido"

* **Usuário Inativo:** Exibe toast "Usuário Inativo!"

* **Erro de Sistema:** Captura e exibe exceções com detalhes técnicos

### Integração

* **Banco de Dados:** Conecta com MySQL através do ContextoDados

* **Configurações:** Carrega configurações de API, templates e sistema após login

* **Tela Principal:** Redireciona para FrmPrincipal após autenticação bem-sucedida

***

## RF002 - Engenharia de Produto

### Informações Básicas

* **Código:** RF002

* **Nome:** Engenharia de Produto

* **Módulo:** AxionConnect

### Descrição Detalhada

O módulo de Engenharia de Produto permite carregar, visualizar e gerenciar componentes de engenharia do ERP, com integração completa ao eDrawings para visualização 2D e 3D sincronizada e gestão de operações de fabricação. O sistema organiza as funcionalidades através de um TabControl com 4 abas específicas: Lista, Operações, Sequencia Operacional e Engenharia.

### Elementos da Interface

**Painel Superior de Dados:**

* Campo Código Engenharia: Campo numérico centralizado para inserir código do produto no ERP

* Botão Nova Engenharia: Limpa todos os dados e permite nova consulta

* Botão Salvar: Salva engenharia do produto selecionado no ERP

**TabControl Principal com 4 Abas:**

**Aba Lista:**

* Grid de Produtos: DataGridView com lista completa de componentes carregados do ERP

* Navegação sincronizada com visualização 2D e 3D

**Aba Operações:**

* Campo Operação: ComboBox com lista de operações disponíveis no sistema

* Campo Máquina: ComboBox para seleção de máquina específica

* Campo Tempo Operação: Campo para tempo padrão da operação (formato mm:ss)

* Campo Número Operadores: Campo numérico para quantidade de operadores necessários

* Botão Inserir: Adiciona operação ao produto atual

* Botões Voltar/Próximo: Navegação entre componentes

* Painel de Operações: FlowLayoutPanel exibindo operações inseridas

**Aba Sequencia Operacional:**

* Painel de Etapas: FlowLayoutPanel para visualização de etapas de consumo

* Controles dinâmicos para cada etapa de consumo

**Aba Engenharia:**

* TreeView de Produtos: Árvore hierárquica dos componentes do produto selecionado

* Visualização da estrutura completa do produto

**Painel Direito de Visualização:**

* Controle eDrawings: Visualizador 2D e 3D integrado para modelos e desenhos

* Botão Zoom: Controla zoom e ajuste de visualização (Zoom Fit/Isométrico)

* Botão Próximo Desenho: Navega entre páginas de desenhos técnicos

* Botão Desenho: Alterna entre visualização 3D e 2D

**Painel de Informações do Produto:**

* Label Nome: Nome do componente selecionado

* Label Descrição: Denominação/descrição do produto

* Label Código Produto: Código do produto no ERP

* Label Peso: Peso do componente em kg

* Label Sobremetal Largura: Dimensão de sobremetal em largura

* Label Sobremetal Comprimento: Dimensão de sobremetal em comprimento

* Label Espessura: Dimensões do material (espessura x largura x comprimento)

* Label Código/Descrição Material: Informações do material utilizado

### Comportamento da Interface

**Carregamento de Engenharia:**
Ao inserir código de engenharia e sair do campo, o sistema:

1. Valida se o código é numérico
2. Carrega assincronamente todos os componentes do ERP
3. Popula o grid de produtos e a árvore hierárquica
4. Habilita todas as funcionalidades de edição
5. Exibe mensagens de pendências se existirem

**Visualização 2D e 3D Sincronizada:**

* Integração completa com eDrawings para visualização de modelos 2D e 3D

* Navegação automática para páginas específicas de desenhos multi-folha

* Alternância entre visualização 3D (modelos) e 2D (desenhos técnicos)

* Controles de zoom e ajuste de visualização

* Background personalizado para melhor visualização

**Navegação de Componentes:**

* Botões Voltar/Próximo permitem navegação sequencial entre componentes

* Seleção no grid atualiza automaticamente:

  * Visualização 3D do componente

  * Informações do produto no painel lateral

  * TreeView com estrutura hierárquica

  * Operações associadas ao componente

**Gestão de Operações:**

* Carregamento dinâmico de operações disponíveis no sistema

* Inserção de operações com validação de campos obrigatórios

* Visualização de operações através de cards no painel

* Tempo padrão inicial de "00:01" e 1 operador

**Controle de Pendências:**

* Verificação automática de pendências críticas e não-críticas

* Exibição de mensagens informativas (Toast) para cada tipo

* Bloqueio de salvamento quando existem pendências críticas

### Fluxo de Processo

**Inicialização do Formulário:**

1. Sistema carrega lista de processos disponíveis
2. Popula ComboBox de operações com dados agrupados
3. Inicializa controle eDrawings
4. Configura valores padrão (tempo "00:01", 1 operador)
5. Limpa todos os labels informativos

**Carregamento de Engenharia:**

1. Usuário insere código de engenharia no campo centralizado
2. Sistema valida se o código é numérico válido
3. Desabilita botões e campos durante carregamento
4. Carrega componentes do ERP de forma assíncrona
5. Constrói árvore completa de componentes
6. Popula grid de produtos na aba Lista
7. Habilita funcionalidades de edição
8. Foca automaticamente no primeiro componente

**Navegação entre Componentes:**

1. Usuário seleciona componente no grid ou usa botões Voltar/Próximo
2. Sistema atualiza informações do produto nos labels
3. Carrega modelo 2D ou 3D no eDrawings
4. Navega automaticamente para página específica em desenhos multi-folha
5. Atualiza TreeView com estrutura hierárquica do componente
6. Carrega operações existentes do componente
7. Verifica e exibe pendências (críticas e não-críticas)

**Inserção de Operações:**

1. Usuário seleciona operação no ComboBox
2. Seleciona máquina correspondente
3. Define tempo de operação e número de operadores
4. Clica em Inserir para adicionar operação
5. Sistema cria card visual da operação no painel
6. Limpa campos para nova inserção

**Salvamento de Engenharia:**

1. Usuário seleciona produto no grid e clica em Salvar
2. Sistema valida se produto não é item de biblioteca
3. Verifica pendências críticas do produto e subcomponentes
4. Bloqueia salvamento se houver pendências críticas
5. Executa cadastro em thread separada com indicador de progresso
6. Percorre árvore hierárquica salvando cada componente
7. Cadastra operações associadas a cada componente
8. Atualiza cor do produto no grid (verde para sucesso)
9. Exibe mensagem de conclusão

### Regras de Negócio

**Carregamento e Validação:**

1. **Código Numérico:** Código de engenharia deve ser numérico válido
2. **Carregamento Assíncrono:** Componentes são carregados de forma assíncrona para não travar interface
3. **Validação de Tipo:** Itens de biblioteca não podem ser processados para salvamento
4. **Verificação de Pendências:** Sistema verifica pendências críticas e não-críticas antes de salvar
5. **Limpeza Automática:** Nova engenharia limpa todos os dados anteriores

**Operações e Processos:**

1. **Tempo Padrão:** Operações iniciam com tempo padrão de "00:01"
2. **Operador Padrão:** Número padrão de operadores é 1
3. **Validação de Operação:** Operação e máquina são campos obrigatórios para inserção
4. **Sequência Operacional:** Operações são numeradas sequencialmente por produto
5. **Processo Axion:** Cada operação deve ter processo correspondente no sistema Axion

**Estrutura e Hierarquia:**

1. **Hierarquia de Produtos:** Sistema mantém estrutura hierárquica completa dos componentes
2. **Árvore Completa:** TreeView exibe estrutura completa do produto selecionado
3. **Navegação Sincronizada:** Seleção no grid sincroniza com visualização 3D e informações
4. **Classificação de Componentes:** Componentes são classificados por tipo (montagem, peça, etc.)

**Visualização 3D:**

1. **Integração eDrawings:** Visualização 3D é carregada automaticamente para cada componente
2. **Detecção de Arquivos:** Sistema verifica existência de desenhos 2D (.SLDDRW) e modelos 3D (.SLDPRT/.SLDASM)
3. **Navegação de Páginas:** Desenhos multi-folha navegam automaticamente para página específica
4. **Background Personalizado:** eDrawings usa background cinza personalizado
5. **Controle de Zoom:** Zoom automático e manual disponível

**Processamento e Salvamento:**

1. **Thread Separada:** Operações pesadas executam em thread de background
2. **Indicador de Progresso:** Salvamento exibe progresso detalhado
3. **Controle de Estado:** Campos são habilitados/desabilitados conforme estado da operação
4. **Validação Hierárquica:** Pendências são verificadas em toda a estrutura do produto
5. **Atualização Visual:** Produtos salvos com sucesso ficam com cor verde no grid

**Integração ERP:**

1. **Cadastro Completo:** Sistema cadastra engenharia completa no ERP
2. **Componentes e Operações:** Salva tanto estrutura de componentes quanto operações
3. **Classificação ERP:** Componentes recebem classificação adequada no ERP
4. **Sequência Operacional:** Operações mantêm sequência correta no ERP

### Validações

**Campos Obrigatórios:**

* Código de engenharia deve ser numérico válido

* Produto deve estar selecionado no grid para salvar

* Componentes devem estar carregados antes de qualquer operação

* Operação e máquina são obrigatórios para inserir processo

**Validações de Tipo:**

* Itens de biblioteca não podem ser processados para salvamento

* Apenas componentes válidos podem receber operações

* Verificação de tipo de componente (montagem, peça, etc.)

**Validações de Pendências:**

* Verificação de pendências críticas antes do salvamento

* Validação de pendências não-críticas com avisos informativos

* Bloqueio de salvamento quando existem pendências críticas

### Tratamento de Erros

**Erros de Entrada:**

* **Código Inválido:** Limpa campo, desabilita botões e retorna foco

* **Sem Componentes:** Exibe toast "Carregar componentes primeiro"

* **Sem Produto Selecionado:** Exibe toast "Nenhum produto selecionado"

**Erros de Processamento:**

* **Item Biblioteca:** Exibe aviso "Recurso indisponível para componentes de biblioteca"

* **Pendências Críticas:** Lista detalhada de todas as pendências encontradas

* **Arquivo Não Encontrado:** Informa quando modelo 3D ou desenho não existe

* **Erro de Visualização:** Captura erros do eDrawings e continua operação

**Erros de Sistema:**

* **Erro de Carregamento:** Captura e exibe exceções durante carregamento do ERP

* **Erro de Salvamento:** Exibe detalhes de erros durante cadastro no ERP

* **Erro de Navegação:** Trata erros de navegação entre componentes

* **Processo Não Encontrado:** Avisa quando processo Axion não é localizado

### Funcionalidades das Abas do TabControl

**Aba Lista:**

* Grid completo com todos os componentes carregados do ERP

* Navegação sincronizada com visualização 2D e 3D

* Indicadores visuais de status (cores para sucesso/erro)

* Seleção de componente atualiza todas as informações

**Aba Operações:**

* ComboBox de operações carregadas dinamicamente do sistema

* ComboBox de máquinas associadas às operações

* Campos de tempo (HH:MM) e número de operadores

* Botão Inserir para adicionar operação ao componente

* Botões Voltar/Próximo para navegação entre componentes

* Painel visual (FlowLayoutPanel) exibindo operações inseridas

* Limpeza automática de campos após inserção

**Aba Sequencia Operacional:**

* Painel de etapas de consumo (FlowLayoutPanel)

* GroupBox para processos não definidos

* Controles dinâmicos para cada etapa de consumo

* Visualização de materiais e quantidades necessárias

**Aba Engenharia:**

* TreeView hierárquica do componente selecionado

* Estrutura completa expandida automaticamente

* Visualização da árvore de componentes e subcomponentes

* Sincronização com seleção no grid da aba Lista

### Integração

**Sistema ERP:**

* **Carregamento de Componentes:** Utiliza classe ProdutoErp para carregar estrutura completa do ERP

* **Cadastro de Engenharia:** Integração completa para cadastro de engenharia no ERP

* **Estrutura Hierárquica:** Mantém relacionamento pai-filho dos componentes

* **Classificação de Componentes:** Integra classificações do ERP (produto, subconjunto, peça, insumo)

**Visualização 3D:**

* **eDrawings Control:** Integração nativa com controle eDrawings da SolidWorks

* **Modelos 3D:** Carregamento automático de arquivos .SLDPRT e .SLDASM

* **Desenhos 2D:** Visualização de arquivos .SLDDRW com navegação entre páginas

* **Markup Control:** Suporte para anotações e marcações nos desenhos

**Sistema de Processos:**

* **Classe Processo:** Carrega operações e máquinas disponíveis no sistema Axion

* **Operações Dinâmicas:** ComboBox populado dinamicamente com operações agrupadas

* **Validação de Processos:** Verifica existência de processos no sistema Axion

* **Cadastro de Operações:** Integra operações cadastradas com engenharia do ERP

**Banco de Dados:**

* **ContextoDados:** Utiliza Entity Framework para operações de banco

* **Configuração API:** Carrega configurações de integração com ERP

* **Transações:** Operações de salvamento em transações para garantir integridade

* **Auditoria:** Registra operações realizadas para controle

**Interface e Controles:**

* **LmCorbieUI:** Utiliza biblioteca customizada de controles de interface

* **Threading:** Operações assíncronas para não bloquear interface

* **Toast Notifications:** Sistema de notificações não-intrusivas

* **Progress Indicators:** Indicadores de progresso para operações longas

