using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class SimulacaoController : BaseController
    {
        private ContratoParcelaBusiness parcelaBusiness;
        public SimulacaoController()
        {
            parcelaBusiness = new ContratoParcelaBusiness();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GerarParcelas(SimulacaoDto simulacaoDto)
        {
            simulacaoDto.ContratoParcelaDto = parcelaBusiness.Gerar(simulacaoDto);

            TempData["SimulacaoDto"] = simulacaoDto;
            TempData.Keep("SimulacaoDto");

            return Json(new { Sucesso = true, Mensagem = "Simulação gerada com sucesso", Data = simulacaoDto.ContratoParcelaDto }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GerarContrato()
        {
            TempData["GerarContrato"] = true;
            TempData.Keep("GerarContrato");
            TempData.Keep("SimulacaoDto");

            return Json(new { Sucesso = true, Mensagem = "Simulação gerada com sucesso" }, JsonRequestBehavior.AllowGet);
        }
    }
}