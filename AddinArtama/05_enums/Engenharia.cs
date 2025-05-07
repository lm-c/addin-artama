using System.ComponentModel;

public enum TipoMateriaPrima {
  [Description("Soldagem")]
  Soldagem = 0,
  [Description("Chapa")]
  Chapa = 1,
}

public enum TipoDucumento{
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