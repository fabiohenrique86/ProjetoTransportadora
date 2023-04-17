using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class SituacaoVeiculoController : BaseController
    {
        private SituacaoVeiculoBusiness situacaoVeiculoBusiness;

        public SituacaoVeiculoController()
        {
            situacaoVeiculoBusiness = new SituacaoVeiculoBusiness();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Listar(SituacaoVeiculoDto situacaoVeiculoDto)
        {
            var lista = situacaoVeiculoBusiness.Listar(situacaoVeiculoDto);

            return Json(new { Sucesso = true, Mensagem = "SituacaoVeiculo listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(SituacaoVeiculoDto situacaoVeiculoDto)
        {
            situacaoVeiculoBusiness.Incluir(situacaoVeiculoDto);

            var lista = situacaoVeiculoBusiness.Listar();

            return Json(new { Sucesso = true, Mensagem = "SituacaoVeiculo cadastrado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(SituacaoVeiculoDto situacaoVeiculoDto)
        {
            situacaoVeiculoBusiness.Alterar(situacaoVeiculoDto);

            var lista = situacaoVeiculoBusiness.Listar(new SituacaoVeiculoDto() { Id = situacaoVeiculoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "SituacaoVeiculo alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(SituacaoVeiculoDto situacaoVeiculoDto)
        {
            situacaoVeiculoBusiness.AlterarStatus(situacaoVeiculoDto);

            var lista = situacaoVeiculoBusiness.Listar(new SituacaoVeiculoDto() { Id = situacaoVeiculoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}