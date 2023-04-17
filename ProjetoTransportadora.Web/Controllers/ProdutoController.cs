using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class ProdutoController : BaseController
    {
        private ProdutoBusiness produtoBusiness;

        public ProdutoController()
        {
            produtoBusiness = new ProdutoBusiness();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Listar(ProdutoDto produtoDto)
        {
            var lista = produtoBusiness.Listar(produtoDto);

            return Json(new { Sucesso = true, Mensagem = "Produto listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(ProdutoDto produtoDto)
        {
            produtoBusiness.Incluir(produtoDto);

            var lista = produtoBusiness.Listar();

            return Json(new { Sucesso = true, Mensagem = "Produto cadastrado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(ProdutoDto produtoDto)
        {
            produtoBusiness.Alterar(produtoDto);

            var lista = produtoBusiness.Listar(new ProdutoDto() { Id = produtoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Produto alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(ProdutoDto produtoDto)
        {
            produtoBusiness.AlterarStatus(produtoDto);

            var lista = produtoBusiness.Listar(new ProdutoDto() { Id = produtoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}