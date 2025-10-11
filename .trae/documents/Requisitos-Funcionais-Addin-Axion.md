# Requisitos Funcionais - Addin Axion

**Cliente:** B.Lotti\
**Sistema:** Addin Axion\
**Data:** 29/09/2025\
**Versão:** 1.0

***

## Índice

1. [RF001 - Sistema de Login](#rf001---sistema-de-login)
2. [RF002 - Cadastro de Usuários](#rf002---cadastro-de-usuários)
3. [RF003 - Cadastro de Perfis de Usuários e Permissões](#rf003---cadastro-de-perfis-de-usuários-e-permissões)
4. [RF004 - Configuração de Templates](#rf004---configuração-de-templates)
5. [RF005 - Configuração do Integrador](#rf005---configuração-do-integrador)
6. [RF006 - Cadastro de Processos](#rf006---cadastro-de-processos)
7. [RF007 - Importação de Produtos](#rf007---importação-de-produtos)

***

# RF001 - Sistema de Login

## 1. Informações Básicas

**Código:** RF001\
**Nome:** Sistema de Login\
**Módulo:** Sistema Axion\
**Formulário:** `AddinArtama\02_formularios\00_login\FrmLogin.cs`

## 2. Descrição Geral

O sistema de login é responsável por autenticar usuários no Addin Axion, oferecendo login automático por máquina e login manual. O sistema verifica conectividade, valida usuários ativos, utiliza criptografia AES para senhas e registra alocação de usuários por hostname.

## 3. Interface do Usuário Real

### 3.1 Componentes da Interface

A interface do formulário `FrmLogin.cs` apresenta os seguintes componentes:

#### 3.1.1 Campos de Entrada

* **Campo txtUsuario**:

  * Tipo: TextBox com watermark "Usuário"

  * Obrigatório: Sim

  * Validação: Não pode estar vazio

* **Campo txtSenha**:

  * Tipo: TextBox com watermark "Senha"

  * Mascaramento: Caracteres '●'

  * Obrigatório: Sim

  * Validação: Não pode estar vazio

#### 3.1.2 Botões de Ação

* **Botão "Entrar"**:

  * Tipo: Button com ícone

  * Função: Executar processo de login

  * Estado: Habilitado quando campos preenchidos

#### 3.1.3 Elementos Visuais

* **PictureBox (ptb)**:

  * Função: Exibir logo/imagem de carregamento

  * Posicionamento: Central na interface

* **Label "Carregando..." (lblCarregando)**:

  * Função: Feedback visual durante processamento

  * Visibilidade: Controlada durante autenticação

## 4. Funcionalidades Implementadas

### 4.1 Login Automático por Máquina

**Descrição**: Sistema verifica se existe usuário alocado para o hostname e usuário do PC atual

**Processo**:

1. Verifica conectividade com internet usando `VerificarConectividade()`
2. Busca usuário alocado na tabela `usuario_alocados` por hostname e usuário PC
3. Se encontrado, executa login automático sem exibir interface
4. Se não encontrado, exibe tela de login manual

**Implementação**: Método `VerificarUsuarioAlocado()` no `Form_Load`

### 4.2 Verificação de Conectividade

**Descrição**: Verifica conexão com internet antes de tentar login

**Método**: `VerificarConectividade()`

**Comportamento**:

* Testa conectividade antes de qualquer operação de login

* Exibe mensagem informativa se não houver conectividade

* Bloqueia tentativas de login sem conexão

### 4.3 Validação de Usuário Ativo

**Descrição**: Verifica se o usuário está ativo no banco de dados

**Processo**:

* Consulta campo `ativo` na tabela de usuários

* Bloqueia login se usuário inativo

* Exibe mensagem específica para usuários inativos

### 4.4 Criptografia AES

**Descrição**: Senhas são criptografadas/descriptografadas usando AES

**Implementação**:

* Classe `Criptografia` com métodos `Criptografar()`

* Senhas nunca armazenadas em texto plano

* Comparação segura entre senha informada e senha do banco

### 4.5 Alocação de Usuário

**Descrição**: Registra usuário\_pc e hostname na tabela `usuario_alocados`

**Dados registrados**:

* `usuario_pc`: Nome do usuário do Windows (`Environment.UserName`)

* `hostname`: Nome da máquina (`Environment.MachineName`)

* `id_usuario`: ID do usuário logado

**Finalidade**: Permitir login automático em acessos futuros na mesma máquina

### 4.6 Carregamento de Configurações

**Descrição**: Após login bem-sucedido, carrega configurações da API

**Configurações carregadas**:

* Token da API para integração

* Chave eDrawings para visualização

* URL base do sistema

* Código da empresa

* Processos disponíveis

* Templates do sistema

**Método**: `CarregarConfiguracoes()` após autenticação

### 4.7 Inicialização do Sistema

**Descrição**: Configura ambiente após login

**Ações executadas**:

* Define permissões do usuário logado

* Carrega rodapé do sistema

* Define variáveis de ambiente padrão:

  * Nome do sistema: "Axion Artama"

  * Pasta padrão: "LM Projetos Data"

  * Cliente: "Artama"

**Método**: `InicializarSistema()` após carregamento de configurações

## 5. Fluxos de Processo

### 5.1 Fluxo de Inicialização

1. **Verificar conectividade** com internet
2. **Buscar usuário alocado** por hostname e usuário PC
3. **Se encontrado**: executar login automático
4. **Se não encontrado**: exibir tela de login manual

### 5.2 Fluxo de Login Manual

1. **Validar preenchimento** dos campos obrigatórios
2. **Buscar usuário** no banco de dados
3. **Verificar se usuário está ativo**
4. **Validar senha** usando criptografia AES
5. **Registrar alocação** (hostname + usuário PC)
6. **Carregar configurações** da API
7. **Inicializar sistema** com permissões e variáveis
8. **Fechar formulário** de login

### 5.3 Fluxo de Login Automático

1. **Verificar conectividade**
2. **Buscar registro** na tabela `usuario_alocados`
3. **Validar usuário** encontrado
4. **Carregar configurações** automaticamente
5. **Inicializar sistema** sem exibir interface
6. **Prosseguir** para tela principal

## 6. Tratamento de Erros

### 6.1 Tipos de Tratamento

* **Toast**: Para avisos e informações gerais

* **MsgBox**: Para erros críticos que requerem atenção

* **LmException**: Para exceções específicas do sistema

* **Logs**: Registro detalhado de tentativas e erros

### 6.2 Situações de Erro

#### 6.2.1 Falta de Conectividade

* **Detecção**: Método `VerificarConectividade()`

* **Comportamento**: Exibe mensagem informativa

* **Ação**: Usuário deve verificar conexão

#### 6.2.2 Usuário Não Encontrado

* **Causa**: Usuário não existe no banco

* **Comportamento**: Exibe mensagem via Toast

* **Ação**: Verificar credenciais informadas

#### 6.2.3 Usuário Inativo

* **Causa**: Campo `ativo = false` no banco

* **Comportamento**: Exibe mensagem de usuário inativo

* **Ação**: Contatar administrador do sistema

#### 6.2.4 Senha Incorreta

* **Causa**: Senha não confere após descriptografia

* **Comportamento**: Exibe mensagem de credenciais inválidas

* **Ação**: Tentar novamente com senha correta

#### 6.2.5 Erro no Carregamento

* **Causa**: Falha ao carregar configurações ou inicializar

* **Comportamento**: Exibe erro via MsgBox ou LmException

* **Ação**: Verificar logs e conectividade

## 7. Regras de Negócio Implementadas

### 7.1 Validação de Usuário

* Usuário deve existir no banco de dados

* Usuário deve estar ativo (campo `ativo = true`)

* Credenciais não podem estar vazias

* Sistema registra tentativas de acesso

### 7.2 Validação de Senha

* Senha é validada usando criptografia AES

* Comparação entre senha informada (criptografada) e senha do banco

* Não há armazenamento de senhas em texto plano

* Sistema não implementa limite de tentativas

### 7.3 Controle de Alocação

* Sistema registra hostname e usuário do PC para cada login

* Permite login automático baseado na alocação registrada

* Tabela `usuario_alocados` controla máquinas autorizadas

* Um usuário pode estar alocado em múltiplas máquinas

### 7.4 Configurações do Sistema

**Carregamento automático após login**:

* Token API para integração com serviços externos

* Chave eDrawings para visualização de projetos

* URL base do sistema

* Código da empresa para identificação

**Variáveis padrão definidas**:

* Nome do sistema: "Axion Artama"

* Pasta de trabalho: "LM Projetos Data"

* Cliente: "Artama"

## 8. Validações e Controles Implementados

### 8.1 Validações de Interface

* Campos txtUsuario e txtSenha são obrigatórios

* Validação visual através de watermarks

* Estilo underlined para campos de entrada

* Ícones identificadores em cada campo

### 8.2 Controles de Processamento

* Label "Carregando..." durante autenticação

* PictureBox para feedback visual

* Tratamento de exceções com classes específicas

* Controle de visibilidade de elementos durante processo

### 8.3 Controles de Segurança

* Criptografia AES para senhas

* Verificação de status ativo do usuário

* Registro de alocação por máquina

* Validação de conectividade antes de operações

## 9. Integração com Outros Módulos

### 9.1 Integração com Banco de Dados

* **Consulta de usuários**: Tabela principal de usuários

* **Verificação de alocação**: Tabela `usuario_alocados`

* **Registro de nova alocação**: Após login bem-sucedido

* **Logs de acesso**: Registro de tentativas e sucessos

### 9.2 Integração com API Externa

* **Carregamento de token**: Para autenticação em serviços

* **Configuração de URL**: Base para chamadas da API

* **Sincronização de processos**: Carregamento de processos disponíveis

* **Templates do sistema**: Sincronização de templates

### 9.3 Integração com Sistema de Arquivos

* **Definição de pasta padrão**: "LM Projetos Data"

* **Configuração de caminhos**: Para templates e arquivos

* **Inicialização de variáveis**: De ambiente do sistema

### 9.4 Integração com eDrawings

* **Carregamento de chave**: De licença do eDrawings

* **Configuração para visualização**: De projetos 3D

* **Integração com módulo**: De engenharia de produto

## 10. Aspectos de Segurança

### 10.1 Criptografia

* **Algoritmo AES**: Para criptografia de senhas

* **Armazenamento seguro**: Senhas nunca em texto plano

* **Validação criptográfica**: Comparação segura de senhas

### 10.2 Controle de Acesso

* **Verificação de usuário ativo**: Antes de permitir login

* **Registro de alocação**: Para controle de máquinas

* **Carregamento de permissões**: Baseado no perfil do usuário

* **Inicialização controlada**: Do ambiente do sistema

***

# RF002 - Cadastro de Usuários

## 1. Informações Básicas

**Código:** RF002\
**Nome:** Cadastro de Usuários\
**Módulo:** Segurança e Controle de Acesso

## 2. Descrição Geral

O sistema de cadastro de usuários permite o gerenciamento completo dos usuários que terão acesso ao Addin Axion. Através de uma interface com abas, o administrador pode cadastrar novos usuários, definir suas credenciais de acesso, associar perfis de permissão e controlar o status de ativação. O sistema garante a unicidade dos logins e implementa validações de segurança para proteger as informações dos usuários.

## 3. Interface do Usuário

O cadastro de usuários é implementado através do formulário `AddinArtama\02_formularios\05_cadastros\FrmUsuarioCad.cs`. A interface é organizada em duas abas principais:

### 3.1 Aba Cadastro

* **Campo Código (txtID)**: Campo numérico com funcionalidade de busca F7 para localizar usuários existentes

* **Campo Nome (txtNome)**: Campo obrigatório para o nome completo do usuário (máximo 250 caracteres)

* **Campo Login (txtLogin)**: Campo obrigatório para o nome de usuário único (máximo 10 caracteres)

* **Campo Senha (txtSenha)**: Campo obrigatório mascarado para a senha do usuário (máximo 100 caracteres)

* **Checkbox Situação (ckbSituacao)**: Controle para ativar/desativar o usuário (padrão: Ativo)

* **Botões de Ação**: Salvar (btnSalvar), Excluir (btnExcluir), Limpar (btnLimpar)

### 3.2 Aba Perfil

* **Grid de Perfis (dgvPerfil)**: Exibe os perfis associados ao usuário

* **Grid de Permissões (dgvPermis)**: Mostra as permissões detalhadas dos perfis selecionados

* **Botões de Gerenciamento**: Selecionar (btnSelPerfil), Alterar (btnAltPerfil), Perfil (btnAddPerfil)

### 3.3 Navegação e Atalhos

* **F7**: Pesquisar usuários existentes no campo código

* **F8**: Navegar para o item anterior

* **F9**: Navegar para o próximo item

## 4. Funcionalidades

### 4.1 Cadastro de Usuário

* Inserção de novos usuários com validação de campos obrigatórios

* Geração automática de código sequencial

* Validação de unicidade do login através do método `txtLogin_Leave`

* Criptografia automática da senha

* Definição do status inicial como ativo

### 4.2 Edição de Usuário

* Busca de usuários existentes através do código (F7) via método `TxtID_ButtonClickF7`

* Carregamento automático dos dados através do método `CarregarPerfil`

* Alteração de dados pessoais e credenciais

* Controle de ativação/desativação

### 4.3 Exclusão de Usuário

* Exclusão através do método `BtnExcluir_Click`

* Validação de dependências antes da exclusão

* Confirmação de segurança

### 4.4 Gerenciamento de Perfis

* Associação de múltiplos perfis ao usuário

* Visualização das permissões resultantes

* Alteração dinâmica de perfis através dos botões da aba Perfil

## 5. Regras de Negócio

### 5.1 Validação de Dados

* **Nome**: Campo obrigatório (CampoObrigatorio = true), máximo 250 caracteres

* **Login**: Campo obrigatório, único no sistema, máximo 10 caracteres

* **Senha**: Campo obrigatório, máximo 100 caracteres, mascarada com PasswordChar

* **Código**: Gerado automaticamente, tipo numérico inteiro

### 5.2 Controle de Acesso

* Apenas usuários com perfil administrativo podem cadastrar usuários

* Usuários não podem alterar seus próprios perfis de permissão

* Usuários inativos não podem acessar o sistema

### 5.3 Segurança

* Senhas são mascaradas na interface (UseSystemPasswordChar = true)

* Validação de unicidade do login em tempo real

* Auditoria de alterações nos dados do usuário

## 6. Fluxos de Processo

### 6.1 Cadastro de Novo Usuário

1. Usuário acessa a tela de cadastro
2. Sistema gera código automático
3. Usuário preenche dados obrigatórios (Nome, Login, Senha)
4. Sistema valida unicidade do login via `txtLogin_Leave`
5. Usuário define status (Ativo/Inativo)
6. Sistema salva o registro via `BtnSalvar_Click`
7. Usuário pode associar perfis na aba Perfil

### 6.2 Edição de Usuário Existente

1. Usuário busca registro pelo código (F7) via `TxtID_ButtonClickF7`
2. Sistema carrega dados do usuário via `CarregarPerfil`
3. Usuário altera informações necessárias
4. Sistema valida alterações
5. Sistema salva modificações
6. Sistema registra auditoria da alteração

### 6.3 Associação de Perfis

1. Usuário acessa aba Perfil
2. Sistema exibe perfis disponíveis no grid dgvPerfil
3. Usuário seleciona perfis através dos botões de gerenciamento
4. Sistema atualiza permissões do usuário
5. Sistema exibe permissões resultantes no grid dgvPermis

## 7. Situações de Erro

* **Login já existe**: Validação através do método `txtLogin_Leave` com exibição de mensagem

* **Campos obrigatórios vazios**: Destacar campos obrigatórios e impedir salvamento

* **Usuário não encontrado**: Exibir mensagem informativa na busca F7

* **Erro de conexão**: Exibir mensagem de erro de sistema

## 8. Validações e Controles

* Validação de formato de dados em tempo real

* Controle de tamanho máximo dos campos (MaxLength)

* Verificação de unicidade do login

* Validação de campos obrigatórios (CampoObrigatorio)

* Controle de caracteres especiais na senha

## 9. Integração com Outros Módulos

* **Sistema de Login**: Validação de credenciais cadastradas

* **Sistema de Perfis**: Associação de permissões através da aba Perfil

* **Banco de Dados**: Persistência das informações

* **Sistema de Auditoria**: Registro de alterações

* **Sistema de Segurança**: Criptografia e validações

***

# RF003 - Cadastro de Perfis de Usuários e Permissões

## 1. Informações Básicas

**Código:** RF003\
**Nome:** Cadastro de Perfis de Usuários e Permissões\
**Módulo:** Administração

## 2. Descrição Geral

O módulo de Cadastro de Perfis de Usuários e Permissões permite a criação, edição e gerenciamento de perfis de usuários no sistema Addin Axion. Controla o acesso às funcionalidades através de um sistema granular de permissões, garantindo que cada usuário tenha acesso apenas às funcionalidades necessárias para seu trabalho.

## 3. Interface do Usuário

### 3.1 Formulário de Cadastro de Perfil

O formulário de cadastro de perfil segue o padrão de tela de cadastro do sistema, apresentando uma interface intuitiva e organizada para gerenciamento de perfis de usuários.

#### 3.1.1 Campos de Entrada

* **Campo Código (txtID)**:

  * Campo numérico para identificação única do perfil

  * Possui botão F7 integrado para pesquisa de perfis existentes

  * Quando em modo de edição, o campo fica somente leitura

  * Suporte a navegação com F8 (anterior) e F9 (próximo)

  * Tooltip informativo: "Pesquisar \[F7]", "Item Anterior \[F8]", "Próximo Item \[F9]"

* **Campo Descrição (txtDescricao)**:

  * Campo de texto obrigatório (marcado com asterisco)

  * Limite máximo de 80 caracteres

  * Utilizado para identificação do perfil no sistema

  * Validação obrigatória antes do salvamento

* **Campo Observação (txtObservacao)**:

  * Campo de texto multiline para informações adicionais

  * Limite máximo de 250 caracteres

  * Possui barras de rolagem horizontal e vertical

  * Campo opcional para detalhamento do perfil

#### 3.1.2 Botões de Ação

Os botões seguem o padrão visual do sistema com bordas arredondadas e ícones identificadores:

* **Botão Salvar**:

  * Executa validação dos campos obrigatórios

  * Salva o perfil no banco de dados

  * Após salvamento bem-sucedido, habilita botões Excluir e Definir Permissões

  * Atualiza o campo código com o ID gerado

* **Botão Excluir**:

  * Inicialmente desabilitado para novos registros

  * Habilitado apenas após salvamento do perfil

  * Verifica se existem usuários associados antes da exclusão

  * Exibe lista de usuários vinculados caso existam

  * Solicita confirmação antes da exclusão definitiva

* **Botão Limpar**:

  * Limpa todos os campos do formulário

  * Reabilita o campo código para edição

  * Remove a lista de usuários associados

  * Desabilita botões Excluir e Definir Permissões

  * Retorna o foco para o campo código

* **Botão Definir Permissões**:

  * Inicialmente desabilitado para novos registros

  * Habilitado apenas após salvamento do perfil

  * Abre tela específica para configuração de permissões do perfil

  * Requer que o perfil esteja salvo antes da configuração

### 3.2 Seção de Usuários

#### 3.2.1 FlowLayoutPanel de Usuários (flpUsuarios)

* **Função**: Exibe lista de usuários associados ao perfil

* **Layout**: Organização em fluxo horizontal com quebra automática

* **Conteúdo**: Cards ou labels com nomes dos usuários vinculados

* **Atualização**: Automática após salvamento ou carregamento de perfil

* **Interação**: Permite visualização rápida dos usuários do perfil

### 3.3 Navegação e Pesquisa

#### 3.3.1 Funcionalidade F7 - Pesquisa

* **Ativação**: Tecla F7 no campo código ou clique no botão integrado

* **Comportamento**: Abre formulário de consulta geral (`FrmConsultaGeral`)

* **Filtros**: Permite busca por código ou descrição

* **Seleção**: Retorna dados do perfil selecionado

* **Carregamento**: Preenche automaticamente todos os campos do formulário

#### 3.3.2 Navegação F8/F9

* **F8 (Anterior)**: Carrega o perfil anterior baseado no código

* **F9 (Próximo)**: Carrega o próximo perfil baseado no código

* **Comportamento**: Navegação sequencial pelos registros

* **Validação**: Verifica existência do registro antes de carregar

### 3.4 Estados da Interface

#### 3.4.1 Modo Novo Registro

* Campo código habilitado para edição

* Campos descrição e observação limpos

* Botões Excluir e Definir Permissões desabilitados

* Botão Salvar habilitado

* FlowLayoutPanel de usuários vazio

#### 3.4.2 Modo Edição

* Campo código em modo somente leitura

* Campos preenchidos com dados do perfil

* Todos os botões habilitados

* FlowLayoutPanel populado com usuários associados

#### 3.4.3 Modo Visualização

* Todos os campos preenchidos

* Navegação F8/F9 disponível

* Botões de ação disponíveis conforme estado do registro

### 3.5 Interface do Usuário

O cadastro de perfis de usuários e permissões será implementado através do formulário `AddinArtama\02_formularios\05_cadastros\FrmPerfil.cs`. A tela de cadastro permitirá a inserção e edição de perfis. Para visualizar o grid de perfis existentes, o usuário deverá clicar na tecla F7 no campo de código, que abrirá uma janela de consulta com todos os registros.

**Campos da Interface:**

* **Código**: Campo numérico com funcionalidade F7 para pesquisa

* **Descrição**: Campo de texto obrigatório para nome do perfil

* **Observação**: Campo de texto opcional para detalhes adicionais

**Botões de Ação:**

* **Salvar**: Grava o perfil no banco de dados

* **Excluir**: Remove o perfil (verifica dependências)

* **Limpar**: Limpa os campos para novo cadastro

* **Definir Permissões**: Abre tela de configuração de permissões

**Seção Usuários:**

* **FlowLayoutPanel**: Exibe usuários associados ao perfil

**Navegação:**

* **F7**: Abre consulta de perfis existentes

* **F8**: Navega para perfil anterior

* **F9**: Navega para próximo perfil

## 4. Funcionalidades

### 4.1 Cadastro de Perfil

#### 4.1.1 Criação de Novo Perfil

**Processo:**

1. Usuário preenche campo descrição (obrigatório)
2. Opcionalmente preenche campo observação
3. Clica em Salvar
4. Sistema valida campos obrigatórios
5. Sistema gera código único automaticamente
6. Perfil é salvo no banco de dados
7. Botões Excluir e Definir Permissões são habilitados

**Validações:**

* Campo descrição não pode estar vazio

* Descrição deve ter no máximo 80 caracteres

* Observação deve ter no máximo 250 caracteres

#### 4.1.2 Edição de Perfil Existente

**Processo:**

1. Usuário localiza perfil via F7 ou navegação F8/F9
2. Sistema carrega dados nos campos
3. Usuário modifica informações desejadas
4. Clica em Salvar
5. Sistema atualiza registro no banco
6. Lista de usuários é atualizada

### 4.2 Pesquisa e Navegação

#### 4.2.1 Pesquisa por F7

**Funcionalidade:**

* Abre formulário `FrmConsultaGeral`

* Permite busca por código ou descrição

* Exibe grid com todos os perfis cadastrados

* Permite seleção e retorno dos dados

**Implementação:**

* Método `TxtID_ButtonClickF7` no formulário

* Integração com sistema de consulta geral

* Carregamento automático dos dados selecionados

#### 4.2.2 Navegação Sequencial

**F8 - Perfil Anterior:**

* Carrega perfil com código imediatamente anterior

* Verifica existência antes de carregar

* Atualiza todos os campos e lista de usuários

**F9 - Próximo Perfil:**

* Carrega perfil com código imediatamente posterior

* Verifica existência antes de carregar

* Atualiza todos os campos e lista de usuários

### 4.3 Gerenciamento de Permissões

#### 4.3.1 Definição de Permissões

**Acesso:**

* Botão "Definir Permissões" habilitado após salvar perfil

* Requer perfil salvo no banco de dados

**Funcionalidade:**

* Abre tela específica para configuração de permissões

* Permite definir acesso granular às funcionalidades

* Salva configurações associadas ao perfil

#### 4.3.2 Tipos de Permissões

**Módulos do Sistema:**

* Acesso a diferentes módulos do Addin Axion

* Controle de visualização e edição

* Permissões específicas por funcionalidade

**Níveis de Acesso:**

* Leitura: Apenas visualização

* Escrita: Criação e edição

* Exclusão: Remoção de registros

* Administração: Controle total

### 4.4 Associação de Usuários

#### 4.4.1 Visualização de Usuários

**FlowLayoutPanel:**

* Exibe usuários associados ao perfil atual

* Atualização automática ao carregar perfil

* Layout responsivo com quebra de linha

**Informações Exibidas:**

* Nome do usuário

* Status (ativo/inativo)

* Data de associação

#### 4.4.2 Gerenciamento de Associações

**Associação:**

* Realizada através do cadastro de usuários

* Usuário seleciona perfil durante cadastro

* Associação é salva na tabela de usuários

**Desassociação:**

* Alteração do perfil do usuário

* Remoção automática da lista

* Atualização em tempo real

### 4.5 Exclusão de Perfil

#### 4.5.1 Verificação de Dependências

**Processo:**

1. Usuário clica em Excluir
2. Sistema verifica usuários associados
3. Se existem usuários: exibe lista e bloqueia exclusão
4. Se não existem usuários: solicita confirmação
5. Após confirmação: remove perfil do banco

**Validações:**

* Perfil não pode ser excluído se possui usuários associados

* Sistema exibe lista de usuários vinculados

* Usuário deve primeiro desassociar usuários

#### 4.5.2 Exclusão Segura

**Confirmação:**

* Dialog de confirmação antes da exclusão

* Informações sobre impacto da exclusão

* Opção de cancelar operação

**Processo:**

* Remove permissões associadas

* Remove perfil da base de dados

* Limpa formulário após exclusão

* Retorna ao modo de novo registro

## 5. Regras de Negócio

### 5.1 Validação de Dados

#### 5.1.1 Campos Obrigatórios

* **Descrição**: Campo obrigatório para identificação

* **Validação**: Não pode estar vazio ou apenas espaços

* **Feedback**: Destaque visual em caso de erro

#### 5.1.2 Limites de Caracteres

* **Descrição**: Máximo 80 caracteres

* **Observação**: Máximo 250 caracteres

* **Validação**: Controle durante digitação

* **Feedback**: Contador de caracteres (opcional)

### 5.2 Controle de Acesso

#### 5.2.1 Permissões para Cadastro

* Apenas usuários com perfil administrativo

* Controle de acesso ao formulário

* Auditoria de alterações realizadas

#### 5.2.2 Hierarquia de Perfis

* Perfis podem ter diferentes níveis de privilégio

* Administradores têm acesso total

* Usuários comuns têm acesso limitado

### 5.3 Integridade de Dados

#### 5.3.1 Consistência

* Perfil não pode ser excluído se possui usuários

* Verificação de integridade antes de operações

* Rollback em caso de erro durante salvamento

## 6. Validações

### 6.1 Validações de Interface

#### 6.1.1 Campos Obrigatórios

* Validação em tempo real

* Destaque visual para campos vazios

* Bloqueio de salvamento se inválido

#### 6.1.2 Formato de Dados

* Código: Apenas números

* Descrição: Texto livre, sem caracteres especiais

* Observação: Texto livre

### 6.2 Validações de Negócio

#### 6.2.1 Unicidade

* Descrição deve ser única no sistema

* Verificação antes do salvamento

* Mensagem de erro específica

#### 6.2.2 Dependências

* Verificação de usuários associados

* Bloqueio de exclusão se existem dependências

* Listagem de dependências encontradas

### 6.3 Validações de Segurança

#### 6.3.1 Autorização

* Verificação de permissão para acessar formulário

* Controle de operações por perfil do usuário

* Log de tentativas de acesso não autorizado

#### 6.3.2 Integridade

* Validação de dados antes de salvar

* Verificação de consistência com banco

* Prevenção de corrupção de dados

## 7. Interface e Usabilidade

### 7.1 Design da Interface

#### 7.1.1 Layout

* Interface limpa e organizada

* Campos agrupados logicamente

* Botões com ícones identificadores

* Cores consistentes com padrão do sistema

#### 7.1.2 Responsividade

* Adaptação a diferentes resoluções

* Redimensionamento adequado dos componentes

* Manutenção da usabilidade em telas menores

### 7.2 Experiência do Usuário

#### 7.2.1 Navegação

* Atalhos de teclado intuitivos (F7, F8, F9)

* Tab order lógica entre campos

* Enter para confirmar ações

* Escape para cancelar operações

#### 7.2.2 Feedback

* Mensagens claras de sucesso e erro

* Indicadores visuais de campos obrigatórios

* Confirmações para operações críticas

* Loading indicators durante processamento

### 7.3 Acessibilidade

#### 7.3.1 Suporte a Teclado

* Navegação completa via teclado

* Atalhos para funções principais

* Indicadores visuais de foco

#### 7.3.2 Tooltips e Ajuda

* Tooltips informativos em campos e botões

* Mensagens de ajuda contextual

* Documentação integrada

***

# RF004 - Configuração de Templates

## 1. Informações Básicas

**Código:** RF004\
**Nome:** Configuração de Templates\
**Módulo:** Configuração

## 2. Descrição Geral

O módulo de Configuração de Templates permite definir e gerenciar os caminhos dos templates de desenho do SolidWorks para diferentes formatos de folha (A4R, A4P, A3, A2, A1, A0) e configurar os templates para listas de material de montagem e soldagem.

## 3. Interface do Usuário

### 3.1 Elementos da Interface

#### 3.1.1 Tela de Configuração

* **Modo de Exibição**: Formulário maximizado

* **Título**: "Configuração de Templates"

* **Layout**: Organizado em dois grupos principais

#### 3.1.2 Grupo "Formatos de Folha"

* **txtA4R**: Campo para template A4 Retrato (.drwdot)

  * Botão F7 para seleção de arquivo

  * Campo somente leitura

  * Exibe caminho completo do template

* **txtA4P**: Campo para template A4 Paisagem (.drwdot)

  * Botão F7 para seleção de arquivo

  * Campo somente leitura

  * Exibe caminho completo do template

* **txtA3**: Campo para template A3 (.drwdot)

  * Botão F7 para seleção de arquivo

  * Campo somente leitura

  * Exibe caminho completo do template

* **txtA2**: Campo para template A2 (.drwdot)

  * Botão F7 para seleção de arquivo

  * Campo somente leitura

  * Exibe caminho completo do template

* **txtA1**: Campo para template A1 (.drwdot)

  * Botão F7 para seleção de arquivo

  * Campo somente leitura

  * Exibe caminho completo do template

* **txtA0**: Campo para template A0 (.drwdot)

  * Botão F7 para seleção de arquivo

  * Campo somente leitura

  * Exibe caminho completo do template

#### 3.1.3 Grupo "Listas de Material"

* **txtListaMotagem**: Campo para template de lista de montagem (.sldbomtbt)

  * Botão F7 para seleção de arquivo

  * Campo somente leitura

  * Exibe caminho completo do template

* **txtListaSoldagem**: Campo para template de lista de soldagem (.sldwldtbt)

  * Botão F7 para seleção de arquivo

  * Campo somente leitura

  * Exibe caminho completo do template

#### 3.1.4 Botões de Ação

* **btnSalvar**: Salva as configurações no banco de dados

  * Ícone de disquete

  * Atalho: Ctrl+S

  * Validação antes do salvamento

### 3.2 Comportamentos da Interface

#### 3.2.1 Carregamento Inicial

* Carregamento automático das configurações existentes

* Preenchimento dos campos com caminhos salvos

* Verificação da existência dos arquivos

* Indicação visual para arquivos não encontrados

#### 3.2.2 Seleção de Arquivos (F7)

* Abertura de diálogo para seleção de arquivo

* Filtros específicos por tipo:

  * Templates de desenho: \*.drwdot

  * Templates de lista de montagem: \*.sldbomtbt

  * Templates de lista de soldagem: \*.sldwldtbt

* Validação do tipo de arquivo selecionado

* Atualização automática do campo correspondente

#### 3.2.3 Validações

* Verificação da existência dos arquivos selecionados

* Validação da extensão dos arquivos

* Verificação de permissões de acesso aos arquivos

* Feedback visual para campos inválidos

## 4. Funcionalidades

### 4.1 Carregamento de Configurações

#### 4.1.1 Inicialização

* Carregamento automático ao abrir o formulário

* Busca no banco de dados pelas configurações salvas

* Preenchimento dos campos com os valores encontrados

* Tratamento de configurações não encontradas

#### 4.1.2 Validação de Arquivos

* Verificação da existência física dos arquivos

* Validação das extensões esperadas

* Indicação visual para arquivos não encontrados

### 4.2 Seleção de Templates

#### 4.2.1 Diálogo de Seleção

* Abertura de FileDialog específico para cada tipo

* Filtros apropriados para cada categoria de template

* Navegação facilitada para pastas de templates

* Pré-seleção da pasta de templates do SolidWorks

#### 4.2.2 Validação de Seleção

* Verificação do tipo de arquivo selecionado

* Validação da extensão do arquivo

* Confirmação de acesso ao arquivo

* Atualização do campo correspondente

### 4.3 Salvamento de Configurações

#### 4.3.1 Validação Geral

* Verificação de todos os campos obrigatórios

* Validação da existência dos arquivos

* Verificação de permissões de acesso

* Confirmação antes do salvamento

#### 4.3.2 Persistência

* Salvamento no banco de dados

* Atualização de configurações existentes

* Criação de novas configurações se necessário

* Confirmação de sucesso do salvamento

### 4.4 Integração com SolidWorks

#### 4.4.1 Configuração de Caminhos

* Definição dos caminhos de templates no SolidWorks

* Sincronização com configurações do sistema

* Aplicação automática das configurações

* Verificação de compatibilidade

#### 4.4.2 Validação de Templates

* Verificação da compatibilidade com versão do SolidWorks

* Validação da estrutura dos templates

* Teste de abertura dos arquivos

* Relatório de problemas encontrados

## 5. Regras de Negócio

### 5.1 Validações de Arquivo

#### 5.1.1 Extensões Permitidas

* Templates de desenho: .drwdot

* Templates de lista de montagem: .sldbomtbt

* Templates de lista de soldagem: .sldwldtbt

* Rejeição de outros tipos de arquivo

#### 5.1.2 Existência de Arquivos

* Todos os arquivos devem existir fisicamente

* Verificação de permissões de leitura

* Validação de integridade dos arquivos

* Tratamento de arquivos corrompidos

### 5.2 Configurações Obrigatórias

#### 5.2.1 Templates Essenciais

* Pelo menos um formato de folha deve ser configurado

* Templates de lista são opcionais

* Configuração padrão para A4R se não especificado

* Validação de configuração mínima

#### 5.2.2 Consistência

* Todos os caminhos devem ser válidos

* Arquivos devem ser acessíveis

* Configurações devem ser compatíveis

* Sincronização com SolidWorks

### 5.3 Segurança e Acesso

#### 5.3.1 Permissões

* Apenas usuários administrativos podem configurar

* Controle de acesso ao formulário

* Auditoria de alterações realizadas

* Log de configurações modificadas

#### 5.3.2 Backup de Configurações

* Backup automático antes de alterações

* Possibilidade de restaurar configurações anteriores

* Histórico de modificações

* Recuperação em caso de erro

## 6. Fluxos de Processo

### 6.1 Configuração Inicial

1. **Abrir Formulário**: O sistema abre o formulário de configuração de templates
2. **Carregar Configurações Existentes**: Busca no banco de dados as configurações previamente salvas
3. **Verificar Configurações**: Se configurações são encontradas, preenche os campos; caso contrário, deixa os campos vazios
4. **Validar Arquivos**: Verifica a existência física dos arquivos de template configurados
5. **Exibir Interface**: Apresenta a interface ao usuário com os campos preenchidos ou vazios
6. **Aguardar Ação do Usuário**: Sistema fica disponível para interação do usuário

### 6.2 Seleção de Template

1. **Clicar F7**: Usuário pressiona F7 ou clica no botão de seleção de arquivo
2. **Abrir FileDialog**: Sistema abre diálogo de seleção de arquivos
3. **Aplicar Filtros**: Filtros específicos são aplicados conforme o tipo de template (\*.drwdot, \*.sldbomtbt, \*.sldwldtbt)
4. **Usuário Seleciona Arquivo**: Usuário navega e seleciona o arquivo desejado
5. **Validar Arquivo**: Sistema verifica se o arquivo possui extensão válida
6. **Processar Resultado**: Se válido, atualiza o campo; se inválido, exibe erro e retorna ao diálogo
7. **Validar Existência**: Confirma se o arquivo selecionado existe fisicamente
8. **Finalizar Seleção**: Se existe, confirma a seleção; se não existe, exibe aviso mas mantém a seleção
9. **Campo Atualizado**: Campo correspondente é atualizado com o caminho do arquivo

### 6.3 Salvamento de Configurações

1. **Clicar Salvar**: Usuário aciona o botão de salvamento
2. **Validar Todos os Campos**: Sistema verifica a validade de todos os caminhos de template configurados
3. **Verificar Validação**: Se a validação falha, exibe erros e permite correção; se passa, prossegue
4. **Confirmar Salvamento**: Sistema solicita confirmação do usuário para salvar as configurações
5. **Processar Confirmação**: Se usuário cancela, retorna à tela; se confirma, prossegue com salvamento
6. **Salvar no Banco**: Persiste as configurações no banco de dados
7. **Verificar Salvamento**: Se salvamento falha, exibe erro e retorna; se sucesso, prossegue
8. **Exibir Sucesso**: Confirma ao usuário que as configurações foram salvas
9. **Atualizar SolidWorks**: Aplica as novas configurações no SolidWorks
10. **Configuração Concluída**: Processo finalizado com sucesso

## 7. Tratamento de Erros

### 7.1 Erros de Arquivo

#### 7.1.1 Arquivo Não Encontrado

* **Situação**: Template selecionado não existe

* **Ação**: Exibir mensagem de erro

* **Solução**: Permitir nova seleção

* **Prevenção**: Validação em tempo real

#### 7.1.2 Extensão Inválida

* **Situação**: Arquivo com extensão incorreta

* **Ação**: Rejeitar seleção

* **Solução**: Exibir tipos aceitos

* **Prevenção**: Filtros no FileDialog

#### 7.1.3 Sem Permissão de Acesso

* **Situação**: Arquivo sem permissão de leitura

* **Ação**: Exibir erro de acesso

* **Solução**: Orientar sobre permissões

* **Prevenção**: Verificação prévia

### 7.2 Erros de Banco de Dados

#### 7.2.1 Falha na Conexão

* **Situação**: Não consegue conectar ao banco

* **Ação**: Exibir erro de conexão

* **Solução**: Verificar configuração

* **Prevenção**: Teste de conexão inicial

#### 7.2.2 Erro no Salvamento

* **Situação**: Falha ao salvar configurações

* **Ação**: Rollback da transação

* **Solução**: Tentar novamente

* **Prevenção**: Validação prévia

### 7.3 Erros de Integração

#### 7.3.1 SolidWorks Não Disponível

* **Situação**: SolidWorks não está executando

* **Ação**: Exibir aviso

* **Solução**: Aplicar na próxima execução

* **Prevenção**: Verificação de disponibilidade

#### 7.3.2 Template Incompatível

* **Situação**: Template não compatível com versão

* **Ação**: Exibir aviso de incompatibilidade

* **Solução**: Sugerir atualização do template

* **Prevenção**: Verificação de versão

## 8. Integração com Outros Módulos

### 8.1 Sistema de Banco de Dados

#### 8.1.1 Tabelas Utilizadas

* **ConfigTemplates**: Armazena caminhos dos templates

* **Campos**: A4R, A4P, A3, A2, A1, A0, ListaMotagem, ListaSoldagem

* **Chaves**: ID do usuário ou configuração global

* **Auditoria**: Data/hora da última modificação

#### 8.1.2 Operações

* **SELECT**: Carregamento de configurações existentes

* **INSERT/UPDATE**: Salvamento de novas configurações

* **Validação**: Verificação de integridade dos dados

* **Backup**: Cópia de segurança antes de alterações

### 8.2 Integração com SolidWorks

#### 8.2.1 API do SolidWorks

* Configuração de caminhos de templates

* Aplicação automática das configurações

* Sincronização com preferências do usuário

* Validação de compatibilidade

#### 8.2.2 Configurações Aplicadas

* Caminhos de templates de desenho

* Templates de listas de material

* Configurações de formato de folha

* Preferências de usuário

### 8.3 Sistema de Segurança

#### 8.3.1 Validação de Dados

* Verificação de integridade dos caminhos

* Validação de existência dos arquivos

* Controle de tipos de arquivo permitidos

* Prevenção de configurações inválidas

***

# RF005 - Configuração do Integrador

## 1. Informações Básicas

**Código:** RF005\
**Nome:** Configuração do Integrador\
**Módulo:** Sistema Axion\
**Formulário:** `AddinArtama\02_formularios\07_integrador\FrmConfigIntegrador.cs`

## 2. Descrição Geral

O sistema de configuração do integrador é responsável por gerenciar as configurações necessárias para integração com sistemas externos, incluindo configurações de API, máscaras de nomenclatura e dados padrão de produtos. O sistema permite configurar parâmetros de conexão, tokens de autenticação, máscaras para peças e conjuntos, além de dados fiscais e contábeis dos produtos.

## 3. Interface do Usuário Real

### 3.1 Componentes da Interface

A interface do formulário `FrmConfigIntegrador.cs` é organizada em três grupos principais:

#### 3.1.1 Grupo API (lmGroupBox3)

* **Campo txtEndereco**:

  * Tipo: LmTextBox

  * Label: "Endereço"

  * Função: URL base da API de integração

  * Obrigatório: Sim

* **Campo txtToken**:

  * Tipo: LmTextBox

  * Label: "Token Consistem"

  * Função: Token de autenticação da API

  * Obrigatório: Sim

  * Segurança: Campo sensível

* **Campo txtCodEmpresa**:

  * Tipo: LmTextBox

  * Label: "Código Empresa"

  * Função: Identificador da empresa no sistema externo

  * Obrigatório: Sim

* **Campo txtEDrawings**:

  * Tipo: LmTextBox

  * Label: "Chave eDrawings"

  * Função: Chave de licença para visualização eDrawings

  * Obrigatório: Não

#### 3.1.2 Grupo Máscara (lmGroupBox1)

* **Campo lmTextBox1**:

  * Tipo: LmTextBox

  * Label: "Máscara Peça"

  * Função: Padrão de nomenclatura para peças individuais

  * Exemplo: "PC-{CODIGO}-{REVISAO}"

  * Obrigatório: Sim

* **Campo lmTextBox2**:

  * Tipo: LmTextBox

  * Label: "Máscara Conjunto"

  * Função: Padrão de nomenclatura para conjuntos/montagens

  * Exemplo: "CJ-{CODIGO}-{REVISAO}"

  * Obrigatório: Sim

#### 3.1.3 Grupo Dados do Produto (lmGroupBox2)

* **Campo txtClassificacaoFiscal**:

  * Tipo: LmTextBox com F7

  * Label: "Classificação Fiscal"

  * Função: NCM padrão dos produtos

  * Busca: F7 para seleção de NCM

* **Campo txtOrigem**:

  * Tipo: LmTextBox com F7

  * Label: "Origem"

  * Função: Origem fiscal do produto (0-Nacional, 1-Importado, etc.)

  * Busca: F7 para seleção de origem

* **Campo txtFinalidade**:

  * Tipo: LmTextBox com F7

  * Label: "Finalidade"

  * Função: Finalidade do material no processo produtivo

  * Busca: F7 para seleção de finalidade

* **Campo txtControleSaida**:

  * Tipo: LmTextBox com F7

  * Label: "Controle Saída"

  * Função: Tipo de controle para saída de materiais

  * Busca: F7 para seleção de controle

* **Campo txtIpiPerc**:

  * Tipo: LmTextBox com F7

  * Label: "% IPI"

  * Função: Percentual de IPI padrão

  * Formato: Numérico com decimais

  * Busca: F7 para seleção de alíquota

* **Campo txtLocalizEntrada**:

  * Tipo: LmTextBox com F7

  * Label: "Localização Entrada"

  * Função: Local padrão para entrada de materiais

  * Busca: F7 para seleção de localização

* **Campo txtLocalizSaida**:

  * Tipo: LmTextBox com F7

  * Label: "Localização Saída"

  * Função: Local padrão para saída de materiais

  * Busca: F7 para seleção de localização

* **Campo txtContaContabil**:

  * Tipo: LmTextBox com F7

  * Label: "Conta Contábil"

  * Função: Conta contábil padrão para produtos

  * Busca: F7 para seleção de conta

#### 3.1.4 Botões de Ação

* **Botão btnSalvar**:

  * Tipo: Button

  * Função: Salvar todas as configurações

  * Estado: Habilitado após validação

## 4. Funcionalidades Implementadas

### 4.1 Carregamento Automático de Configurações

**Descrição**: Sistema carrega automaticamente as configurações existentes ao abrir o formulário

**Processo**:

1. Método `FrmConfigIntegrador_Loaded()` é executado na inicialização
2. Busca configurações na tabela `configuracao_api`
3. Preenche todos os campos com valores salvos
4. Se não existem configurações, campos ficam vazios

**Implementação**: Carregamento via `configuracao_api.CarregarConfiguracoes()`

### 4.2 Validação de Dados

**Descrição**: Validação completa dos dados antes do salvamento

**Método**: `ValidarDados()`

**Validações realizadas**:

* Campos obrigatórios não podem estar vazios

* Formato de URL válido para endereço da API

* Token deve ter formato válido

* Código da empresa deve ser numérico

* Máscaras devem conter placeholders válidos

* Percentual de IPI deve ser numérico

* Códigos fiscais devem ter formato NCM válido

### 4.3 Salvamento de Configurações

**Descrição**: Persiste todas as configurações no banco de dados

**Processo**:

1. Método `BtnSalvar_Click()` é executado
2. Chama `ValidarDados()` para verificar integridade
3. Se validação passa, salva no banco via `configuracao_api`
4. Atualiza variáveis globais da API
5. Exibe confirmação de sucesso

**Implementação**: Salvamento via `configuracao_api.SalvarConfiguracoes()`

### 4.4 Busca com F7

**Descrição**: Funcionalidade de busca avançada para campos específicos

**Campos com F7**:

* Classificação Fiscal: Busca NCM

* Origem: Busca códigos de origem

* Finalidade: Busca tipos de finalidade

* Controle Saída: Busca tipos de controle

* % IPI: Busca alíquotas cadastradas

* Localização Entrada/Saída: Busca localizações

* Conta Contábil: Busca plano de contas

**Implementação**: Eventos `ButtonClickF7` específicos para cada campo

### 4.5 Atualização de Variáveis da API

**Descrição**: Após salvamento, atualiza variáveis globais do sistema

**Variáveis atualizadas**:

* URL base da API

* Token de autenticação

* Código da empresa

* Chave eDrawings

* Máscaras de nomenclatura

* Dados padrão de produtos

**Finalidade**: Sincronizar configurações com outros módulos do sistema

## 5. Regras de Negócio

### 5.1 Validação de Campos Obrigatórios

#### 5.1.1 Campos API

* Endereço da API é obrigatório

* Token Consistem é obrigatório

* Código da Empresa é obrigatório

* Chave eDrawings é opcional

#### 5.1.2 Campos Máscara

* Máscara Peça é obrigatória

* Máscara Conjunto é obrigatória

* Máscaras devem conter placeholders válidos

#### 5.1.3 Campos Dados do Produto

* Todos os campos são opcionais

* Se preenchidos, devem ter formato válido

* Valores servem como padrão para novos produtos

### 5.2 Formato de Dados

#### 5.2.1 URL da API

* Deve começar com http\:// ou https\://

* Deve ser uma URL válida

* Deve estar acessível para teste de conectividade

#### 5.2.2 Token de Autenticação

* Formato específico conforme API externa

* Campo sensível, não deve ser exibido em logs

* Deve ser válido para autenticação

#### 5.2.3 Máscaras de Nomenclatura

* Devem conter placeholders entre chaves {}

* Placeholders válidos: {CODIGO}, {REVISAO}, {DATA}, etc.

* Não podem conter caracteres especiais inválidos

#### 5.2.4 Dados Fiscais

* NCM deve ter 8 dígitos

* Origem deve ser código válido (0-8)

* Percentual IPI deve ser numérico (0-100)

* Contas contábeis devem seguir plano de contas

### 5.3 Segurança e Acesso

#### 5.3.1 Permissões

* Apenas usuários administrativos podem configurar

* Controle de acesso ao formulário

* Auditoria de alterações realizadas

* Log de configurações modificadas

#### 5.3.2 Proteção de Dados Sensíveis

* Token da API deve ser criptografado no banco

* Não exibir tokens completos na interface

* Log de alterações sem dados sensíveis

* Backup seguro das configurações

## 6. Fluxos de Processo

### 6.1 Configuração Inicial

1. **Abrir Formulário**: O sistema abre o formulário de configuração do integrador
2. **Carregar Configurações Existentes**: Busca no banco de dados as configurações previamente salvas
3. **Verificar Configurações**: Se configurações são encontradas, preenche os campos; caso contrário, deixa os campos vazios
4. **Validar Conectividade**: Testa conectividade com API se endereço estiver configurado
5. **Exibir Interface**: Apresenta a interface ao usuário com os três grupos de configuração
6. **Aguardar Ação do Usuário**: Sistema fica disponível para interação do usuário

### 6.2 Configuração de API

1. **Preencher Endereço**: Usuário informa URL base da API de integração
2. **Informar Token**: Usuário insere token de autenticação do sistema externo
3. **Definir Código Empresa**: Usuário informa código identificador da empresa
4. **Configurar eDrawings**: Opcionalmente, usuário informa chave de licença eDrawings
5. **Testar Conectividade**: Sistema pode testar conexão com API (opcional)
6. **Validar Formato**: Sistema verifica formato de URL e token
7. **Confirmar Configuração**: Usuário confirma configurações de API

### 6.3 Configuração de Máscaras

1. **Definir Máscara Peça**: Usuário informa padrão de nomenclatura para peças individuais
2. **Definir Máscara Conjunto**: Usuário informa padrão de nomenclatura para conjuntos/montagens
3. **Validar Placeholders**: Sistema verifica se máscaras contêm placeholders válidos
4. **Testar Máscaras**: Sistema pode gerar exemplos com as máscaras definidas
5. **Confirmar Máscaras**: Usuário confirma padrões de nomenclatura

### 6.4 Configuração de Dados do Produto

1. **Selecionar Classificação Fiscal**: Usuário usa F7 para buscar e selecionar NCM padrão
2. **Definir Origem**: Usuário seleciona origem fiscal padrão dos produtos
3. **Configurar Finalidade**: Usuário define finalidade padrão do material
4. **Definir Controle Saída**: Usuário configura tipo de controle para saída
5. **Configurar IPI**: Usuário define percentual de IPI padrão
6. **Definir Localizações**: Usuário configura locais padrão de entrada e saída
7. **Configurar Conta Contábil**: Usuário define conta contábil padrão
8. **Validar Dados Fiscais**: Sistema verifica formato e validade dos dados
9. **Confirmar Configurações**: Usuário confirma dados padrão de produtos

### 6.5 Salvamento de Configurações

1. **Clicar Salvar**: Usuário aciona o botão de salvamento
2. **Validar Todos os Campos**: Sistema executa validação completa de todos os grupos
3. **Verificar Campos Obrigatórios**: Sistema confirma preenchimento de campos obrigatórios
4. **Validar Formatos**: Sistema verifica formato de URLs, tokens, máscaras e dados fiscais
5. **Confirmar Salvamento**: Sistema solicita confirmação do usuário
6. **Salvar no Banco**: Persiste todas as configurações no banco de dados
7. **Atualizar Variáveis Globais**: Atualiza variáveis da API em memória
8. **Exibir Confirmação**: Confirma ao usuário que configurações foram salvas
9. **Sincronizar Sistema**: Aplica configurações em outros módulos
10. **Configuração Concluída**: Processo finalizado com sucesso

## 7. Tratamento de Erros

### 7.1 Erros de Validação

#### 7.1.1 Campos Obrigatórios Vazios

* **Situação**: Campos obrigatórios não preenchidos

* **Ação**: Destacar campos em erro

* **Solução**: Orientar preenchimento obrigatório

* **Prevenção**: Validação em tempo real

#### 7.1.2 Formato de URL Inválido

* **Situação**: URL da API com formato incorreto

* **Ação**: Exibir erro de formato

* **Solução**: Mostrar exemplo de URL válida

* **Prevenção**: Validação durante digitação

#### 7.1.3 Token Inválido

* **Situação**: Token com formato incorreto

* **Ação**: Exibir erro de autenticação

* **Solução**: Orientar sobre formato correto

* **Prevenção**: Validação de formato

#### 7.1.4 Máscaras Inválidas

* **Situação**: Máscaras sem placeholders válidos

* **Ação**: Exibir erro de formato

* **Solução**: Mostrar exemplos de máscaras válidas

* **Prevenção**: Lista de placeholders disponíveis

### 7.2 Erros de Conectividade

#### 7.2.1 API Inacessível

* **Situação**: Não consegue conectar com API externa

* **Ação**: Exibir aviso de conectividade

* **Solução**: Verificar URL e conectividade

* **Prevenção**: Teste de conectividade opcional

#### 7.2.2 Token Expirado

* **Situação**: Token de autenticação expirado

* **Ação**: Exibir erro de autenticação

* **Solução**: Solicitar renovação do token

* **Prevenção**: Monitoramento de validade

### 7.3 Erros de Banco de Dados

#### 7.3.1 Falha na Conexão

* **Situação**: Não consegue conectar ao banco

* **Ação**: Exibir erro de conexão

* **Solução**: Verificar configuração do banco

* **Prevenção**: Teste de conexão inicial

#### 7.3.2 Erro no Salvamento

* **Situação**: Falha ao salvar configurações

* **Ação**: Rollback da transação

* **Solução**: Tentar novamente

* **Prevenção**: Validação prévia completa

### 7.4 Erros de Integração

#### 7.4.1 Falha na Sincronização

* **Situação**: Erro ao atualizar variáveis globais

* **Ação**: Exibir aviso de sincronização

* **Solução**: Reiniciar sistema para aplicar

* **Prevenção**: Validação de integridade

#### 7.4.2 Configuração Incompatível

* **Situação**: Configurações incompatíveis com sistema

* **Ação**: Exibir aviso de incompatibilidade

* **Solução**: Ajustar configurações

* **Prevenção**: Validação de compatibilidade

## 8. Integração com Outros Módulos

### 8.1 Sistema de Banco de Dados

#### 8.1.1 Tabela configuracao\_api

* **Campos API**: endereco\_api, token\_consistem, codigo\_empresa, chave\_edrawings

* **Campos Máscara**: mascara\_peca, mascara\_conjunto

* **Campos Produto**: classificacao\_fiscal, origem, finalidade, controle\_saida, ipi\_perc, localiz\_entrada, localiz\_saida, conta\_contabil

* **Auditoria**: data\_alteracao, usuario\_alteracao

#### 8.1.2 Operações

* **SELECT**: Carregamento de configurações existentes

* **INSERT/UPDATE**: Salvamento de configurações

* **Validação**: Verificação de integridade

* **Auditoria**: Log de alterações

### 8.2 Sistema de API Externa

#### 8.2.1 Configurações de Conexão

* URL base para requisições

* Token de autenticação

* Código da empresa

* Timeout de conexão

#### 8.2.2 Integração

* Teste de conectividade

* Validação de credenciais

* Sincronização de dados

* Monitoramento de status

### 8.3 Sistema de Nomenclatura

#### 8.3.1 Aplicação de Máscaras

* Geração automática de códigos

* Aplicação em peças individuais

* Aplicação em conjuntos/montagens

* Validação de unicidade

#### 8.3.2 Placeholders Disponíveis

* {CODIGO}: Código sequencial

* {REVISAO}: Revisão do item

* {DATA}: Data de criação

* {USUARIO}: Usuário criador

* {CLIENTE}: Código do cliente

### 8.4 Sistema Fiscal e Contábil

#### 8.4.1 Dados Padrão

* Aplicação automática em novos produtos

* Classificação fiscal padrão

* Origem e finalidade padrão

* Configurações de IPI

#### 8.4.2 Integração Contábil

* Conta contábil padrão

* Localizações de estoque

* Controles de movimentação

* Relatórios fiscais

***

# RF006 - Cadastro de Processos

## 1. Descrição Geral

O RF006 implementa o cadastro e gerenciamento de processos de fabricação no sistema. Este módulo permite o controle completo de processos, incluindo operações internas e externas, máquinas associadas e situação ativa/inativa dos processos.

## 2. Interface do Usuário

### 2.1 Formulário Principal

#### 2.1.1 Campos de Identificação

* **Código**: Campo numérico com botão F7 para pesquisa de processos existentes

* **Situação**: Checkbox para indicar se o processo está ativo

#### 2.1.2 Campos de Configuração

* \*\*Operação \*\*\*: ComboBox obrigatório para seleção do tipo de operação

* \*\*Máquina \*\*\*: ComboBox para seleção da máquina (pode ser desabilitado para operações externas)

#### 2.1.3 Botões de Ação

* **Salvar**: Grava as informações do processo

* **Excluir**: Remove o processo selecionado (habilitado apenas para processos existentes)

* **Limpar**: Limpa todos os campos do formulário

### 2.2 Comportamentos da Interface

#### 2.2.1 Estado Inicial

* Formulário aberto em modo de inclusão

* Botão Excluir desabilitado

* Checkbox Situação marcado (ativo)

* Campos obrigatórios destacados

#### 2.2.2 Modo de Edição

* Carregamento automático dos dados do processo

* Botão Excluir habilitado

* Validação de campos obrigatórios

* Controle de dependências entre operação e máquina

## 3. Funcionalidades

### 3.1 Carregamento de Dados

#### 3.1.1 Carregamento Inicial

* Carrega lista de operações via API

* Carrega lista de máquinas disponíveis

* Inicializa controles do formulário

* Configura eventos e validações

#### 3.1.2 Pesquisa de Processos

* Busca por código via botão F7

* Carregamento automático dos dados

* Atualização da interface para modo edição

* Habilitação do botão Excluir

### 3.2 Validação de Dados

#### 3.2.1 Campos Obrigatórios

* Validação de preenchimento da operação

* Validação condicional da máquina

* Destaque visual de campos em erro

* Mensagens de orientação ao usuário

#### 3.2.2 Regras de Negócio

* Operações internas requerem máquina

* Operações externas não utilizam máquina

* Códigos devem ser únicos no sistema

* Processos inativos não podem ser utilizados

### 3.3 Operações CRUD

#### 3.3.1 Inclusão

* Validação de dados obrigatórios

* Geração automática de código (se aplicável)

* Gravação no banco de dados

* Confirmação de sucesso

#### 3.3.2 Alteração

* Carregamento de dados existentes

* Validação de alterações

* Atualização no banco de dados

* Log de auditoria

#### 3.3.3 Exclusão

* Verificação de dependências

* Confirmação do usuário

* Exclusão lógica ou física

* Atualização de referências

#### 3.3.4 Consulta

* Pesquisa por código

* Filtros por situação

* Listagem de processos

* Exportação de dados

### 3.4 Controle de Operações

#### 3.4.1 Operações Internas

* Requerem seleção de máquina

* ComboBox de máquina habilitado

* Validação obrigatória de máquina

* Controle de capacidade

#### 3.4.2 Operações Externas

* Não requerem máquina

* ComboBox de máquina desabilitado

* Campo máquina limpo automaticamente

* Controle de fornecedores externos

## 4. Regras de Negócio

### 4.1 Validação de Dados

#### 4.1.1 Campos Obrigatórios

* Operação deve ser selecionada

* Máquina obrigatória para operações internas

* Código deve ser único no sistema

* Situação deve ser definida

#### 4.1.2 Consistência de Dados

* Operação deve existir no cadastro

* Máquina deve estar ativa

* Processo não pode ter dependências circulares

* Histórico de alterações preservado

### 4.2 Controle de Acesso

#### 4.2.1 Permissões

* Usuários autorizados podem incluir

* Supervisores podem alterar

* Administradores podem excluir

* Auditoria de todas as operações

#### 4.2.2 Restrições

* Processos em uso não podem ser excluídos

* Alterações requerem justificativa

* Exclusão requer confirmação dupla

* Log completo de atividades

### 4.3 Integração com Sistema

#### 4.3.1 Dependências

* Cadastro de operações atualizado

* Cadastro de máquinas disponível

* Sistema de permissões ativo

* Banco de dados acessível

#### 4.3.2 Impactos

* Processos afetam planejamento

* Alterações impactam custos

* Exclusões afetam histórico

* Situação controla disponibilidade

## 5. Fluxos de Processo

### 5.1 Inicialização do Formulário

1. **Abertura**: Sistema abre formulário de cadastro
2. **Carregamento**: Carrega listas de operações e máquinas via API
3. **Inicialização**: Configura estado inicial dos controles
4. **Validação**: Prepara validadores de campos
5. **Eventos**: Associa eventos aos controles
6. **Pronto**: Formulário pronto para uso

### 5.2 Cadastro de Novo Processo

1. **Início**: Usuário inicia cadastro de novo processo
2. **Operação**: Seleciona tipo de operação no combobox
3. **Máquina**: Sistema habilita/desabilita campo máquina conforme operação
4. **Seleção**: Usuário seleciona máquina (se necessário)
5. **Situação**: Define se processo estará ativo
6. **Validação**: Sistema valida campos obrigatórios
7. **Gravação**: Salva processo no banco de dados
8. **Confirmação**: Exibe mensagem de sucesso
9. **Limpeza**: Limpa formulário para novo cadastro

### 5.3 Edição de Processo Existente

1. **Pesquisa**: Usuário clica F7 no campo código
2. **Seleção**: Escolhe processo na lista de pesquisa
3. **Carregamento**: Sistema carrega dados do processo
4. **Exibição**: Preenche campos com dados carregados
5. **Habilitação**: Habilita botão Excluir
6. **Edição**: Usuário modifica dados necessários
7. **Validação**: Sistema valida alterações
8. **Gravação**: Atualiza processo no banco
9. **Auditoria**: Registra log de alteração
10. **Confirmação**: Exibe mensagem de sucesso

### 5.4 Exclusão de Processo

1. **Seleção**: Processo carregado para edição
2. **Exclusão**: Usuário clica botão Excluir
3. **Verificação**: Sistema verifica dependências
4. **Confirmação**: Solicita confirmação do usuário
5. **Validação**: Verifica permissões de exclusão
6. **Remoção**: Remove processo do banco
7. **Atualização**: Atualiza referências relacionadas
8. **Log**: Registra exclusão na auditoria
9. **Limpeza**: Limpa formulário
10. **Confirmação**: Exibe mensagem de sucesso

## 6. Tratamento de Erros

### 6.1 Erros de Validação

#### 6.1.1 Campos Obrigatórios Vazios

* **Situação**: Operação não selecionada

* **Ação**: Destacar campo em erro

* **Solução**: Orientar seleção obrigatória

* **Prevenção**: Validação em tempo real

#### 6.1.2 Máquina Não Selecionada

* **Situação**: Operação interna sem máquina

* **Ação**: Exibir erro de validação

* **Solução**: Solicitar seleção de máquina

* **Prevenção**: Controle automático de habilitação

#### 6.1.3 Código Duplicado

* **Situação**: Código já existe no sistema

* **Ação**: Exibir erro de duplicidade

* **Solução**: Sugerir novo código

* **Prevenção**: Validação durante digitação

### 6.2 Erros de Sistema

#### 6.2.1 Falha no Carregamento

* **Situação**: Erro ao carregar listas via API

* **Ação**: Exibir aviso de conectividade

* **Solução**: Tentar recarregar dados

* **Prevenção**: Cache local de dados

#### 6.2.2 Erro de Gravação

* **Situação**: Falha ao salvar no banco

* **Ação**: Rollback da transação

* **Solução**: Tentar novamente

* **Prevenção**: Validação prévia completa

### 6.3 Erros de Permissão

#### 6.3.1 Acesso Negado

* **Situação**: Usuário sem permissão

* **Ação**: Exibir erro de acesso

* **Solução**: Solicitar permissão adequada

* **Prevenção**: Verificação inicial de permissões

#### 6.3.2 Processo em Uso

* **Situação**: Tentativa de excluir processo em uso

* **Ação**: Exibir aviso de dependência

* **Solução**: Remover dependências primeiro

* **Prevenção**: Verificação de dependências

## 7. Integração com Outros Módulos

### 7.1 Sistema de Banco de Dados

#### 7.1.1 Tabela processos

* **Campos**: id, codigo, operacao\_id, maquina\_id, situacao

* **Auditoria**: data\_criacao, data\_alteracao, usuario

* **Índices**: codigo (único), operacao\_id, maquina\_id

* **Relacionamentos**: operacoes, maquinas

#### 7.1.2 Operações

* **SELECT**: Consulta de processos

* **INSERT**: Inclusão de novos processos

* **UPDATE**: Alteração de processos

* **DELETE**: Exclusão de processos

### 7.2 Sistema de API

#### 7.2.1 Endpoints Utilizados

* **/operacoes**: Lista de operações disponíveis

* **/maquinas**: Lista de máquinas por operação

* **/processos**: CRUD de processos

* **/validacao**: Validação de dados

#### 7.2.2 Integração

* Carregamento dinâmico de listas

* Validação em tempo real

* Sincronização de dados

* Cache de informações

### 7.3 Sistema de Planejamento

#### 7.3.1 Impacto nos Processos

* Processos ativos disponíveis para planejamento

* Capacidade de máquinas considerada

* Tempos de operação calculados

* Sequenciamento otimizado

#### 7.3.2 Dependências

* Alterações afetam planejamento existente

* Exclusões requerem replanejamento

* Situação controla disponibilidade

* Histórico preservado para auditoria

***

# RF007 - Importação de Produtos

## 1. Informações Básicas

**Código:** RF007\
**Nome:** Importação de Produtos\
**Módulo:** Integrador\
**Formulário:** `AddinArtama\02_formularios\07_integrador\FrmProdutoImport.cs`

## 2. Descrição Geral

O sistema de importação de produtos é responsável pela importação e cadastro automático de produtos do SolidWorks para o sistema ERP, permitindo a navegação entre componentes e geração da engenharia de produto. O sistema integra diretamente com o SolidWorks para extrair informações de assemblies e realizar cadastros massivos no ERP.

## 3. Interface do Usuário

### 3.1 Painel de Dados do Produto

#### 3.1.1 Informações do Material

* **Dimensão (lblEspess)**:

  * Tipo: Label somente leitura

  * Função: Exibe dimensões da peça (espessura x largura x comprimento)

  * Formato: Diferenciado para chapas e perfis

- **Código Material (lblCodMat)**:

  * Tipo: Label somente leitura

  * Função: Exibe código do material da lista de corte

  * Cor: Vermelha para destaque

  * Tooltip: "Clique para Copiar"

- **Descrição Material (lblDescMat)**:

  * Tipo: Label somente leitura

  * Função: Exibe descrição do material

  * Fonte: Pequena para economia de espaço

#### 3.1.2 Informações do Produto

* **Código Produto (lblCodigoProduto)**:

  * Tipo: Label clicável

  * Função: Exibe código do produto no ERP

  * Cor: Vermelha para destaque

  * Evento: Click para copiar código

* **Peso Bruto (lblPesoBrut)**:

  * Tipo: Label somente leitura

  * Função: Exibe peso bruto calculado

  * Formato: "X,XXX kg"

* **Peso NBR (lblPesoNbr)**:

  * Tipo: Label somente leitura

  * Função: Exibe peso padrão NBR

  * Formato: "X,XXX kg"

#### 3.1.3 Campos Editáveis

* **Descrição (txtDescricao)**:

  * Tipo: TextBox editável

  * Função: Descrição do produto

  * Validação: Campo obrigatório

* **Sobremetal Largura (txtSmLarg)**:

  * Tipo: TextBox numérico

  * Função: Define sobremetal na largura

  * Formato: Números inteiros

* **Sobremetal Comprimento (txtSmCompr)**:

  * Tipo: TextBox numérico

  * Função: Define sobremetal no comprimento

  * Formato: Números inteiros

#### 3.1.4 Indicadores Visuais

* **Ícone de Erro de Material (ptbMaterialError)**:

  * Tipo: PictureBox clicável

  * Função: Indica problemas com material

  * Visibilidade: Controlada por pendências

  * Tooltip: "Alterar Material"

### 3.2 Botões de Ação Principal

* **Carregar (btnCarrProcess)**:

  * Função: Carrega componentes do assembly ativo

  * Ícone: Carregar

  * Tooltip: "Carregar componentes"

  * Estado: Habilitado quando há assembly ativo

* **Salvar (btnSalvar)**:

  * Função: Inicia cadastro/atualização massiva

  * Ícone: Salvar

  * Tooltip: "Salvar/Atualizar Produto e Engenharia"

  * Estado: Habilitado após carregamento

### 3.4 Sistema de Abas (tbcOperacoes)

#### 3.4.1 Aba Lista (tbpLista)

* **Grid de Componentes (dgv)**:

  * Tipo: DataGridView customizado

  * Função: Lista todos componentes carregados

  * Colunas: Nome, Nível, Status, Pendências

  * Eventos: Seleção sincronizada com navegação

#### 3.4.2 Aba Engenharia (tbpEngenharia)

* **Árvore de Produto (trvProduto)**:

  * Tipo: TreeView

  * Função: Exibe estrutura hierárquica

  * Sincronização: Atualizada conforme navegação

## 4. Funcionalidades Implementadas

### 4.1 Carregamento de Componentes

**Descrição**: Extração automática de componentes do assembly ativo do SolidWorks

**Processo**:

1. Verificação de assembly ativo no SolidWorks
2. Extração da estrutura hierárquica completa
3. Identificação de tipos de componente (peça, subconjunto, biblioteca)
4. Carregamento de informações de lista de corte
5. Análise de propriedades customizadas
6. Identificação de pendências de engenharia
7. Construção da árvore de engenharia
8. Carregamento no grid de lista

**Método**: `CarregarProcessosAsync()` executado de forma assíncrona

### 4.2 Navegação Inteligente

**Descrição**: Sistema de navegação sequencial entre componentes com sincronização 3D

**Funcionalidades**:

* Navegação através de botões Voltar/Próximo

* Sincronização com seleção no grid

* Abertura automática de arquivos no SolidWorks

* Salvamento automático antes da navegação

* Controle de arquivos somente leitura

* Fechamento inteligente de arquivos

**Métodos**: `BtnVoltar_Click()`, `BtnProximo_Click()`, `Dgv_RowIndexChanged()`

### 4.3 Cadastro Automático no ERP

**Descrição**: Integração completa com API do sistema ERP para cadastro massivo

**Processo de Cadastro**:

1. Validação de conectividade com API
2. Verificação de produtos existentes
3. Aplicação de máscaras de nomenclatura
4. Cadastro de novos produtos via `Api.DuplicarItemGenericoAsync()`
5. Atualização de produtos existentes via `Api.UpdateItemGenericoAsync()`
6. Atualização de propriedades customizadas no SolidWorks
7. Controle de erros e rollback quando necessário

**Método**: `CadastrarNovo()` com execução assíncrona e barra de progresso

### 4.4 Controle de Qualidade e Pendências

**Descrição**: Sistema de identificação e controle de pendências de engenharia

**Tipos de Pendências**:

* **Críticas**: Bloqueiam o cadastro

  * Material incorreto

  * Propriedades obrigatórias ausentes

  * Erros de dimensionamento

* **Não Críticas**: Apenas alertas

  * Informações complementares

  * Sugestões de melhoria

  * Avisos de configuração

**Indicadores Visuais**:

* Cores no grid (verde=sucesso, vermelho=erro)

* Ícones de status por componente

* Tooltips informativos

* Mensagens contextuais

### 4.5 Integração SolidWorks

**Descrição**: Integração nativa com API do SolidWorks

**Operações**:

* Abertura automática de arquivos (`OpenDoc6`)

* Ativação de documentos (`ActivateDoc2`)

* Controle de configurações (`ShowConfiguration2`)

* Atualização de propriedades customizadas

* Salvamento automático (`Save`, `Save3`)

* Fechamento controlado (`CloseDoc`)

* Controle de visualização (`ShowNamedView`, `ViewZoomtofit`)

### 4.6 Geração de Engenharia

**Descrição**: Criação automática da estrutura de engenharia no ERP

**Processo**:

1. Percorrimento da árvore hierárquica
2. Criação de relacionamentos pai-filho
3. Definição de quantidades por nível
4. Validação de integridade da estrutura
5. Salvamento da engenharia completa

**Método**: `PercorrerTreeViewSalvarEngAsync()` com controle de progresso

## 5. Regras de Negócio

### 5.1 Validação de Dados

#### 5.1.1 Campos Obrigatórios

* Descrição do produto deve estar preenchida

* Código do material deve estar definido

* Propriedades customizadas obrigatórias

#### 5.1.2 Validação Numérica

* Campos de sobremetal devem ser numéricos

* Pesos devem ser positivos

* Peso líquido não pode ser maior que bruto

#### 5.1.3 Integridade de Materiais

* Material deve existir no sistema ERP

* Propriedades de material devem estar corretas

* Dimensões devem ser compatíveis

### 5.2 Controle de Acesso

#### 5.2.1 Permissões de Usuário

* Acesso restrito a usuários autorizados

* Validação de permissões para cadastro no ERP

* Controle de operações por perfil

#### 5.2.2 Controle de Arquivos

* Verificação de arquivos em uso

* Controle de arquivos somente leitura

* Validação de caminhos de arquivo

### 5.3 Integração ERP

#### 5.3.1 Configurações da API

* Utilização de token de autenticação

* Respeito aos limites de requisições

* Tratamento de timeouts

#### 5.3.2 Máscaras de Produto

* Aplicação de máscaras configuradas no sistema

* Validação de nomenclatura

* Controle de duplicidade de códigos

#### 5.3.3 Sincronização de Dados

* Sincronização com dados de materiais

* Controle de versões de dados

### 5.4 Gestão de Arquivos SolidWorks

#### 5.4.1 Controle de Estado

* Verificação de documentos ativos

* Controle de modificações não salvas

* Gestão de configurações ativas

#### 5.4.2 Operações de Arquivo

* Salvamento automático após alterações

* Fechamento controlado de documentos

* Verificação de integridade de arquivos

## 6. Fluxos de Processo

### 6.1 Carregamento Inicial

1. **Verificação**: Sistema verifica assembly ativo no SolidWorks
2. **Inicialização**: Inicia processo de carregamento assíncrono
3. **Extração**: Extrai estrutura completa de componentes
4. **Análise**: Analisa listas de corte e propriedades
5. **Pendências**: Identifica pendências de engenharia
6. **Grid**: Carrega dados no grid da aba "Lista"
7. **Árvore**: Constrói árvore hierárquica na aba "Engenharia"
8. **Posicionamento**: Posiciona no primeiro componente
9. **Interface**: Atualiza interface com dados do primeiro item
10. **Finalização**: Habilita controles para navegação

### 6.2 Navegação entre Componentes

1. **Ação**: Usuário clica em "Voltar" ou "Próximo"
2. **Validação**: Verifica se há componentes na direção solicitada
3. **Salvamento**: Salva alterações do componente atual
4. **Fechamento**: Fecha arquivo atual (se não for o principal)
5. **Seleção**: Atualiza seleção no grid
6. **Abertura**: Abre novo componente no SolidWorks
7. **Carregamento**: Carrega dados do novo componente
8. **Atualização**: Atualiza informações na interface
9. **Pendências**: Verifica e exibe pendências do componente
10. **Sincronização**: Atualiza árvore de engenharia

### 6.3 Processo de Cadastro Massivo

1. **Inicialização**: Usuário clica em "Salvar"

2. **Preparação**: Sistema prepara processo assíncrono

3. **Progresso**: Exibe barra de progresso

4. **Iteração**: Para cada componente da estrutura:

   * Validação de dados obrigatórios

   * Verificação de existência no ERP

   * Aplicação de máscaras de nomenclatura

   * Cadastro ou atualização no ERP

   * Atualização de propriedades no SolidWorks

   * Controle de erros e pendências

5. **Engenharia**: Gera estrutura de engenharia no ERP

6. **Salvamento**: Salva assembly principal

7. **Finalização**: Exibe resultado do processo

8. **Atualização**: Atualiza interface com novos códigos

## 7. Tratamento de Erros

### 7.1 Erros de Carregamento

#### 7.1.1 Assembly Não Ativo

* **Situação**: Nenhum assembly ativo no SolidWorks

* **Ação**: Exibir mensagem orientativa

* **Solução**: Abrir assembly antes de carregar

* **Prevenção**: Verificação prévia de documentos

#### 7.1.2 Falha na Extração

* **Situação**: Erro ao extrair componentes

* **Ação**: Log detalhado do erro

* **Solução**: Verificar integridade do assembly

* **Prevenção**: Validação de estrutura

### 7.2 Erros de Integração ERP

#### 7.2.1 Falha de Conectividade

* **Situação**: API do ERP indisponível

* **Ação**: Exibir erro de conectividade

* **Solução**: Verificar configurações de rede

* **Prevenção**: Teste de conectividade prévia

#### 7.2.2 Erro de Autenticação

* **Situação**: Token de API inválido

* **Ação**: Solicitar reconfiguração

* **Solução**: Atualizar token nas configurações

* **Prevenção**: Validação periódica de token

#### 7.2.3 Erro de Cadastro

* **Situação**: Falha ao cadastrar produto

* **Ação**: Rollback da operação

* **Solução**: Verificar dados obrigatórios

* **Prevenção**: Validação prévia completa

### 7.3 Erros de SolidWorks

#### 7.3.1 Arquivo Não Encontrado

* **Situação**: Componente com caminho inválido

* **Ação**: Marcar como pendência

* **Solução**: Corrigir referências de arquivo

* **Prevenção**: Verificação de caminhos

#### 7.3.2 Arquivo Somente Leitura

* **Situação**: Tentativa de salvar arquivo protegido

* **Ação**: Exibir aviso específico

* **Solução**: Remover proteção ou pular salvamento

* **Prevenção**: Verificação de permissões

### 7.4 Erros de Validação

#### 7.4.1 Dados Obrigatórios

* **Situação**: Campos obrigatórios vazios

* **Ação**: Destacar campos em erro

* **Solução**: Preencher informações necessárias

* **Prevenção**: Validação em tempo real

#### 7.4.2 Formato Inválido

* **Situação**: Dados em formato incorreto

* **Ação**: Exibir erro de formato

* **Solução**: Corrigir formato dos dados

* **Prevenção**: Máscaras de entrada

## 8. Integração com Outros Módulos

### 8.1 Sistema de Configuração

#### 8.1.1 Configurações da API

* Utilização de configurações centralizadas

* Token de autenticação

* URLs de endpoints

* Timeouts e limites

#### 8.1.2 Máscaras de Produto

* Máscaras para peças e conjuntos

* Regras de nomenclatura

* Validação de formatos

### 8.2 Sistema ERP

#### 8.2.1 Cadastro de Produtos

* Integração com módulo de produtos

* Sincronização de materiais

* Controle de duplicidade

#### 8.2.2 Engenharia de Produto

* Criação de estruturas

* Relacionamentos hierárquicos

* Quantidades por nível

### 8.3 SolidWorks API

#### 8.3.1 Manipulação de Documentos

* Abertura e fechamento controlado

* Salvamento automático

* Controle de configurações

#### 8.3.2 Extração de Dados

* Propriedades customizadas

* Lista de corte

* Estrutura de assembly

* Informações de material

