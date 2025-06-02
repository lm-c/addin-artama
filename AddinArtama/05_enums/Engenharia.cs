using System.ComponentModel;

public enum TipoMateriaPrima {
  [Description("Soldagem")]
  Soldagem = 0,
  [Description("Chapa")]
  Chapa = 1,
}

public enum TipoDocumento{
  [Description("Peça")]
  Peca = 0,
  [Description("Montagem")]
  Montagem = 1,
}

public enum TipoSequencia {
  [Description("Processamento")]
  Processamento = 0,
  [Description("Destino")]
  Destino = 1,
}

public enum TipoLogEngenharia {
  [Description("Duplicação de Produto")]
  DuplicacaoProduto = 0,
  [Description("Pendência de Engenharia")]
  PendenciEngenharia = 1,
}

public enum PendenciasEngenharia {
  [Description("Nescessário revisar operações")]
  OperacaoRevisar = 0,
  [Description("Não possui operações")]
  OperacaoNaoPossui = 1,
  [Description("Aberto como somente leitura")]
  SomenteLeitura = 2,
  [Description("Material não cadastrado no ERP")]
  MateriaPrimaIncorreta = 3,
}