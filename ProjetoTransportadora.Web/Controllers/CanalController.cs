using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class CanalController : BaseController
    {
        private CanalBusiness canalBusiness;

        public CanalController()
        {
            canalBusiness = new CanalBusiness();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Listar(CanalDto canalDto)
        {
            var lista = canalBusiness.Listar(canalDto);

            return Json(new { Sucesso = true, Mensagem = "Canal listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(CanalDto canalDto)
        {
            canalBusiness.Incluir(canalDto);

            var lista = canalBusiness.Listar();

            return Json(new { Sucesso = true, Mensagem = "Canal cadastrado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(CanalDto canalDto)
        {
            canalBusiness.Alterar(canalDto);

            var lista = canalBusiness.Listar(new CanalDto() { Id = canalDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Canal alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(CanalDto canalDto)
        {
            canalBusiness.AlterarStatus(canalDto);

            var lista = canalBusiness.Listar(new CanalDto() { Id = canalDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}