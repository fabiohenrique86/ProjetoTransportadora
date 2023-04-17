using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class RegimeCasamentoController : BaseController
    {
        private RegimeCasamentoBusiness regimeCasamentoBusiness;

        public RegimeCasamentoController()
        {
            regimeCasamentoBusiness = new RegimeCasamentoBusiness();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Listar(RegimeCasamentoDto regimeCasamentoDto)
        {
            var lista = regimeCasamentoBusiness.Listar(regimeCasamentoDto);

            return Json(new { Sucesso = true, Mensagem = "RegimeCasamento listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(RegimeCasamentoDto regimeCasamentoDto)
        {
            regimeCasamentoBusiness.Incluir(regimeCasamentoDto);

            var lista = regimeCasamentoBusiness.Listar();

            return Json(new { Sucesso = true, Mensagem = "RegimeCasamento cadastrado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(RegimeCasamentoDto regimeCasamentoDto)
        {
            regimeCasamentoBusiness.Alterar(regimeCasamentoDto);

            var lista = regimeCasamentoBusiness.Listar(new RegimeCasamentoDto() { Id = regimeCasamentoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "RegimeCasamento alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(RegimeCasamentoDto regimeCasamentoDto)
        {
            regimeCasamentoBusiness.AlterarStatus(regimeCasamentoDto);

            var lista = regimeCasamentoBusiness.Listar(new RegimeCasamentoDto() { Id = regimeCasamentoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}