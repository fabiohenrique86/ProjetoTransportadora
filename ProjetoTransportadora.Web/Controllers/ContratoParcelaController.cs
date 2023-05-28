using ProjetoTransportadora.Business;
using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class ContratoParcelaController : BaseController
    {
        private ContratoParcelaBusiness contratoParcelaBusiness;

        public ContratoParcelaController()
        {
            contratoParcelaBusiness = new ContratoParcelaBusiness();
        }

        [HttpGet]
        public JsonResult Listar(ContratoParcelaDto contratoParcelaDto)
        {
            var listaContratoParcelaDto = contratoParcelaBusiness.Listar(contratoParcelaDto);

            return Json(new { Sucesso = true, Mensagem = "Contrato parcela listado com sucesso", Data = listaContratoParcelaDto }, JsonRequestBehavior.AllowGet);
        }
    }
}