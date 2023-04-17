using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class MontadoraController : BaseController
    {
        private MontadoraBusiness montadoraBusiness;

        public MontadoraController()
        {
            montadoraBusiness = new MontadoraBusiness();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Listar(MontadoraDto montadoraDto)
        {
            var lista = montadoraBusiness.Listar(montadoraDto);

            return Json(new { Sucesso = true, Mensagem = "Montadora listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(MontadoraDto montadoraDto)
        {
            montadoraBusiness.Incluir(montadoraDto);

            var lista = montadoraBusiness.Listar();

            return Json(new { Sucesso = true, Mensagem = "Montadora cadastrado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(MontadoraDto montadoraDto)
        {
            montadoraBusiness.Alterar(montadoraDto);

            var lista = montadoraBusiness.Listar(new MontadoraDto() { Id = montadoraDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Montadora alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(MontadoraDto montadoraDto)
        {
            montadoraBusiness.AlterarStatus(montadoraDto);

            var lista = montadoraBusiness.Listar(new MontadoraDto() { Id = montadoraDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}