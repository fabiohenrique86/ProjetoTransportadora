using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class GrupoController : BaseController
    {
        private GrupoBusiness grupoBusiness;
        private FuncionalidadeBusiness funcionalidadeBusiness;

        public GrupoController()
        {
            grupoBusiness = new GrupoBusiness();
            funcionalidadeBusiness = new FuncionalidadeBusiness();
        }

        public ActionResult Index()
        {
            ViewBag.Funcionalidade = funcionalidadeBusiness.Listar();

            return View();
        }


        [HttpGet]
        public JsonResult Listar(GrupoDto grupoDto)
        {
            var lista = grupoBusiness.Listar(grupoDto);

            return Json(new { Sucesso = true, Mensagem = "Grupo listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(GrupoDto grupoDto)
        {
            grupoBusiness.Incluir(grupoDto);

            var lista = grupoBusiness.Listar();

            return Json(new { Sucesso = true, Mensagem = "Grupo cadastrado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(GrupoDto grupoDto)
        {
            grupoBusiness.Alterar(grupoDto);
            
            var lista = grupoBusiness.Listar(new GrupoDto() { Id = grupoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Grupo alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(GrupoDto grupoDto)
        {
            grupoBusiness.AlterarStatus(grupoDto);

            var lista = grupoBusiness.Listar(new GrupoDto() { Id = grupoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}