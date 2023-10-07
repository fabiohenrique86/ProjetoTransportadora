using ProjetoTransportadora.Business;
using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class ParcelaController : BaseController
    {
        private SituacaoContratoBusiness situacaoContratoBusiness;
        private SituacaoParcelaBusiness situacaoParcelaBusiness;
        private ProdutoBusiness produtoBusiness;
        public ParcelaController()
        {
            situacaoContratoBusiness = new SituacaoContratoBusiness();
            situacaoParcelaBusiness = new SituacaoParcelaBusiness();
            produtoBusiness = new ProdutoBusiness();
        }
        public ActionResult Index()
        {
            ViewBag.SituacaoContrato = situacaoContratoBusiness.Listar(new SituacaoContratoDto() { Ativo = true });
            ViewBag.SituacaoParcela = situacaoParcelaBusiness.Listar(new SituacaoParcelaDto() { Ativo = true });
            ViewBag.Produto = produtoBusiness.Listar(new ProdutoDto() { Ativo = true });

            return View();
        }
    }
}