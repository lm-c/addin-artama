using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(InfoAssembly.TitleView)]
[assembly: AssemblyDescription(InfoAssembly.DescrView)]
[assembly: AssemblyConfiguration(InfoAssembly.Configuration)]
[assembly: AssemblyCompany(InfoAssembly.Company)]
[assembly: AssemblyProduct(InfoAssembly.Product)]
[assembly: AssemblyCopyright(InfoAssembly.Copyright)]
[assembly: AssemblyTrademark(InfoAssembly.Trademark)]
[assembly: AssemblyCulture(InfoAssembly.Culture)]

// Definir ComVisible como false torna os tipos neste assembly invisíveis
// para componentes COM. Caso precise acessar um tipo neste assembly de
// COM, defina o atributo ComVisible como true nesse tipo.
[assembly: ComVisible(true)]

// O GUID a seguir será destinado à ID de typelib se este projeto for exposto para COM
[assembly: Guid("923499af-9881-485f-98dc-18eb621b2f3a")]

[assembly: AssemblyVersion(InfoAssembly.Version)]
[assembly: AssemblyFileVersion(InfoAssembly.Version)]

public class InfoAssembly {
  public const string Version = "4.0.0.1";

  public const string TitleView = "Addin Leonardo Michalak";

  public const string DescrView = "Sistema de Gerenciamento de projetos, Licenciado para Artama";

  public const string Copyright = "Copyright © 2024 Leonardo Adriano Michalak. Todos os direitos reservados.";
  public const string Company = "Leonardo Adriano Michalak";
  public const string Product = "Suplemento Solidworks para Gerenciamento de Projetos";
  public const string Configuration = "";
  public const string Trademark = "";
  public const string Culture = "";
}
