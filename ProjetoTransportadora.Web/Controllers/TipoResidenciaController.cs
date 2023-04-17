using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class TipoResidenciaController : BaseController
    {
        private TipoResidenciaBusiness tipoResidenciaBusiness;

        public TipoResidenciaController()
        {
            tipoResidenciaBusiness = new TipoResidenciaBusiness();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Listar(TipoResidenciaDto tipoResidenciaDto)
        {
            var lista = tipoResidenciaBusiness.Listar(tipoResidenciaDto);

            return Json(new { Sucesso = true, Mensagem = "TipoResidencia listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(TipoResidenciaDto tipoResidenciaDto)
        {
            tipoResidenciaBusiness.Incluir(tipoResidenciaDto);

            var lista = tipoResidenciaBusiness.Listar();

            return Json(new { Sucesso = true, Mensagem = "TipoResidencia cadastrado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(TipoResidenciaDto tipoResidenciaDto)
        {
            tipoResidenciaBusiness.Alterar(tipoResidenciaDto);

            var lista = tipoResidenciaBusiness.Listar(new TipoResidenciaDto() { Id = tipoResidenciaDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "TipoResidencia alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(TipoResidenciaDto tipoResidenciaDto)
        {
            tipoResidenciaBusiness.AlterarStatus(tipoResidenciaDto);

            var lista = tipoResidenciaBusiness.Listar(new TipoResidenciaDto() { Id = tipoResidenciaDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}