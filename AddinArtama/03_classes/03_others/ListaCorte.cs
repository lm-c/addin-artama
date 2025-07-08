using LmCorbieUI;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AddinArtama {
  internal class ListaCorte {
    public int Codigo { get; set; }
    public string CodProduto { get; set; }
    public string NomeLista { get; set; }
    public string Denominacao { get; set; }
    public string Material { get; set; }
    public string Operacao { get; set; }
    public string OperacaoOrigem { get; set; }
    public double Espessura { get; set; }
    public double Largura { get; set; }
    public double Comprimento { get; set; }
    public double Massa { get; set; }
    public int Quantidade { get; set; }
    public string unidadeMedida { get; set; }
    public TipoListaMaterial Tipo { get; set; }

    public static List<ListaCorte> GetCutList(ModelDoc2 swModel, string nomePeca, out bool changeCutListName) {
      List<ListaCorte> _return = new List<ListaCorte>();
      bool boolstatus;
      changeCutListName = false;

      try {
        FeatureManager swFeatMgr = default(FeatureManager);
        Feature swFeat = default(Feature);
        string FeatType = null;
        string FeatTypeName = null;
        int bodyCount = 0;

        BodyFolder swBodyFolder = default(BodyFolder);

        Feature[] featureArr = new Feature[3];

        swFeatMgr = swModel.FeatureManager;

        var configAtiva = ((Configuration)swModel.GetActiveConfiguration()).Name;

        swFeat = (Feature)swModel.FirstFeature();

        while ((swFeat != null)) {
          ListaCorte listaCorte = new ListaCorte();

          FeatType = swFeat.Name;
          FeatTypeName = swFeat.GetTypeName2();


          if (FeatTypeName == "CutListFolder") {
            swBodyFolder = (BodyFolder)swFeat.GetSpecificFeature2();
            bodyCount = swBodyFolder.GetBodyCount();

            if (bodyCount > 0) {
              boolstatus = swModel.Extension.SelectByID2(FeatType, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

              SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
              swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);

              var isExcluded = swFeat.ExcludeFromCutList;

              if (!isExcluded) {
                var nomeLista = $"Item da lista de corte{_return.Count + 1}";
                var nomeLista2 = $"Item da lista de corte {_return.Count + 1}";
                var nomeLista3 = $"Item da lista de corte  {_return.Count + 1}";

                if (swFeat.Name != nomeLista && swFeat.Name != nomeLista2 && swFeat.Name != nomeLista3) {
                  changeCutListName = true;
                  swFeat.Name = nomeLista;
                  if (swFeat.Name != nomeLista) {
                    swFeat.Name = nomeLista2;
                    if (swFeat.Name != nomeLista2) {
                      swFeat.Name = nomeLista3;
                    }
                  }
                }

                listaCorte.NomeLista = swFeat.Name;

                CustomPropertyManager swCustPropMngr = swFeat.CustomPropertyManager;

                object[] custPropNames = (object[])swCustPropMngr.GetNames();

                if (custPropNames != null) {
                  object[] vBodies = (object[])swBodyFolder.GetBodies();

                  if ((vBodies != null)) {
                    for (int i = vBodies.GetLowerBound(0); i <= vBodies.GetUpperBound(0); i++) {
                      Body2 Body = default(Body2);
                      Body = (Body2)vBodies[i];

                      if (Body.IsSheetMetal())
                        listaCorte.Tipo = TipoListaMaterial.Chapa;
                      else
                        listaCorte.Tipo = TipoListaMaterial.Soldagem;
                    }
                  }

                  swCustPropMngr.Add3("Massa", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Mass\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
                  swCustPropMngr.Add3("Material", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Material\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                  string sValue, sResolvedvalue;

                  swBodyFolder = (BodyFolder)swFeat.GetSpecificFeature2();
                  boolstatus = swBodyFolder.SetAutomaticCutList(true);
                  boolstatus = swBodyFolder.UpdateCutList();

                  swCustPropMngr.Get2("Código", out sValue, out sResolvedvalue);
                  int.TryParse(sResolvedvalue, out int cod);
                  listaCorte.Codigo = cod;

                  swCustPropMngr.Get2("Código Produto", out sValue, out sResolvedvalue);
                  listaCorte.CodProduto = sResolvedvalue;

                  swCustPropMngr.Get2("Denominação", out sValue, out sResolvedvalue);
                  listaCorte.Denominacao = sResolvedvalue;

                  swCustPropMngr.Get2("Material", out sValue, out sResolvedvalue);
                  listaCorte.Material = sResolvedvalue;

                  swCustPropMngr.Get2("Espessura da Chapa metálica", out sValue, out sResolvedvalue);
                  double.TryParse(sResolvedvalue.Replace(".", ","), out double esp);
                  listaCorte.Espessura = Math.Round(esp, 4);

                  swCustPropMngr.Get2("COMPRIMENTO", out sValue, out sResolvedvalue);
                  var compr = sResolvedvalue;
                  //
                  if (compr.ToLower().Contains("x")) {
                    var splComp = compr.ToLower().Replace(" ", "").Split('x');
                    double.TryParse(splComp[0].Replace(".", ","), out double largura);
                    double.TryParse(splComp[1].Replace(".", ","), out double comprim);
                    listaCorte.Largura = Math.Round(largura, 4);
                    listaCorte.Comprimento = Math.Round(comprim, 4);
                    listaCorte.Tipo = TipoListaMaterial.Chapa;
                  } else if (listaCorte.Tipo == TipoListaMaterial.Chapa) {
                    string descCustProp = $"\"SW-Largura da Caixa delimitadora@@@{listaCorte.NomeLista}@{nomePeca}\" X \"SW-Comprimento da Caixa delimitadora@@@{listaCorte.NomeLista}@{nomePeca}\"";
                    swCustPropMngr.Add3("COMPRIMENTO", (int)swCustomInfoType_e.swCustomInfoText, descCustProp,
                        (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                    swCustPropMngr.Get2("COMPRIMENTO", out sValue, out sResolvedvalue);
                    compr = sResolvedvalue;
                    var splComp = compr.ToLower().Replace(" ", "").Split('x');
                    double.TryParse(splComp[0].Replace(".", ","), out double largura);
                    double.TryParse(splComp[1].Replace(".", ","), out double comprim);
                    listaCorte.Largura = Math.Round(largura, 4);
                    listaCorte.Comprimento = Math.Round(comprim, 4);
                  } else {
                    double.TryParse(sResolvedvalue.Replace(".", ","), out double comprim);
                    listaCorte.Comprimento = Math.Round(comprim, 4);
                  }

                  //swCustPropMngr.Get2("Largura da Caixa delimitadora", out sValue, out sResolvedvalue);
                  //double.TryParse(sResolvedvalue.Replace(".", ","), out double larg);
                  //listaCorte.Largura = Math.Round(larg, 4);

                  //swCustPropMngr.Get2("Comprimento da Caixa delimitadora", out sValue, out sResolvedvalue);
                  //double.TryParse(sResolvedvalue.Replace(".", ","), out double comp_cxd);
                  //listaCorte.Comprimento = Math.Round(comp_cxd, 4);

                  swCustPropMngr.Get2("QUANTITY", out sValue, out sResolvedvalue);
                  listaCorte.Quantidade = !string.IsNullOrEmpty(sResolvedvalue) ? Convert.ToInt32(sResolvedvalue) : 0;

                  if (listaCorte.Quantidade == 0) {
                    swCustPropMngr.Get2("Quantidade", out sValue, out sResolvedvalue);
                    listaCorte.Quantidade = !string.IsNullOrEmpty(sResolvedvalue) ? Convert.ToInt32(sResolvedvalue) : 0;
                  }

                  swCustPropMngr.Get2("Operação", out sValue, out sResolvedvalue);
                  listaCorte.Operacao = listaCorte.OperacaoOrigem = sResolvedvalue;

                  swCustPropMngr.Get2("Massa", out sValue, out sResolvedvalue);
                  double.TryParse(sResolvedvalue.Replace(".", ","), out double Massa);
                  listaCorte.Massa = Math.Round(Massa, 4);

                }

                // ver configs aqui para pegar sobremetal no comprimento
                if (listaCorte.Tipo == TipoListaMaterial.Soldagem) {
                  var configs = (object[])swModel.GetConfigurationNames();
                  foreach (var conf in configs) {
                    swModel.ShowConfiguration((string)conf);
                    boolstatus = swModel.Extension.SelectByID2(listaCorte.NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

                    SelectionMgr swSelMgr2 = (SelectionMgr)swModel.SelectionManager;
                    Feature swFeat2 = (Feature)swSelMgr2.GetSelectedObject6(1, 0);
                    CustomPropertyManager swCustPropMgr = swFeat2.CustomPropertyManager;

                    swCustPropMngr.Get2("COMPRIMENTO", out string sValue, out string sResolvedvalue);
                    double.TryParse(sResolvedvalue.Replace(".", ","), out double comprim);

                    if (comprim > listaCorte.Comprimento && comprim - listaCorte.Comprimento < 10) {
                      listaCorte.Comprimento = Math.Round(comprim, 4);
                    }
                  }
                }

                _return.Add(listaCorte);
              }
            }
          }
          swFeat = (Feature)swFeat.GetNextFeature();
        }

        swModel.ShowConfiguration((string)configAtiva);
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar lista corte\n\nItem: {nomePeca}\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return _return;
    }

    public static void RefreshCutList(ModelDoc2 swModel, string nomePeca, ListaCorte listaCorte) {
      bool boolstatus;

      try {
        boolstatus = swModel.Extension.SelectByID2(listaCorte.NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

        SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
        Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
        CustomPropertyManager swCustPropMngr = swFeat.CustomPropertyManager;
        string FeatType = null;
        BodyFolder swBodyFolder = default(BodyFolder);

        if (swFeat != null) {
          var isExcluded = swFeat.ExcludeFromCutList;
          if (!isExcluded) {
            object[] custPropNames = (object[])swCustPropMngr.GetNames();
            if (custPropNames != null) {

              swBodyFolder = (BodyFolder)swFeat.GetSpecificFeature2();
              boolstatus = swBodyFolder.SetAutomaticCutList(true);
              boolstatus = swBodyFolder.UpdateCutList();

              object[] vBodies = (object[])swBodyFolder.GetBodies();

              if ((vBodies != null)) {
                for (int i = vBodies.GetLowerBound(0); i <= vBodies.GetUpperBound(0); i++) {
                  Body2 Body = default(Body2);
                  Body = (Body2)vBodies[i];

                  if (Body.IsSheetMetal())
                    listaCorte.Tipo = TipoListaMaterial.Chapa;
                  else
                    listaCorte.Tipo = TipoListaMaterial.Soldagem;


                  swCustPropMngr.Add3("Massa", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Mass\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
                  swCustPropMngr.Add3("Material", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Material\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                  string sValue, sResolvedvalue;

                  swCustPropMngr.Get2("Código", out sValue, out sResolvedvalue);
                  int.TryParse(sResolvedvalue, out int cod);
                  listaCorte.Codigo = cod;

                  swCustPropMngr.Get2("Código Produto", out sValue, out sResolvedvalue);
                  listaCorte.CodProduto = sResolvedvalue;

                  swCustPropMngr.Get2("Denominação", out sValue, out sResolvedvalue);
                  listaCorte.Denominacao = sResolvedvalue;

                  swCustPropMngr.Get2("Material", out sValue, out sResolvedvalue);
                  listaCorte.Material = sResolvedvalue;

                  swCustPropMngr.Get2("Espessura da Chapa metálica", out sValue, out sResolvedvalue);
                  double.TryParse(sResolvedvalue.Replace(".", ","), out double esp);
                  listaCorte.Espessura = Math.Round(esp, 4);

                  swCustPropMngr.Get2("COMPRIMENTO", out sValue, out sResolvedvalue);
                  var compr = sResolvedvalue;
                  //double.TryParse(sResolvedvalue.Replace(".", ","), out double compr);
                  if (compr.ToLower().Contains("x")) {
                    var splComp = compr.ToLower().Replace(" ", "").Split('x');
                    double.TryParse(splComp[0].Replace(".", ","), out double largura);
                    double.TryParse(splComp[1].Replace(".", ","), out double comprim);
                    listaCorte.Largura = Math.Round(largura, 4);
                    listaCorte.Comprimento = Math.Round(comprim, 4);
                    listaCorte.Tipo = TipoListaMaterial.Chapa;
                  } else if (listaCorte.Tipo == TipoListaMaterial.Chapa) {
                    string descCustProp = $"\"SW-Largura da Caixa delimitadora@@@{listaCorte.NomeLista}@{nomePeca}\" X \"SW-Comprimento da Caixa delimitadora@@@{listaCorte.NomeLista}@{nomePeca}\"";
                    swCustPropMngr.Add3("COMPRIMENTO", (int)swCustomInfoType_e.swCustomInfoText, descCustProp,
                        (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                    swCustPropMngr.Get2("COMPRIMENTO", out sValue, out sResolvedvalue);
                    compr = sResolvedvalue;
                    var splComp = compr.ToLower().Replace(" ", "").Split('x');
                    double.TryParse(splComp[0].Replace(".", ","), out double largura);
                    double.TryParse(splComp[1].Replace(".", ","), out double comprim);
                    listaCorte.Largura = Math.Round(largura, 4);
                    listaCorte.Comprimento = Math.Round(comprim, 4);
                  }

                  //swCustPropMngr.Get2("Largura da Caixa delimitadora", out sValue, out sResolvedvalue);
                  //double.TryParse(sResolvedvalue, NumberStyles.Any, CultureInfo.InvariantCulture, out double larg);
                  //listaCorte.CxdLarg = Math.Round(larg, 4);

                  //swCustPropMngr.Get2("Comprimento da Caixa delimitadora", out sValue, out sResolvedvalue);
                  //double.TryParse(sResolvedvalue, NumberStyles.Any, CultureInfo.InvariantCulture, out double comp);
                  //listaCorte.CxdCompr = comp > 0 ? Math.Round(comp, 4) : Math.Round(compr, 4);

                  swCustPropMngr.Get2("QUANTITY", out sValue, out sResolvedvalue);
                  listaCorte.Quantidade = !string.IsNullOrEmpty(sResolvedvalue) ? Convert.ToInt32(sResolvedvalue) : 0;

                  if (listaCorte.Quantidade == 0) {
                    swCustPropMngr.Get2("Quantidade", out sValue, out sResolvedvalue);
                    listaCorte.Quantidade = !string.IsNullOrEmpty(sResolvedvalue) ? Convert.ToInt32(sResolvedvalue) : 0;
                  }

                  swCustPropMngr.Get2("Operação", out sValue, out sResolvedvalue);
                  listaCorte.Operacao = listaCorte.OperacaoOrigem = sResolvedvalue;

                  swCustPropMngr.Get2("Massa", out sValue, out sResolvedvalue);
                  double.TryParse(sResolvedvalue, NumberStyles.Any, CultureInfo.InvariantCulture, out double massa);
                  listaCorte.Massa = Math.Round(massa, 4);

                  // ver configs aqui para pegar sobremetal no comprimento
                  if (listaCorte.Tipo == TipoListaMaterial.Soldagem) {
                    var configs = (object[])swModel.GetConfigurationNames();
                    foreach (var conf in configs) {
                      swModel.ShowConfiguration((string)conf);
                      boolstatus = swModel.Extension.SelectByID2(listaCorte.NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

                      SelectionMgr swSelMgr2 = (SelectionMgr)swModel.SelectionManager;
                      Feature swFeat2 = (Feature)swSelMgr2.GetSelectedObject6(1, 0);
                      CustomPropertyManager swCustPropMgr = swFeat2.CustomPropertyManager;

                      swCustPropMngr.Get2("COMPRIMENTO", out  sValue, out  sResolvedvalue);
                      double.TryParse(sResolvedvalue.Replace(".", ","), out double comprim);

                      if (comprim > listaCorte.Comprimento && comprim - listaCorte.Comprimento < 500) {
                        listaCorte.Comprimento = Math.Round(comprim, 4);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar lista corte\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

    }

    public static void UpdateCutList(ModelDoc2 swModel, ListaCorte listaCorte) {
      bool boolstatus = swModel.Extension.SelectByID2(listaCorte.NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

      SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
      Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
      CustomPropertyManager swCustPropMngr = swFeat.CustomPropertyManager;

      swCustPropMngr.Add3("Operação", (int)swCustomInfoType_e.swCustomInfoText, Convert.ToString(listaCorte.Operacao), (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

      if (listaCorte.Tipo == TipoListaMaterial.Chapa) {
        string nomePeca = Path.GetFileName(swModel.GetPathName());

        swCustPropMngr.Add3("Código", (int)swCustomInfoType_e.swCustomInfoText, listaCorte.Codigo.ToString(), (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        swCustPropMngr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, listaCorte.CodProduto.ToString(), (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        swCustPropMngr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText, listaCorte.Denominacao.ToString(), (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        swCustPropMngr.Add3("COMPRIMENTO", (int)swCustomInfoType_e.swCustomInfoText,
            $"\"SW-Largura da Caixa delimitadora@@@{listaCorte.NomeLista}@{nomePeca}\" X \"SW-Comprimento da Caixa delimitadora@@@{listaCorte.NomeLista}@{nomePeca}\"",
            (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
      }
    }

    public static void DeletarPropriedadeDaLista(ModelDoc2 swModel, ListaCorte CutList, string nomePropriedade) {
      bool boolstatus = swModel.Extension.SelectByID2(CutList.NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

      SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
      Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
      CustomPropertyManager swCustPropMngr = swFeat.CustomPropertyManager;

      swCustPropMngr.Delete2(nomePropriedade);
    }

    public static void ExcluirLista(ModelDoc2 swModel) {
      try {
        Feature swFeat = (Feature)swModel.FirstFeature();

        while ((swFeat != null)) {
          Debug.Print(swFeat.Name + " [" + swFeat.GetTypeName2() + "]");

          if (swFeat.GetTypeName() == "TableFolder") {
            Feature swSubFeat = (Feature)swFeat.GetFirstSubFeature();

            if ((swSubFeat != null)) {
              Debug.Print("    " + swSubFeat.Name + " [" + swSubFeat.GetTypeName() + "]");
              bool boolstatus = swModel.Extension.SelectByID2(swSubFeat.Name, "BOMFEATURE", 0, 0, 0, false, 0, null, 0);
              swModel.EditDelete();
              break;
            }
          }
          swFeat = (Feature)swFeat.GetNextFeature();
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Excluir Lista\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
