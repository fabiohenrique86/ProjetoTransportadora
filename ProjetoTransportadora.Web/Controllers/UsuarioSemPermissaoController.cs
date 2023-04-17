using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class UsuarioSemPermissaoController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}