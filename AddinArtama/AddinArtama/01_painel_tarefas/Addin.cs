using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AddinArtama {
  public class Addin : ISwAddin {
    static Addin() {
      AppDomain.CurrentDomain.AssemblyResolve += ResolveDependencyFromAddinFolder;
    }

    // O add-in roda dentro do processo SLDWORKS.exe, que ignora o app.config/bindingRedirect
    // do AddinArtama. LmCorbieUI.dll pode estar compilada contra uma revisão diferente do
    // itextsharp da que está deployada; aqui forçamos o carregamento da DLL que está ao lado
    // do AddinArtama.dll, independente da revisão exata solicitada.
    private static Assembly ResolveDependencyFromAddinFolder(object sender, ResolveEventArgs args) {
      string assemblyName = new AssemblyName(args.Name).Name;
      string addinFolder = Path.GetDirectoryName(typeof(Addin).Assembly.Location);
      string candidatePath = Path.Combine(addinFolder, assemblyName + ".dll");

      if (File.Exists(candidatePath)) {
        return Assembly.LoadFrom(candidatePath);
      }

      return null;
    }

    [ComRegisterFunction()]
    private static void ComRegister(Type t) {
      string keyPath = string.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
      using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyPath)) {
        rk.SetValue(null, 1);
        rk.SetValue("Title", "Addin Axion Artama");
        rk.SetValue("Description", "Gerenciador de Projetos Artama");
      }
    }

    [ComUnregisterFunction()]
    private static void ComUnregister(Type t) {
      string keyPath = string.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
      Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath);
    }

    public SldWorks mSWApplication;
    private int mSWCookie;

    public bool ConnectToSW(object ThisSW, int Cookie) {
      mSWApplication = (SldWorks)ThisSW;
      mSWCookie = Cookie;

      bool result = mSWApplication.SetAddinCallbackInfo(0, this, Cookie);

      UISetup();
      return true;
    }

    public bool DisconnectFromSW() {
      UITeardown();
      return true;
    }

    private TaskpaneView mTaskpaneView;
    private UcPainelTarefas mPainelTarefas;

    private void UISetup() {
      string icon = @"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\01 - Addin LM\IconTaskpanel.png";
      mTaskpaneView = mSWApplication.CreateTaskpaneView2(icon, "Addin Artama Axion");

      // Janelas WPF (LmWindow, MsgBox) abrem centralizadas e na frente do SolidWorks.
      var frame = (Frame)mSWApplication.Frame();
      LmCorbieUI.Wpf.LmWindow.DefinirDonoPadrao(new IntPtr(frame.GetHWndx64()));

      mPainelTarefas = (UcPainelTarefas)mTaskpaneView.AddControl(UcPainelTarefas.SWTASKPANE_PROGID, "");
    }

    private void UITeardown() {
      mPainelTarefas = null;
      mTaskpaneView.DeleteView();
      Marshal.ReleaseComObject(mTaskpaneView);
      mTaskpaneView = null;
    }
  }
}
