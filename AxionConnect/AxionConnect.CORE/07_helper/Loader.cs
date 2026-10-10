using System;
using System.Threading.Tasks;

namespace AxionConnect {
  /// <summary>
  /// Substitui o Loader que existia na LmCorbieUI (foi removido de lá), com a mesma API usada pelas
  /// regras de negócio: <see cref="ShowDuringOperation"/> mostra o progresso enquanto a operação roda e
  /// <see cref="_isWorking"/> fica false quando o usuário cancela.
  /// Quem mostra o progresso é o app: ele registra <see cref="Exibir"/> (no Axion Connect WPF, o overlay com Cancelar).
  /// </summary>
  public static class Loader {
    /// <summary>True enquanto a operação roda; o cancelamento do usuário volta para false.</summary>
    public static bool _isWorking { get; private set; }

    /// <summary>Exibe o progresso e executa a operação (recebe o IProgress das mensagens).</summary>
    public static Func<Func<IProgress<string>, Task>, Task> Exibir { get; set; }

    /// <summary>Pedido de cancelamento: as regras de negócio param no próximo ponto de verificação.</summary>
    public static void Cancelar() => _isWorking = false;

    public static async Task ShowDuringOperation(Func<IProgress<string>, Task> operacao) {
      _isWorking = true;
      try {
        if (Exibir != null)
          await Exibir(operacao);
        else
          await operacao(new Progress<string>(_ => { }));
      } finally {
        _isWorking = false;
      }
    }
  }
}
