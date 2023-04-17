using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class PermissaoController : BaseController
    {
        private GrupoBusiness grupoBusiness;
        private GrupoFuncionalidadeBusiness grupoFuncionalidadeBusiness;

        public PermissaoController()
        {
            grupoBusiness = new GrupoBusiness();
            grupoFuncionalidadeBusiness = new GrupoFuncionalidadeBusiness();
        }

        public ActionResult Index()
        {
            ViewBag.Grupo = grupoBusiness.Listar(new GrupoDto() { Ativo = true });

            return View();
        }

        [HttpGet]
        public JsonResult Listar(GrupoFuncionalidadeDto grupoFuncionalidadeDto)
        {
            var lista = grupoFuncionalidadeBusiness.Listar(grupoFuncionalidadeDto);

            return Json(new { Sucesso = true, Mensagem = "GrupoFuncionalidade listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(GrupoFuncionalidadeDto grupoFuncionalidadeDto)
        {
            grupoFuncionalidadeBusiness.AlterarStatus(grupoFuncionalidadeDto);

            var lista = grupoFuncionalidadeBusiness.Listar(new GrupoFuncionalidadeDto() { Id = grupoFuncionalidadeDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}