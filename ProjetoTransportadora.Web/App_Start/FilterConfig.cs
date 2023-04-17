using ProjetoTransportadora.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new Filters.ProjetoTransportadoraHandleErrorAttribute());
            filters.Add(new Filters.ProjetoTransportadoraHandleRequestAttribute());
            filters.Add(new System.Web.Mvc.HandleErrorAttribute());            
        }
    }
}
