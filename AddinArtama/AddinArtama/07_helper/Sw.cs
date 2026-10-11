using SolidWorks.Interop.sldworks;
using System;
using System.IO;

namespace AddinArtama {
  public class Sw {
    public static SldWorks App = new SldWorks();

    /// <summary>
    /// O InsertBomTable3 devolve null (sem lançar erro) quando não consegue inserir a lista,
    /// quase sempre porque o template não existe. Sem esta checagem o erro aparecia depois,
    /// como "Referência de objeto não definida", sem dizer a causa.
    /// </summary>
    public static BomTableAnnotation ExigirLista(BomTableAnnotation lista, string template) {
      if (lista != null)
        return lista;

      if (string.IsNullOrWhiteSpace(template))
        throw new InvalidOperationException(
          "O template da lista de materiais não está configurado.\nInforme o arquivo em Configurações > Templates.");

      if (!File.Exists(template))
        throw new FileNotFoundException("Template da lista de materiais não encontrado.", template);

      throw new InvalidOperationException(
        $"O SolidWorks não conseguiu inserir a lista de materiais na montagem.\nTemplate: {template}");
    }
  }
}
