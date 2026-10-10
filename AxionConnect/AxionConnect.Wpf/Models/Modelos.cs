using AxionConnect.Wpf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using LmCorbieUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using TreeNode = System.Windows.Forms.TreeNode;

namespace AxionConnect.Wpf.Models {
  /// <summary>
  /// Linha da grade de componentes: o ProdutoErp do CORE (com os ícones em Bitmap) e o estado de tela.
  /// </summary>
  internal partial class LinhaProduto : ObservableObject {
    public LinhaProduto(ProdutoErp produto) => Produto = produto;

    public ProdutoErp Produto { get; }

    public string Nivel => Produto.Nivel;
    public string Name => Produto.Name;
    public string CodProduto => Produto.CodProduto;
    public string Denominacao => Produto.Denominacao;
    public string Referencia => Produto.Referencia;

    public System.Drawing.Bitmap Img3D => Produto.Img3D;
    public System.Drawing.Bitmap Img2D => Produto.Img2D;
    public System.Drawing.Bitmap ImgFantasma => Produto.ImgFantasma;
    public System.Drawing.Bitmap ImgPendencia => Produto.ImgPendencia;

    public string TextoPendencias => string.Join("\n", Produto.Pendencias.Select(x => $"- {x.ObterDescricaoEnum()}"));

    /// <summary>Engenharia gravada no ERP (linha verde, como no FrmEngenharia).</summary>
    [ObservableProperty]
    private bool _salvo;

    /// <summary>O ProdutoErp mudou (pendências, fantasma...): redesenha a linha.</summary>
    public void Atualizar() => OnPropertyChanged(string.Empty);
  }

  /// <summary>
  /// Nó da árvore da engenharia: cópia em WPF do TreeNode (WinForms) que o CORE monta ao ler o ERP.
  /// </summary>
  internal class NoProduto {
    /// <summary>Mesmos ícones/índices do ImageList do FrmEngenharia.</summary>
    static readonly ImageSource[] icones = {
      Icones.Montagem, Icones.Peca, Icones.ListaCorte, Icones.Chapa, Icones.Soldagem, Icones.Biblioteca,
    };

    public ProdutoErp Produto { get; private set; }
    public string Texto { get; private set; }
    public ImageSource Icone { get; private set; }
    public List<NoProduto> Filhos { get; } = new List<NoProduto>();
    public bool Expandido { get; set; } = true;

    public static NoProduto Criar(TreeNode node) {
      var no = new NoProduto {
        Produto = node.Tag as ProdutoErp,
        Texto = node.Text,
        Icone = node.ImageIndex >= 0 && node.ImageIndex < icones.Length ? icones[node.ImageIndex] : icones[0],
      };

      foreach (TreeNode filho in node.Nodes)
        no.Filhos.Add(Criar(filho));

      return no;
    }
  }

  /// <summary>Item de ComboBox (antigo Z_Padrao).</summary>
  internal record OpcaoCombo(int Codigo, string Descricao);

  /// <summary>Linha da grade do cadastro de processos (operação + máquina do ERP).</summary>
  internal record LinhaProcesso(int Codigo, string Operacao, string Maquina, string Tipo, string Situacao);

  /// <summary>Operação inserida no componente (antigo CardOperacao).</summary>
  internal class CartaoOperacao {
    public produto_erp_operacao Operacao { get; }
    public string Texto { get; }

    public CartaoOperacao(produto_erp_operacao operacao) {
      Operacao = operacao;

      // Mesmo texto do CardOperacao.SetText
      var processo = Processo.ListaProcessos.FirstOrDefault(x => x.codAxion == operacao.processo_id);
      Texto = processo == null
        ? $"Processo {operacao.processo_id} não encontrado\r\nTempo Operação: {operacao.tempo} | QTD Operadores: {operacao.qtd_operador}"
        : $"Operação: {processo.codOperacao} {processo.descrOperacao}\r\n" +
          $"Máquina: {processo.codMaquina} {processo.descrMaquina}\r\n" +
          $"Tempo Operação: {operacao.tempo} | QTD Operadores: {operacao.qtd_operador}";
    }
  }

  /// <summary>Grupo da aba "Sequência Oper.": componentes consumidos em cada operação (antigos LmGroupBox + CardProduto).</summary>
  internal class EtapaConsumo {
    public int Sequencia { get; init; }
    public string Titulo { get; init; }
    public ObservableCollection<ProdutoErp> Produtos { get; } = new ObservableCollection<ProdutoErp>();
  }
}
