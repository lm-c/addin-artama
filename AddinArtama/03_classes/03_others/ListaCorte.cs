using LmCorbieUI;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddinArtama {
  internal class ListaCorte {
    public int Codigo { get; set; }
    public string NomeLista { get; set; }
    public string Denominacao { get; set; }
    public string Material { get; set; } 
    public string Comprimento { get; set; }
    public string Operacao { get; set; }
    public string OperacaoOrigem { get; set; }
    public double CxdEspess { get; set; }
    public double CxdLarg { get; set; }
    public double CxdCompr { get; set; }
    public double Massa { get; set; }
    public int Quantidade { get; set; }
    public TipoListaMaterial Tipo { get; set; }

    public static List<ListaCorte> GetCutList(ModelDoc2 swModel, Componente componente) {
      List<ListaCorte> _return = new List<ListaCorte>();
      bool boolstatus;
      try {
        FeatureManager swFeatMgr = default(FeatureManager);
        Feature swFeat = default(Feature);
        string FeatType = null;
        string FeatTypeName = null;
        int bodyCount = 0;

        BodyFolder swBodyFolder = default(BodyFolder);

        Feature[] featureArr = new Feature[3];

        swFeatMgr = swModel.FeatureManager;

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

              CustomPropertyManager oCustPropMngr = swFeat.CustomPropertyManager;

              object[] custPropNames = (object[])oCustPropMngr.GetNames();

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

                oCustPropMngr.Add3("Massa", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Mass\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
                oCustPropMngr.Add3("Material", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Material\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                string sValue, sResolvedvalue;

                swBodyFolder = (BodyFolder)swFeat.GetSpecificFeature2();
                boolstatus = swBodyFolder.SetAutomaticCutList(true);
                boolstatus = swBodyFolder.UpdateCutList();

                listaCorte.NomeLista = FeatType;
                oCustPropMngr.Get2("Código", out sValue, out sResolvedvalue);
                int.TryParse(sResolvedvalue, out int cod);
                listaCorte.Codigo = cod;

                oCustPropMngr.Get2("Denominação", out sValue, out sResolvedvalue);
                listaCorte.Denominacao = sResolvedvalue;
                
                oCustPropMngr.Get2("Material", out sValue, out sResolvedvalue);
                listaCorte.Material = sResolvedvalue;

                oCustPropMngr.Get2("COMPRIMENTO", out sValue, out sResolvedvalue);
                listaCorte.Comprimento = sResolvedvalue;

                oCustPropMngr.Get2("QUANTITY", out sValue, out sResolvedvalue);
                listaCorte.Quantidade = !string.IsNullOrEmpty(sResolvedvalue) ? Convert.ToInt32(sResolvedvalue) : 0;

                if (listaCorte.Quantidade == 0) {
                  oCustPropMngr.Get2("Quantidade", out sValue, out sResolvedvalue);
                  listaCorte.Quantidade = !string.IsNullOrEmpty(sResolvedvalue) ? Convert.ToInt32(sResolvedvalue) : 0;
                }

                if (listaCorte.Comprimento.Contains(" X ") && listaCorte.Tipo == TipoListaMaterial.Soldagem) {
                  listaCorte.Tipo = TipoListaMaterial.Chapa;
                }
                oCustPropMngr.Get2("Operação", out sValue, out sResolvedvalue);
                listaCorte.Operacao = listaCorte.OperacaoOrigem = sResolvedvalue;

                oCustPropMngr.Get2("Espessura da Chapa metálica", out sValue, out sResolvedvalue);
                double.TryParse(sResolvedvalue.Replace(".", ","), out double esp);
                listaCorte.CxdEspess = Math.Round(esp, 3);

                oCustPropMngr.Get2("Largura da Caixa delimitadora", out sValue, out sResolvedvalue);
                double.TryParse(sResolvedvalue.Replace(".", ","), out double larg);
                listaCorte.CxdLarg = Math.Round(larg, 3);

                oCustPropMngr.Get2("Comprimento da Caixa delimitadora", out sValue, out sResolvedvalue);
                double.TryParse(sResolvedvalue.Replace(".", ","), out double comp);
                listaCorte.CxdCompr = Math.Round(comp, 3);

                oCustPropMngr.Get2("Massa", out sValue, out sResolvedvalue);
                double.TryParse(sResolvedvalue.Replace(".", ","), out double Massa);
                listaCorte.Massa = Math.Round(Massa, 3);

                if (listaCorte.Tipo == TipoListaMaterial.Chapa) {
                  string nomePeca = Path.GetFileName(componente.LongName);
                  string descCustProp = $"\"SW-Largura da Caixa delimitadora@@@{listaCorte.NomeLista}@{nomePeca}\" X \"SW-Comprimento da Caixa delimitadora@@@{listaCorte.NomeLista}@{nomePeca}\"";
                  oCustPropMngr.Add3("COMPRIMENTO", (int)swCustomInfoType_e.swCustomInfoText, descCustProp,
                      (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                  listaCorte.Comprimento = listaCorte.CxdLarg + " X " + listaCorte.CxdCompr;
                }
              }

              _return.Add(listaCorte);
            }
          }
          swFeat = (Feature)swFeat.GetNextFeature();
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar lista corte\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return _return;
    }

    public static void AtualizarListaCorte(ModelDoc2 swModel, ListaCorte CutList) {
      swModel = (ModelDoc2)Sw.App.ActiveDoc;
      bool boolstatus = swModel.Extension.SelectByID2(CutList.NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

      SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
      Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
      CustomPropertyManager oCustPropMngr = swFeat.CustomPropertyManager;

      oCustPropMngr.Add3("Operação", (int)swCustomInfoType_e.swCustomInfoText, Convert.ToString(CutList.Operacao), (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

      if (CutList.Tipo == TipoListaMaterial.Chapa) {
        string nomePeca = Path.GetFileName(swModel.GetPathName());

        oCustPropMngr.Add3("Código", (int)swCustomInfoType_e.swCustomInfoText, CutList.Codigo.ToString(), (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        oCustPropMngr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText, CutList.Denominacao.ToString(), (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        oCustPropMngr.Add3("COMPRIMENTO", (int)swCustomInfoType_e.swCustomInfoText,
            $"\"SW-Largura da Caixa delimitadora@@@{CutList.NomeLista}@{nomePeca}\" X \"SW-Comprimento da Caixa delimitadora@@@{CutList.NomeLista}@{nomePeca}\"",
            (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
      }

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
