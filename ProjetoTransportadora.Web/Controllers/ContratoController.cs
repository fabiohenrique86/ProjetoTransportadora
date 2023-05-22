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
    public class ContratoController : BaseController
    {
        private ContratoBusiness contratoBusiness;
        private SituacaoContratoBusiness situacaoContratoBusiness;
        private SituacaoMultaBusiness situacaoMultaBusiness;
        private ProdutoBusiness produtoBusiness;
        private CanalBusiness canalBusiness;

        public ContratoController()
        {
            contratoBusiness = new ContratoBusiness();
            situacaoContratoBusiness = new SituacaoContratoBusiness();
            situacaoMultaBusiness = new SituacaoMultaBusiness();
            produtoBusiness = new ProdutoBusiness();
            canalBusiness = new CanalBusiness();
        }

        public ActionResult Index()
        {
            ViewBag.SituacaoContrato = situacaoContratoBusiness.Listar(new SituacaoContratoDto() { Ativo = true });
            ViewBag.Produto = produtoBusiness.Listar(new ProdutoDto() { Ativo = true });
            ViewBag.Canal = canalBusiness.Listar(new CanalDto() { Ativo = true });

            return View();
        }

        [HttpPost]
        public JsonResult Listar(ContratoDto contratoDto)
        {
            var listaContrato = contratoBusiness.Listar(contratoDto);
            var listaSituacaoContratoResumo = contratoBusiness.ListarSituacaoContratoResumo(listaContrato);
            var listaSituacaoParcelaResumo = contratoBusiness.ListarSituacaoParcelaResumo(listaContrato);

            return Json(new { Sucesso = true, Mensagem = "Contrato listado com sucesso", Data = listaContrato, SituacaoContratoResumo = listaSituacaoContratoResumo, SituacaoParcelaResumo = listaSituacaoParcelaResumo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(ContratoDto contratoDto)
        {
            contratoBusiness.Incluir(contratoDto);

            var listaContrato = contratoBusiness.Listar();
            var listaSituacaoContratoResumo = contratoBusiness.ListarSituacaoContratoResumo(listaContrato);
            var listaSituacaoParcelaResumo = contratoBusiness.ListarSituacaoParcelaResumo(listaContrato);

            return Json(new { Sucesso = true, Mensagem = "Contrato listado com sucesso", Data = listaContrato, SituacaoContratoResumo = listaSituacaoContratoResumo, SituacaoParcelaResumo = listaSituacaoParcelaResumo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Antecipar(ContratoDto contratoDto)
        {
            contratoBusiness.Antecipar(contratoDto);

            var lista = contratoBusiness.Listar(new ContratoDto() { Id = contratoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Contrato antecipado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Baixar(ContratoDto contratoDto)
        {
            contratoBusiness.Baixar(contratoDto);

            var lista = contratoBusiness.Listar(new ContratoDto() { Id = contratoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Contrato baixado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult Exportar(ContratoDto contratoDto)
        {
            var lista = contratoBusiness.Listar(contratoDto);

            string csv = "Montadora; Modelo; Ano Fabricação; Ano Modelo; Cor; Placa; Proprietário Atual; Proprietário Anterior; Renavam; Chassi; Data Aquisição; Valor Aquisição; Data Venda; Valor Venda; Data Recuperação; Data Valor FIPE; Valor FIPE; Valor Transportadora; Implemento; Comprimento; Altura; Largura; Rastreador; Situação" + Environment.NewLine;

            //foreach (var item in lista)
            //{
            //    csv += (item.MontadoraDto.Nome == null ? "" : item.MontadoraDto.Nome) + ";";
            //    csv += item.Modelo + ";";
            //    csv += item.AnoFabricacao + ";";
            //    csv += item.AnoModelo + ";";
            //    csv += item.Cor + ";";
            //    csv += item.Placa + ";";
            //    csv += (item.PessoaProprietarioAtualDto == null ? "" : item.PessoaProprietarioAtualDto.Nome) + ";";
            //    csv += (item.PessoaProprietarioAnteriorDto == null ? "" : item.PessoaProprietarioAnteriorDto.Nome) + ";";
            //    csv += item.Renavam + ";";
            //    csv += item.Chassi + ";";
            //    csv += (item.DataAquisicao == null ? "" : item.DataAquisicao.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";
            //    csv += (item.ValorAquisicao == null ? "" : item.ValorAquisicao.GetValueOrDefault().ToString("C")) + ";";
            //    csv += (item.DataVenda == null ? "" : item.DataVenda.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";
            //    csv += (item.ValorVenda == null ? "" : item.ValorVenda.GetValueOrDefault().ToString("C")) + ";";
            //    csv += (item.DataRecuperacao == null ? "" : item.DataRecuperacao.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";
            //    csv += (item.DataValorFIPE == null ? "" : item.DataValorFIPE.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";
            //    csv += (item.ValorFIPE == null ? "" : item.ValorFIPE.GetValueOrDefault().ToString("C")) + ";";
            //    csv += (item.ValorTransportadora == null ? "" : item.ValorTransportadora.GetValueOrDefault().ToString("C")) + ";";
            //    csv += item.Implemento + ";";
            //    csv += item.Comprimento + ";";
            //    csv += item.Altura + ";";
            //    csv += item.Largura + ";";
            //    csv += item.Rastreador + ";";
            //    csv += (item.SituacaoContratoDto == null ? "" : item.SituacaoContratoDto.Nome) + ";";

            //    csv += Environment.NewLine;
            //}

            return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray(), "text/csv", "contrato-exportar-" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv");
        }

        public JsonResult Importar()
        {
            HttpPostedFileBase file = Request.Files[0];

            if (file == null)
                return Json("Arquivo é obrigatório", JsonRequestBehavior.AllowGet);

            var tamanhoArquivo = file.ContentLength;
            var tipoArquivo = file.ContentType;
            var streamArquivo = file.InputStream;
            var conteudoArquivo = string.Empty;
            var mensagem = string.Empty;
            var placa = string.Empty;

            if (tamanhoArquivo <= 0)
                return Json("Arquivo está vazio", JsonRequestBehavior.AllowGet);

            if (!tipoArquivo.Contains("csv"))
                return Json("Arquivo deve ser do tipo csv", JsonRequestBehavior.AllowGet);

            using (var sr = new StreamReader(streamArquivo, Encoding.GetEncoding(new System.Globalization.CultureInfo("pt-BR").TextInfo.ANSICodePage)))
                conteudoArquivo = sr.ReadToEnd();

            var idUsuario = UsuarioLogado().Id;
            var dataCadastro = DateTime.UtcNow;

            foreach (var item in conteudoArquivo.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                try
                {
                    var linhaArquivo = item.Split(";".ToCharArray(), StringSplitOptions.None);

                    var montadora = linhaArquivo[0]?.Trim();
                    var modelo = linhaArquivo[1]?.Trim();
                    var anoFabricacao = linhaArquivo[2]?.Trim();
                    var anoModelo = linhaArquivo[3]?.Trim();
                    var cor = linhaArquivo[4]?.Trim();
                    placa = linhaArquivo[5]?.Trim();
                    var proprietarioAtual = linhaArquivo[6]?.Trim();
                    var proprietarioAnterior = linhaArquivo[7]?.Trim();
                    var renavam = linhaArquivo[8]?.Trim();
                    var chassi = linhaArquivo[9]?.Trim();
                    var dataAquisicao = linhaArquivo[10]?.Trim();
                    var valorAquisicao = linhaArquivo[11]?.Trim();
                    var dataVenda = linhaArquivo[12]?.Trim();
                    var valorVenda = linhaArquivo[13]?.Trim();
                    var dataRecuperacao = linhaArquivo[14]?.Trim();
                    var dataValorFIPE = linhaArquivo[15]?.Trim();
                    var valorFIPE = linhaArquivo[16]?.Trim();
                    var valorTranpostadora = linhaArquivo[17]?.Trim();
                    var implemento = linhaArquivo[18]?.Trim();
                    var comprimento = linhaArquivo[19]?.Trim();
                    var altura = linhaArquivo[20]?.Trim();
                    var largura = linhaArquivo[21]?.Trim();
                    var rastreador = linhaArquivo[22]?.Trim();
                    var situacaoContrato = linhaArquivo[23]?.Trim();

                    DateTime dtAquisicao;
                    DateTime.TryParse(dataAquisicao, out dtAquisicao);

                    DateTime dtVenda;
                    DateTime.TryParse(dataVenda, out dtVenda);

                    DateTime dtRecuperacao;
                    DateTime.TryParse(dataRecuperacao, out dtRecuperacao);

                    DateTime dtValorFIPE;
                    DateTime.TryParse(dataValorFIPE, out dtValorFIPE);

                    int aFabricacao;
                    int.TryParse(anoFabricacao, out aFabricacao);

                    int aModelo;
                    int.TryParse(anoModelo, out aModelo);

                    double vlAquisicao;
                    double.TryParse(valorAquisicao, out vlAquisicao);

                    double vlVenda;
                    double.TryParse(valorVenda, out vlVenda);

                    double vlFIPE;
                    double.TryParse(valorFIPE, out vlFIPE);

                    double vlTransportadora;
                    double.TryParse(valorTranpostadora, out vlTransportadora);

                    double dComprimento;
                    double.TryParse(comprimento, out dComprimento);

                    double dAltura;
                    double.TryParse(altura, out dAltura);

                    double dLargura;
                    double.TryParse(largura, out dLargura);

                    var idMontadora = 0;                    
                    //var montadoraDto = montadoraBusiness.Obter(new MontadoraDto() { Nome = montadora });
                    
                    //if (montadoraDto != null)
                    //    idMontadora = montadoraDto.Id;

                    int? idProprietarioAtual = null;
                    //if (!string.IsNullOrEmpty(proprietarioAtual))
                    //{
                    //    var pessoaProprietarioAtualDto = pessoaBusiness.Listar(new PessoaDto() { Nome = proprietarioAtual }).FirstOrDefault();

                    //    if (pessoaProprietarioAtualDto != null)
                    //        idProprietarioAtual = pessoaProprietarioAtualDto.Id;
                    //}

                    int? idProprietarioAnterior = null;
                    //if (!string.IsNullOrEmpty(proprietarioAnterior))
                    //{
                    //    var pessoaProprietarioAnteriorDto = pessoaBusiness.Listar(new PessoaDto() { Nome = proprietarioAnterior }).FirstOrDefault();

                    //    if (pessoaProprietarioAnteriorDto != null)
                    //        idProprietarioAnterior = pessoaProprietarioAnteriorDto.Id;
                    //}

                    int? idSituacaoContrato = null;
                    if (!string.IsNullOrEmpty(situacaoContrato))
                    {
                        var situacaoContratoDto = situacaoContratoBusiness.Obter(new SituacaoContratoDto() { Nome = situacaoContrato });

                        if (situacaoContratoDto != null)
                            idSituacaoContrato = situacaoContratoDto.Id;
                    }

                    //var ContratoDto = new ContratoDto()
                    //{
                    //    IdMontadora = idMontadora,
                    //    Modelo = modelo,
                    //    AnoFabricacao = aFabricacao,
                    //    AnoModelo = aModelo,
                    //    Cor = cor,
                    //    Placa = placa,
                    //    IdProprietarioAtual = idProprietarioAtual,
                    //    IdProprietarioAnterior = idProprietarioAnterior,
                    //    Renavam = renavam,
                    //    Chassi = chassi,
                    //    DataAquisicao = dtAquisicao == DateTime.MinValue ? (DateTime?)null : dtAquisicao,
                    //    ValorAquisicao = vlAquisicao,
                    //    DataVenda = dtVenda == DateTime.MinValue ? (DateTime?)null : dtVenda,
                    //    DataRecuperacao = dtRecuperacao == DateTime.MinValue ? (DateTime?)null : dtRecuperacao,
                    //    DataValorFIPE = dtValorFIPE == DateTime.MinValue ? (DateTime?)null : dtValorFIPE,
                    //    ValorFIPE = vlFIPE,
                    //    ValorTransportadora = vlTransportadora,
                    //    Implemento = implemento,
                    //    Comprimento = dComprimento,
                    //    Altura = dAltura,
                    //    Largura = dLargura,
                    //    Rastreador = rastreador,
                    //    IdSituacaoContrato = idSituacaoContrato,
                    //    IdUsuarioCadastro = idUsuario,
                    //    DataCadastro = dataCadastro
                    //};

                    //contratoBusiness.Incluir(ContratoDto);

                    mensagem += $"Contrato ({placa}) incluído com sucesso" + Environment.NewLine;
                }
                catch (BusinessException ex)
                {
                    mensagem += $"Erro ao incluir Contrato ({placa}): " + ex.Message + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    mensagem += $"Erro ao incluir Contrato ({placa}): " + ex.Message + Environment.NewLine;
                }
            }

            return Json(new { Sucesso = true, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }
    }
}