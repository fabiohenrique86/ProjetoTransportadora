using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// As informações gerais sobre um assembly são controladas através do seguinte
// conjunto de atributos a seguir. Altere esses valores de atributo para modificar as informações
// associadas a um assembly.
[assembly: AssemblyTitle("ProjetoTransportadora.Web")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("ProjetoTransportadora.Web")]
[assembly: AssemblyCopyright("Copyright ©  2023")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Definir ComVisible como falso torna não visíveis os tipos neste assembly
// para componentes COM. Caso precise acessar um tipo neste assembly a partir de
// COM, defina o atributo ComVisible como true nesse tipo.
[assembly: ComVisible(false)]

// A GUID a seguir será referente à ID do typelib se este projeto for exposto ao COM
[assembly: Guid("b5383717-f66c-4a67-b822-2610bdb97ab6")]

// As informações de versão de um assembly consistem nos seguintes quatro valores:
//
//      Versão Principal
//      Versão Secundária
//      Número da Versão
//      Revisão
//
// É possível especificar todos os valores ou definir como padrão os números de revisão e de versão
// usando o '*' como mostrado abaixo:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

#if !(DEBUG)
[assembly: AllowPartiallyTrustedCallers]
#endif