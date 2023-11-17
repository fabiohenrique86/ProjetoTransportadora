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
        private ContratoParcelaBusiness contratoParcelaBusiness;
        private ContratoBusiness contratoBusiness;
        public ParcelaController()
        {
            situacaoContratoBusiness = new SituacaoContratoBusiness();
            situacaoParcelaBusiness = new SituacaoParcelaBusiness();
            produtoBusiness = new ProdutoBusiness();
            contratoParcelaBusiness = new ContratoParcelaBusiness();
            contratoBusiness = new ContratoBusiness();
        }
        public ActionResult Index()
        {
            ViewBag.SituacaoContrato = situacaoContratoBusiness.Listar(new SituacaoContratoDto() { Ativo = true });
            ViewBag.SituacaoParcela = situacaoParcelaBusiness.Listar(new SituacaoParcelaDto() { Ativo = true });
            ViewBag.Produto = produtoBusiness.Listar(new ProdutoDto() { Ativo = true });

            return View();
        }

        [HttpGet]
        public JsonResult Listar(ContratoParcelaDto contratoParcelaDto)
        {
            var listaContratoParcelaDto = contratoParcelaBusiness.Listar(contratoParcelaDto);

            return Json(new { Sucesso = true, Mensagem = "Contrato parcela listado com sucesso", Data = listaContratoParcelaDto }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Calcular(int idContrato, int idContratoParcela, DateTime? dataPagamento, double taxaMulta, double taxaMora, double valorResiduo, double valorAcrescimo, double valorDescontoParcela)
        {
            var calculoPagamento = contratoParcelaBusiness.Calcular(idContrato, idContratoParcela, dataPagamento.GetValueOrDefault(), taxaMulta, taxaMora, valorResiduo, valorAcrescimo, valorDescontoParcela);

            return Json(new { Sucesso = true, Mensagem = "Contrato parcela calculado com sucesso", Data = calculoPagamento }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(ContratoParcelaDto contratoParcelaDto)
        {
            contratoParcelaBusiness.Alterar(contratoParcelaDto);

            var lista = contratoBusiness.ListarGridParcela(new ContratoDto() { ContratoParcelaDto = new List<ContratoParcelaDto>() { new ContratoParcelaDto() { Id = contratoParcelaDto.Id } } });

            return Json(new { Sucesso = true, Mensagem = "Contrato parcela alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileContentResult Exportar(string idsContratoParcela)
        {
            var contratoParcela = new ContratoParcelaDto();

            contratoParcela.ListaIdContratoParcela = idsContratoParcela?.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            var listaContratoParcela = contratoParcelaBusiness.ListarSimples(contratoParcela);

            string csv = "Número Contrato; Número Parcela; Data Emissão; Data Início; Data Vencimento; Dias Parcela; Dias Contrato; Data Pagamento; Valor Original; Valor Amortização; Valor Juros; Valor Multa; Valor Mora; Valor Desconto Juros; Valor Desconto Parcela; Valor Parcela; Taxa Mora; Taxa Multa; Valor Acréscimo; Valor Resíduo; Situação;" + Environment.NewLine;

            foreach (var cp in listaContratoParcela)
            {
                csv += cp.IdContrato + ";";
                csv += cp.NumeroParcela.ToString() + ";";
                csv += cp.DataEmissao.ToString("dd/MM/yyyy") + ";";
                csv += cp.DataInicio.ToString("dd/MM/yyyy") + ";";
                csv += cp.DataVencimento.ToString("dd/MM/yyyy") + ";";
                csv += cp.DiasParcela.ToString() + ";";
                csv += cp.DiasContrato.ToString() + ";";
                csv += (cp.DataPagamento.GetValueOrDefault() == DateTime.MinValue ? string.Empty : cp.DataPagamento.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";                
                csv += cp.ValorOriginal.ToString("C") + ";";
                csv += cp.ValorAmortizacao.GetValueOrDefault().ToString("C") + ";";
                csv += cp.ValorJuros.GetValueOrDefault().ToString("C") + ";";
                csv += cp.ValorMulta.GetValueOrDefault().ToString("C") + ";";
                csv += cp.ValorMora.GetValueOrDefault().ToString("C") + ";";
                csv += cp.ValorDescontoJuros.GetValueOrDefault().ToString("C") + ";";
                csv += cp.ValorDescontoParcela.GetValueOrDefault().ToString("C") + ";";
                csv += cp.ValorParcela.ToString("C") + ";";
                csv += cp.TaxaMora.GetValueOrDefault().ToString() + ";";
                csv += cp.TaxaMulta.GetValueOrDefault().ToString() + ";";
                csv += cp.ValorAcrescimo.GetValueOrDefault().ToString("C") + ";";
                csv += cp.ValorResiduo.GetValueOrDefault().ToString("C") + ";";
                csv += (cp.SituacaoParcelaDto == null ? string.Empty : cp.SituacaoParcelaDto.Nome) + ";";

                csv += Environment.NewLine;
            }

            return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray(), "text/csv", "contrato-parcela-exportar-" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv");
        }

        public JsonResult Importar()
        {
            HttpPostedFileBase file = Request.Files[0];

            if (file == null)
                return Json(new { Sucesso = false, Mensagem = "Arquivo é obrigatório" }, JsonRequestBehavior.AllowGet);

            var tamanhoArquivo = file.ContentLength;
            var tipoArquivo = file.ContentType;
            var streamArquivo = file.InputStream;
            var conteudoArquivo = string.Empty;
            var mensagem = string.Empty;

            if (tamanhoArquivo <= 0)
                return Json(new { Sucesso = false, Mensagem = "Arquivo está vazio" }, JsonRequestBehavior.AllowGet);

            if (!tipoArquivo.Contains("csv"))
                return Json(new { Sucesso = false, Mensagem = "Arquivo deve ser do tipo csv" }, JsonRequestBehavior.AllowGet);

            using (var sr = new StreamReader(streamArquivo, Encoding.GetEncoding(new System.Globalization.CultureInfo("pt-BR").TextInfo.ANSICodePage)))
                conteudoArquivo = sr.ReadToEnd();

            var listaContratoParcelaDto = new List<ContratoParcelaDto>();
            int idContrato = 0;

            foreach (var item in conteudoArquivo.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                try
                {
                    var linhaArquivo = item.Split(";".ToCharArray(), StringSplitOptions.None);

                    var contrato = linhaArquivo[0]?.Trim();
                    var dataVencimento = linhaArquivo[1]?.Trim();
                    var dataPagamento = linhaArquivo[2]?.Trim();
                    var dataEmissao = linhaArquivo[3]?.Trim();
                    var valorOriginal = linhaArquivo[4]?.Trim();
                    var valorMulta = linhaArquivo[5]?.Trim();
                    var valorMora = linhaArquivo[6]?.Trim();
                    var valorDesconto = linhaArquivo[7]?.Trim();
                    var valorResiduo = linhaArquivo[8]?.Trim();
                    var valorParcela = linhaArquivo[9]?.Trim();
                    var valorPago = linhaArquivo[10]?.Trim();
                    var taxaMulta = linhaArquivo[11]?.Trim();
                    var taxaMora = linhaArquivo[12]?.Trim();
                    var situacaoParcela = linhaArquivo[13]?.Trim();

                    DateTime dtVencimento;
                    DateTime.TryParse(dataVencimento, out dtVencimento);

                    DateTime dtPagamento;
                    DateTime.TryParse(dataPagamento, out dtPagamento);

                    DateTime dtEmissao;
                    DateTime.TryParse(dataEmissao, out dtEmissao);

                    int.TryParse(contrato, out idContrato);

                    double vlOriginal;
                    double.TryParse(valorOriginal, out vlOriginal);

                    double vlMulta;
                    double.TryParse(valorMulta, out vlMulta);

                    double vlMora;
                    double.TryParse(valorMora, out vlMora);

                    double vlDesconto;
                    double.TryParse(valorDesconto, out vlDesconto);

                    double vlResiduo;
                    double.TryParse(valorResiduo, out vlResiduo);

                    double vlParcela;
                    double.TryParse(valorParcela, out vlParcela);

                    double vlPago;
                    double.TryParse(valorPago, out vlPago);

                    double txMulta;
                    double.TryParse(taxaMulta, out txMulta);

                    double txMora;
                    double.TryParse(taxaMora, out txMora);

                    int? idSituacaoParcela = null;
                    if (!string.IsNullOrEmpty(situacaoParcela))
                    {
                        var situacaoParcelaDto = situacaoParcelaBusiness.Obter(new SituacaoParcelaDto() { Nome = situacaoParcela });

                        if (situacaoParcelaDto != null)
                            idSituacaoParcela = situacaoParcelaDto.Id;
                    }

                    listaContratoParcelaDto.Add(new ContratoParcelaDto()
                    {
                        IdContrato = idContrato,
                        NumeroParcela = 0,
                        IdSituacaoParcela = idSituacaoParcela.GetValueOrDefault(),
                        DataVencimento = dtVencimento,
                        DiasParcela = 0,
                        DiasContrato = 0,
                        DataPagamento = dtPagamento,
                        DataEmissao = dtEmissao,
                        ValorOriginal = vlOriginal,
                        ValorAmortizacao = vlPago,
                        ValorJuros = 0,
                        ValorMulta = vlMulta,
                        ValorMora = vlMora,
                        ValorDescontoJuros = vlDesconto,
                        ValorParcela = vlParcela,
                        DataInicio = new DateTime()
                    });
                }
                catch (BusinessException ex)
                {
                    mensagem += $"Erro ao incluir parcelas do contrato: " + ex.Message + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    mensagem += $"Erro ao incluir parcelas do contrato: " + ex.Message + Environment.NewLine;
                }
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                int numeroParcela = 1;
                foreach (var item in listaContratoParcelaDto.OrderBy(x => x.DataVencimento))
                {
                    item.NumeroParcela = numeroParcela;
                    contratoParcelaBusiness.Incluir(item);
                    numeroParcela++;
                }

                scope.Complete();
            }

            mensagem += $"Parcelas do Contrato ({idContrato}) incluídas com sucesso" + Environment.NewLine;

            return Json(new { Sucesso = true, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }
    }
}