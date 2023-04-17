using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class EstadoCivilController : BaseController
    {
        private EstadoCivilBusiness estadoCivilBusiness;

        public EstadoCivilController()
        {
            estadoCivilBusiness = new EstadoCivilBusiness();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Listar(EstadoCivilDto estadoCivilDto)
        {
            var lista = estadoCivilBusiness.Listar(estadoCivilDto);

            return Json(new { Sucesso = true, Mensagem = "EstadoCivil listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(EstadoCivilDto estadoCivilDto)
        {
            estadoCivilBusiness.Incluir(estadoCivilDto);

            var lista = estadoCivilBusiness.Listar();

            return Json(new { Sucesso = true, Mensagem = "EstadoCivil cadastrado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(EstadoCivilDto estadoCivilDto)
        {
            estadoCivilBusiness.Alterar(estadoCivilDto);

            var lista = estadoCivilBusiness.Listar(new EstadoCivilDto() { Id = estadoCivilDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "EstadoCivil alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(EstadoCivilDto estadoCivilDto)
        {
            estadoCivilBusiness.AlterarStatus(estadoCivilDto);

            var lista = estadoCivilBusiness.Listar(new EstadoCivilDto() { Id = estadoCivilDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}