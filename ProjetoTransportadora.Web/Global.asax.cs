using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ProjetoTransportadora.ModelBinder;

namespace ProjetoTransportadora.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(double), new DoubleModelBinder());
            ModelBinders.Binders.Add(typeof(double?), new DoubleModelBinder());
        }
    }
}
