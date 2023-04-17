using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (UsuarioLogado() != null)
                return RedirectToAction("Index", "Transportadora");

            return View();
        }
    }
}