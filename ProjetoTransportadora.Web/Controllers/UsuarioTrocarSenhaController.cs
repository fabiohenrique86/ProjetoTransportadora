using System.Configuration;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class UsuarioTrocarSenhaController : BaseController
    {
        public ActionResult Index()
        {
            if (Session[ConfigurationManager.AppSettings["ProjetoTransportadoraSessionUsuarioTrocarSenha"]] == null)
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}