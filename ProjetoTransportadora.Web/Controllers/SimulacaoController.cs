using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class SimulacaoController : BaseController
    {
        private ParcelaBusiness parcelaBusiness;
        public SimulacaoController()
        {
            parcelaBusiness = new ParcelaBusiness();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GerarParcelas(SimulacaoDto simulacaoDto)
        {
            var lista = parcelaBusiness.Gerar(simulacaoDto);

            return Json(new { Sucesso = true, Mensagem = "Simulação gerada com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}