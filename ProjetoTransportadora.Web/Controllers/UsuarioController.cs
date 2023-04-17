using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class UsuarioController : BaseController
    {
        private UsuarioBusiness usuarioBusiness;
        private GrupoBusiness grupoBusiness;

        public UsuarioController()
        {
            usuarioBusiness = new UsuarioBusiness();
            grupoBusiness = new GrupoBusiness();
        }

        public ActionResult Index()
        {
            ViewBag.Grupo = grupoBusiness.Listar(new GrupoDto() { Ativo = true });

            return View();
        }

        [HttpPost]
        public JsonResult Login(UsuarioDto usuarioDto)
        {
            var usuario = usuarioBusiness.Login(usuarioDto);

            if (usuario.TrocarSenha.GetValueOrDefault())
            {
                usuario.Senha = null;
                Session[ConfigurationManager.AppSettings["ProjetoTransportadoraSessionUsuarioTrocarSenha"]] = usuario;

                return Json(new { Sucesso = false, Mensagem = "Usuário necessita trocar a senha", Data = "", TrocarSenha = true }, JsonRequestBehavior.AllowGet);
            }

            LogIn(usuario);

            return Json(new { Sucesso = true, Mensagem = "Usuário logado com sucesso", Data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sair()
        {
            LogOut();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult AlterarSenha(UsuarioDto usuarioDto)
        {
            usuarioBusiness.Alterar(usuarioDto);

            LogIn((UsuarioDto)Session[ConfigurationManager.AppSettings["ProjetoTransportadoraSessionUsuarioTrocarSenha"]]);

            return Json(new { Sucesso = true, Mensagem = "Usuário logado com sucesso", Data = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Listar(UsuarioDto usuarioDto)
        {
            var lista = usuarioBusiness.Listar(usuarioDto);

            return Json(new { Sucesso = true, Mensagem = "Usuário listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(UsuarioDto usuarioDto)
        {
            usuarioBusiness.Incluir(usuarioDto);

            var lista = usuarioBusiness.Listar();

            return Json(new { Sucesso = true, Mensagem = "Usuário cadastrado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(UsuarioDto usuarioDto)
        {
            var atualizarGrupos = false;

            usuarioBusiness.Alterar(usuarioDto);

            var lista = usuarioBusiness.Listar(new UsuarioDto() { Id = usuarioDto.Id }).FirstOrDefault();

            // se usuário estiver alterando o próprio usuário, deve atualizar a session
            if (UsuarioLogado().Id == usuarioDto.Id)
            {
                atualizarGrupos = true;
                LogIn(lista);
            }

            return Json(new { Sucesso = true, Mensagem = "Usuário alterado com sucesso", Data = lista, AtualizarGrupos = atualizarGrupos }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(UsuarioDto usuarioDto)
        {
            usuarioBusiness.AlterarStatus(usuarioDto);

            var lista = usuarioBusiness.Listar(new UsuarioDto() { Id = usuarioDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}