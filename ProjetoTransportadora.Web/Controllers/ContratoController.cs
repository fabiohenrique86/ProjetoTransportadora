﻿using ProjetoTransportadora.Business;
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
    public class ContratoController : BaseController
    {
        private ContratoBusiness contratoBusiness;
        private SituacaoContratoBusiness situacaoContratoBusiness;
        private SituacaoMultaBusiness situacaoMultaBusiness;
        private ProdutoBusiness produtoBusiness;
        private CanalBusiness canalBusiness;
        private PessoaBusiness pessoaBusiness;
        private VeiculoBusiness veiculoBusiness;
        private ContratoParcelaBusiness contratoParcelaBusiness;

        public ContratoController()
        {
            contratoBusiness = new ContratoBusiness();
            situacaoContratoBusiness = new SituacaoContratoBusiness();
            situacaoMultaBusiness = new SituacaoMultaBusiness();
            produtoBusiness = new ProdutoBusiness();
            canalBusiness = new CanalBusiness();
            pessoaBusiness = new PessoaBusiness();
            veiculoBusiness = new VeiculoBusiness();
            contratoParcelaBusiness = new ContratoParcelaBusiness();
        }

        public ActionResult Index()
        {
            ViewBag.SituacaoContrato = situacaoContratoBusiness.Listar(new SituacaoContratoDto() { Ativo = true });
            ViewBag.Produto = produtoBusiness.Listar(new ProdutoDto() { Ativo = true });
            ViewBag.Canal = canalBusiness.Listar(new CanalDto() { Ativo = true });

            // Se existe uma simulação e veio do botão 'GerarContrato', inclui a simulação na view
            if (TempData["SimulacaoDto"] != null && TempData["GerarContrato"] != null)
            {
                ViewBag.SimulacaoDto = TempData["SimulacaoDto"];
                TempData.Remove("SimulacaoDto");
                TempData.Remove("GerarContrato");
            }

            return View();
        }

        [HttpPost]
        public JsonResult ListarGridParcela(ContratoDto contratoDto)
        {
            var lista = contratoBusiness.ListarGridParcela(contratoDto);

            return Json(new { Sucesso = true, Mensagem = "Contrato listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarGrid(ContratoDto contratoDto)
        {
            var listaContrato = contratoBusiness.ListarGrid(contratoDto);
            var listaSituacaoContratoResumo = contratoBusiness.ListarSituacaoContratoResumo(contratoDto);
            var listaSituacaoParcelaResumo = contratoBusiness.ListarSituacaoParcelaResumo(contratoDto);

            return Json(new { Sucesso = true, Mensagem = "Contrato listado com sucesso", Data = listaContrato, SituacaoContratoResumo = listaSituacaoContratoResumo, SituacaoParcelaResumo = listaSituacaoParcelaResumo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Listar(ContratoDto contratoDto)
        {
            var listaContrato = contratoBusiness.Listar(contratoDto);

            return Json(new { Sucesso = true, Mensagem = "Contrato listado com sucesso", Data = listaContrato }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(ContratoDto contratoDto)
        {
            contratoBusiness.Incluir(contratoDto);

            var listaContrato = contratoBusiness.ListarGrid();
            var listaSituacaoContratoResumo = contratoBusiness.ListarSituacaoContratoResumo();
            var listaSituacaoParcelaResumo = contratoBusiness.ListarSituacaoParcelaResumo();

            return Json(new { Sucesso = true, Mensagem = "Contrato incluído com sucesso", Data = listaContrato, SituacaoContratoResumo = listaSituacaoContratoResumo, SituacaoParcelaResumo = listaSituacaoParcelaResumo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(ContratoDto contratoDto)
        {
            contratoBusiness.Alterar(contratoDto);

            var listaContrato = contratoBusiness.ListarGrid(new ContratoDto() { Id = contratoDto.Id });
            var listaSituacaoContratoResumo = contratoBusiness.ListarSituacaoContratoResumo();
            var listaSituacaoParcelaResumo = contratoBusiness.ListarSituacaoParcelaResumo();

            return Json(new { Sucesso = true, Mensagem = "Contrato alterado com sucesso", Data = listaContrato, SituacaoContratoResumo = listaSituacaoContratoResumo, SituacaoParcelaResumo = listaSituacaoParcelaResumo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Antecipar(ContratoDto contratoDto)
        {
            var listaContratoParcela = contratoBusiness.Antecipar(contratoDto);

            if (!contratoDto.SimulacaoAntecipacao)
            {
                var listaContrato = contratoBusiness.ListarGrid(new ContratoDto() { Id = contratoDto.Id });
                var listaSituacaoContratoResumo = contratoBusiness.ListarSituacaoContratoResumo();
                var listaSituacaoParcelaResumo = contratoBusiness.ListarSituacaoParcelaResumo();

                return Json(new { Sucesso = true, Mensagem = "Contrato listado com sucesso", Data = listaContrato, SituacaoContratoResumo = listaSituacaoContratoResumo, SituacaoParcelaResumo = listaSituacaoParcelaResumo }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Sucesso = true, Mensagem = "Contrato antecipado com sucesso", ValorSaldoAntecipacao = listaContratoParcela?.Sum(x => x.ValorParcela) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Baixar(ContratoDto contratoDto)
        {
            contratoBusiness.Baixar(contratoDto);

            var listaContrato = contratoBusiness.ListarGrid(new ContratoDto() { Id = contratoDto.Id });
            var listaSituacaoContratoResumo = contratoBusiness.ListarSituacaoContratoResumo();
            var listaSituacaoParcelaResumo = contratoBusiness.ListarSituacaoParcelaResumo();

            return Json(new { Sucesso = true, Mensagem = "Contrato baixado com sucesso", Data = listaContrato, SituacaoContratoResumo = listaSituacaoContratoResumo, SituacaoParcelaResumo = listaSituacaoParcelaResumo }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult Exportar(ContratoDto contratoDto)
        {
            var listaContrato = contratoBusiness.Listar(contratoDto);

            string csv = "Número Contrato; Data Contrato; Nome Cliente; CPF cliente; Produto; Placa (Veículo); Modelo (Veículo); Cor (Veículo); Renavam (Veículo); Fiador; Indicação; Promotor; Canal; Valor Parcela; Taxa Multa; Taxa Mora; Tipo Cálculo; Data Primeira Parcela; Valor Veículo; Valor Entrada; Valor Documentação; Valor Desconto; Valor Dinheiro; Placa (Veículo Entrada); Modelo (Veículo Entrada); Cor (Veículo Entrada); Renavam (Veículo Entrada); Valor Veículo Entrada; Valor Depositado; Valor Tarifa; Valor Financiado Total; Taxa Juros; Quantidade Parcelas; Valor Financiado Documentação; Valor Financiado Veículo; Data Antecipação; Valor Antecipação; Data Baixa; Situação;" + Environment.NewLine;

            foreach (var contrato in listaContrato)
            {
                csv += contrato.Id + ";";
                csv += contrato.DataContrato.ToString("dd/MM/yyyy") + ";";
                csv += (contrato.PessoaClienteDto == null ? string.Empty : contrato.PessoaClienteDto.Nome) + ";";
                csv += (contrato.PessoaClienteDto == null ? string.Empty : string.IsNullOrEmpty(contrato.PessoaClienteDto.Cpf) ? contrato.PessoaClienteDto.Cnpj : contrato.PessoaClienteDto.Cpf) + ";";
                csv += (contrato.ProdutoDto == null ? string.Empty : contrato.ProdutoDto.Nome) + ";";
                csv += (contrato.VeiculoDto == null ? "" : contrato.VeiculoDto.Placa) + ";";
                csv += (contrato.VeiculoDto == null ? "" : contrato.VeiculoDto.Modelo) + ";";
                csv += (contrato.VeiculoDto == null ? "" : contrato.VeiculoDto.Cor) + ";";
                csv += (contrato.VeiculoDto == null ? "" : contrato.VeiculoDto.Renavam) + ";";
                csv += (contrato.PessoaFiadorDto == null ? "" : contrato.PessoaFiadorDto.Nome) + ";";
                csv += (contrato.PessoaIndicacaoDto == null ? string.Empty : contrato.PessoaIndicacaoDto.Nome) + ";";
                csv += (contrato.PessoaPromotorDto == null ? string.Empty : contrato.PessoaPromotorDto.Nome) + ";";
                csv += (contrato.CanalDto == null ? string.Empty : contrato.CanalDto.Nome) + ";";
                csv += contrato.ContratoParcelaDto?.FirstOrDefault()?.ValorParcela.ToString("C") + ";";
                csv += contrato.TaxaMulta.GetValueOrDefault().ToString() + ";";
                csv += contrato.TaxaMora.GetValueOrDefault().ToString() + ";";
                csv += (contrato.IdTipoContrato == TipoContratoDto.EnumTipoContrato.Diario.GetHashCode() ? "Diário" : "Mensal") + ";";
                csv += contrato.DataPrimeiraParcela.ToString("dd/MM/yyyy") + ";";
                csv += contrato.ValorVeiculo.GetValueOrDefault().ToString("C") + ";";
                csv += contrato.ValorEntrada.GetValueOrDefault().ToString("C") + ";";
                csv += contrato.ValorDocumentacao.GetValueOrDefault().ToString("C") + ";";
                csv += contrato.ValorDesconto.GetValueOrDefault().ToString("C") + ";";
                csv += contrato.ValorDinheiro.GetValueOrDefault().ToString("C") + ";";
                csv += (contrato.VeiculoEntradaDto == null ? "" : contrato.VeiculoEntradaDto.Placa) + ";";
                csv += (contrato.VeiculoEntradaDto == null ? "" : contrato.VeiculoEntradaDto.Modelo) + ";";
                csv += (contrato.VeiculoEntradaDto == null ? "" : contrato.VeiculoEntradaDto.Cor) + ";";
                csv += (contrato.VeiculoEntradaDto == null ? "" : contrato.VeiculoEntradaDto.Renavam) + ";";
                csv += contrato.ValorVeiculoEntrada.GetValueOrDefault().ToString("C") + ";";
                csv += contrato.ValorDepositado.GetValueOrDefault().ToString("C") + ";";
                csv += contrato.ValorTarifa.GetValueOrDefault().ToString("C") + ";";
                csv += contrato.ValorFinanciado.ToString("C") + ";";
                csv += contrato.TaxaJuros.ToString() + ";";
                csv += contrato.ContratoParcelaDto?.Count() + ";";
                csv += contrato.ValorFinanciadoDocumentacao.GetValueOrDefault().ToString("C") + ";";
                csv += contrato.ValorFinanciadoVeiculo.GetValueOrDefault().ToString("C") + ";";
                csv += (contrato.DataAntecipacao == null ? string.Empty : contrato.DataAntecipacao.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";
                csv += contrato.ValorAntecipacao.GetValueOrDefault().ToString("C") + ";";
                csv += (contrato.DataBaixa == null ? string.Empty : contrato.DataBaixa.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";
                csv += (contrato.SituacaoContratoDto == null ? "" : contrato.SituacaoContratoDto.Nome) + ";";

                csv += Environment.NewLine;
            }

            return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray(), "text/csv", "contrato-exportar-" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv");
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

            var idUsuario = UsuarioLogado().Id;
            var dataCadastro = DateTime.UtcNow;

            foreach (var item in conteudoArquivo.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                try
                {
                    var linhaArquivo = item.Split(";".ToCharArray(), StringSplitOptions.None);

                    var dataContrato = linhaArquivo[0]?.Trim();
                    var canal = linhaArquivo[1]?.Trim();
                    var cliente = linhaArquivo[2]?.Trim();
                    var produto = linhaArquivo[3]?.Trim();
                    var promotor = linhaArquivo[4]?.Trim();
                    var indicacao = linhaArquivo[5]?.Trim();
                    var fiador = linhaArquivo[6]?.Trim();
                    var veiculo = linhaArquivo[7]?.Trim();
                    var valorVenda = linhaArquivo[8]?.Trim();
                    var valorEntrada = linhaArquivo[9]?.Trim();
                    var veiculoEntrada = linhaArquivo[10]?.Trim();
                    var valorVeiculoEntrada = linhaArquivo[11]?.Trim();
                    var valorDinheiroEntrada = linhaArquivo[12]?.Trim();
                    var dataInicioJuros = linhaArquivo[13]?.Trim();
                    var dataPrimeiraParcela = linhaArquivo[14]?.Trim();
                    var quantidadeParcelas = linhaArquivo[15]?.Trim();
                    var taxaJurosMensal = linhaArquivo[16]?.Trim();
                    var valorFinanciado = linhaArquivo[17]?.Trim();
                    var valorFinanciadoVeiculo = linhaArquivo[18]?.Trim();
                    var valorFinanciadoDocumentacao = linhaArquivo[19]?.Trim();
                    var valorCaixa = linhaArquivo[20]?.Trim();
                    var valorDepositado = linhaArquivo[21]?.Trim();
                    var valorDesconto = linhaArquivo[22]?.Trim();
                    var dataBaixa = linhaArquivo[23]?.Trim();
                    var dataAntecipacao = linhaArquivo[24]?.Trim();
                    var valorAntecipacao = linhaArquivo[25]?.Trim();

                    DateTime dtContrato;
                    DateTime.TryParse(dataContrato, out dtContrato);

                    DateTime dtInicioJuros;
                    DateTime.TryParse(dataInicioJuros, out dtInicioJuros);

                    DateTime dtPrimeiraParcela;
                    DateTime.TryParse(dataPrimeiraParcela, out dtPrimeiraParcela);

                    DateTime dtBaixa;
                    DateTime.TryParse(dataBaixa, out dtBaixa);

                    DateTime dtAntecipacao;
                    DateTime.TryParse(dataAntecipacao, out dtAntecipacao);

                    int qtdParcelas;
                    int.TryParse(quantidadeParcelas, out qtdParcelas);

                    double vlVenda;
                    double.TryParse(valorVenda, out vlVenda);

                    double vlVeiculoEntrada;
                    double.TryParse(valorVeiculoEntrada, out vlVeiculoEntrada);

                    double txJurosMensal;
                    double.TryParse(taxaJurosMensal, out txJurosMensal);

                    double vlFinanciado;
                    double.TryParse(valorFinanciado, out vlFinanciado);

                    double vlFinanciadoVeiculo;
                    double.TryParse(valorFinanciadoVeiculo, out vlFinanciadoVeiculo);

                    double vlFinanciadoDocumentacao;
                    double.TryParse(valorFinanciadoDocumentacao, out vlFinanciadoDocumentacao);

                    double vlCaixa;
                    double.TryParse(valorCaixa, out vlCaixa);

                    double vlDepositado;
                    double.TryParse(valorDepositado, out vlDepositado);

                    double vlDesconto;
                    double.TryParse(valorDesconto, out vlDesconto);

                    double vlAntecipacao;
                    double.TryParse(valorAntecipacao, out vlAntecipacao);

                    int? idCanal = null;
                    if (!string.IsNullOrEmpty(canal))
                    {
                        var canalDto = canalBusiness.Obter(new CanalDto() { Nome = canal });

                        if (canalDto != null)
                            idCanal = canalDto.Id;
                    }

                    int? idCliente = null;
                    if (!string.IsNullOrEmpty(cliente))
                    {
                        var pessoaClienteDto = pessoaBusiness.Listar(new PessoaDto() { Nome = cliente }).FirstOrDefault();

                        if (pessoaClienteDto != null)
                            idCliente = pessoaClienteDto.Id;
                    }

                    int? idProduto = null;
                    if (!string.IsNullOrEmpty(produto))
                    {
                        var produtoDto = produtoBusiness.Listar(new ProdutoDto() { Nome = produto }).FirstOrDefault();

                        if (produtoDto != null)
                            idProduto = produtoDto.Id;
                    }

                    int? idPromotor = null;
                    if (!string.IsNullOrEmpty(promotor))
                    {
                        var pessoaPromotorDto = pessoaBusiness.Listar(new PessoaDto() { Nome = promotor }).FirstOrDefault();

                        if (pessoaPromotorDto != null)
                            idPromotor = pessoaPromotorDto.Id;
                    }

                    int? idIndicacao = null;
                    if (!string.IsNullOrEmpty(cliente))
                    {
                        var pessoaIndicacaoDto = pessoaBusiness.Listar(new PessoaDto() { Nome = indicacao }).FirstOrDefault();

                        if (pessoaIndicacaoDto != null)
                            idIndicacao = pessoaIndicacaoDto.Id;
                    }

                    int? idFiador = null;
                    if (!string.IsNullOrEmpty(fiador))
                    {
                        var pessoaFiadorDto = pessoaBusiness.Listar(new PessoaDto() { Nome = fiador }).FirstOrDefault();

                        if (pessoaFiadorDto != null)
                            idFiador = pessoaFiadorDto.Id;
                    }

                    int? idVeiculo = null;
                    if (!string.IsNullOrEmpty(fiador))
                    {
                        var veiculoDto = veiculoBusiness.Listar(new VeiculoDto() { Placa = veiculo }).FirstOrDefault();

                        if (veiculoDto != null)
                            idVeiculo = veiculoDto.Id;
                    }

                    int? idVeiculoEntrada = null;
                    if (!string.IsNullOrEmpty(fiador))
                    {
                        var veiculoEntradaDto = veiculoBusiness.Listar(new VeiculoDto() { Placa = veiculoEntrada }).FirstOrDefault();

                        if (veiculoEntradaDto != null)
                            idVeiculoEntrada = veiculoEntradaDto.Id;
                    }

                    var listaContratoParcelaDto = contratoParcelaBusiness.Gerar(new SimulacaoDto()
                    {
                        DataInicio = dtContrato,
                        DataPrimeiraParcela = dtPrimeiraParcela,
                        QuantidadeParcela = qtdParcelas,
                        TaxaMensalJuros = txJurosMensal,
                        ValorFinanciado = vlFinanciado
                    });

                    var contratoDto = new ContratoDto()
                    {
                        DataContrato = dtContrato,
                        IdCanal = idCanal,
                        IdCliente = idCliente.GetValueOrDefault(),
                        IdProduto = idProduto.GetValueOrDefault(),
                        IdPromotor = idPromotor,
                        IdIndicacao = idIndicacao,
                        IdFiador = idFiador,
                        IdVeiculo = idVeiculo.GetValueOrDefault(),
                        IdVeiculoEntrada = idVeiculoEntrada,
                        ValorVeiculoEntrada = vlVeiculoEntrada,
                        //DataRecuperacao = dtRecuperacao == DateTime.MinValue ? (DateTime?)null : dtRecuperacao,
                        DataPrimeiraParcela = dtPrimeiraParcela,
                        IdSituacaoContrato = SituacaoContratoDto.EnumSituacaoContrato.Ativo.GetHashCode(),
                        IdUsuarioCadastro = idUsuario,
                        DataCadastro = dataCadastro,
                        ContratoParcelaDto = listaContratoParcelaDto
                    };

                    var idContrato = contratoBusiness.Incluir(contratoDto);

                    mensagem += $"Contrato ({idContrato}) incluído com sucesso" + Environment.NewLine;
                }
                catch (BusinessException ex)
                {
                    mensagem += $"Erro ao incluir Contrato: " + ex.Message + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    mensagem += $"Erro ao incluir Contrato: " + ex.Message + Environment.NewLine;
                }
            }

            return Json(new { Sucesso = true, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }
    }
}