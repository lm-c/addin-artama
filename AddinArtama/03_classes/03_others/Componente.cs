using LmCorbieUI;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;

namespace AddinArtama {
  internal class Componente {
    public string Nivel { get; set; }
    public string CompCodInterno { get; set; }
    public string Denominacao { get; set; }
    public string ShortName { get; set; }
    public string LongName { get; set; }
    public string ConfigName { get; set; }
    public double Massa { get; set; }
    public string Operacao { get; set; }
    public string OperacaoOrigem { get; set; }
    public bool Interno { get; set; }
    public int Quantidade { get; set; }

    public List<ListaCorte> ItensCorte { get; set; }

    public static Componente GetComponente(ModelDoc2 swModel) {
      Componente _return = new Componente();

      try {
        object[] ativeConfiguration = null;
        string valOut;
        string resolvedValOut;
        double[] massProp;

        var swModelDocExt = swModel.Extension;

        swModel.Rebuild((int)swRebuildOptions_e.swRebuildAll);

        ConfigurationManager swConfMgr;
        Configuration swConf;

        swConfMgr = swModel.ConfigurationManager;
        swConf = swConfMgr.ActiveConfiguration;
        ativeConfiguration = (object[])swModel.GetConfigurationNames();

        var swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");

        swCustPropMngr.Get2("Componente", out valOut, out resolvedValOut);
        _return.CompCodInterno = resolvedValOut;
        swCustPropMngr.Get2("Denominação", out valOut, out resolvedValOut);
        _return.Denominacao = resolvedValOut;
        swCustPropMngr.Get2("Operação", out valOut, out resolvedValOut);
        _return.Operacao = _return.OperacaoOrigem = resolvedValOut;
        swCustPropMngr.Get2("Interno", out valOut, out resolvedValOut);
        _return.Interno = resolvedValOut == "Sim";

        _return.Quantidade= 1;
        _return.LongName = swModel.GetPathName();
        _return.ShortName = Path.GetFileNameWithoutExtension(_return.LongName);
        _return.ConfigName = swConf.Name;

        massProp = (double[])swModelDocExt.GetMassProperties(1, 0);

        if (massProp != null)
          _return.Massa = Math.Round(massProp[5], 3);

        _return.ItensCorte = ListaCorte.GetCutList(swModel, _return.LongName, out _);
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Caregar Componente");
      }

      return _return;
    }

  }
}
