using ProjetoTransportadora.Dto;
using System.Configuration;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class UsuarioTrocarSenhaController : BaseController
    {
        public ActionResult Index()
        {
            if (TempData[ConfigurationManager.AppSettings["ProjetoTransportadora.UsuarioTrocarSenha"]] == null)
                return RedirectToAction("Index", "Home");

            TempData.Keep(ConfigurationManager.AppSettings["ProjetoTransportadora.UsuarioTrocarSenha"]);
            ViewBag.IdUsuario = ((UsuarioDto)TempData[ConfigurationManager.AppSettings["ProjetoTransportadora.UsuarioTrocarSenha"]]).Id;

            return View();
        }
    }
}