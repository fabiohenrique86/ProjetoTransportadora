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
    public class VeiculoController : BaseController
    {
        private VeiculoBusiness veiculoBusiness;
        private SituacaoVeiculoBusiness situacaoVeiculoBusiness;
        private SituacaoVeiculoBusiness situacaoMultaBusiness;

        public VeiculoController()
        {
            veiculoBusiness = new VeiculoBusiness();
            situacaoVeiculoBusiness = new SituacaoVeiculoBusiness();
            situacaoMultaBusiness = new SituacaoVeiculoBusiness();
        }

        public ActionResult Index()
        {
            ViewBag.SituacaoVeiculo = situacaoVeiculoBusiness.Listar(new SituacaoVeiculoDto() { Ativo = true });
            ViewBag.SituacaoMulta = situacaoMultaBusiness.Listar();

            return View();
        }

        [HttpGet]
        public JsonResult Listar(VeiculoDto veiculoDto)
        {
            var lista = veiculoBusiness.Listar(veiculoDto);

            return Json(new { Sucesso = true, Mensagem = "Veículo listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(VeiculoDto VeiculoDto)
        {
            veiculoBusiness.Incluir(VeiculoDto);

            var lista = veiculoBusiness.Listar();

            return Json(new { Sucesso = true, Mensagem = "Veículo cadastrado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(VeiculoDto veiculoDto)
        {
            veiculoBusiness.Alterar(veiculoDto);

            var lista = veiculoBusiness.Listar(new VeiculoDto() { Id = veiculoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Veículo alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(VeiculoDto veiculoDto)
        {
            veiculoBusiness.AlterarStatus(veiculoDto);

            var lista = veiculoBusiness.Listar(new VeiculoDto() { Id = veiculoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult Exportar(VeiculoDto VeiculoDto)
        {
            var lista = veiculoBusiness.Listar(VeiculoDto);

            string csv = "Nome Empresa; Cnpj; Proprietário; Cep; Logradouro; Número; Complemento; Bairro; Cidade; Uf; Produto; Data Abertura" + Environment.NewLine;

            //foreach (var item in lista)
            //{
            //    csv += item.Nome + ";";
            //    csv += item.Cnpj + ";";
            //    csv += (item.VeiculoProprietarioDto == null ? "" : item.VeiculoProprietarioDto.Nome) + ";";
            //    csv += item.CepResidencia + ";";
            //    csv += item.LogradouroResidencia + ";";
            //    csv += item.NumeroResidencia + ";";
            //    csv += item.ComplementoResidencia + ";";
            //    csv += item.BairroResidencia + ";";
            //    csv += item.CidadeResidencia + ";";
            //    csv += item.UfResidencia + ";";
            //    csv += (item.ProdutoDto == null ? "" : item.ProdutoDto.Nome) + ";";
            //    csv += (item.DataAbertura == null ? "" : item.DataAbertura.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";

            //    csv += Environment.NewLine;
            //}

            return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray(), "text/csv", "veículo-exportar-" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv");
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
            var nome = string.Empty;

            if (tamanhoArquivo <= 0)
                return Json("Arquivo está vazio", JsonRequestBehavior.AllowGet);

            if (!tipoArquivo.Contains("csv"))
                return Json("Arquivo deve ser do tipo csv", JsonRequestBehavior.AllowGet);

            using (var sr = new StreamReader(streamArquivo, Encoding.GetEncoding(new System.Globalization.CultureInfo("pt-BR").TextInfo.ANSICodePage)))
                conteudoArquivo = sr.ReadToEnd();

            var idUsuario = UsuarioLogado().Id;
            var dataCadastro = DateTime.UtcNow;

            //foreach (var item in conteudoArquivo.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1))
            //{
            //    try
            //    {
            //        var linhaArquivo = item.Split(";".ToCharArray(), StringSplitOptions.None);

            //        nome = linhaArquivo[0];
            //        var cnpj = linhaArquivo[1];
            //        var proprietario = linhaArquivo[2];
            //        var cepResidencia = linhaArquivo[3];
            //        var logradouroResidencia = linhaArquivo[4];
            //        var numeroResidencia = linhaArquivo[5];
            //        var complementoResidencia = linhaArquivo[6];
            //        var bairroResidencia = linhaArquivo[7];
            //        var cidadeResidencia = linhaArquivo[8];
            //        var ufResidencia = linhaArquivo[9];
            //        var produto = linhaArquivo[10];
            //        var dataAbertura = linhaArquivo[11];

            //        DateTime dtAbertura;
            //        DateTime.TryParse(dataAbertura, out dtAbertura);

            //        int nroResidencia;
            //        int.TryParse(numeroResidencia, out nroResidencia);

            //        var idProduto = produtoBusiness.Listar(new ProdutoDto() { Nome = produto }).FirstOrDefault()?.Id;

            //        var VeiculoDto = new VeiculoDto()
            //        {
            //            Nome = nome,
            //            Cnpj = cnpj,
            //            CepResidencia = cepResidencia,
            //            LogradouroResidencia = logradouroResidencia,
            //            NumeroResidencia = nroResidencia,
            //            ComplementoResidencia = complementoResidencia,
            //            BairroResidencia = bairroResidencia,
            //            CidadeResidencia = cidadeResidencia,
            //            UfResidencia = ufResidencia,
            //            IdProduto = idProduto,
            //            DataAbertura = dtAbertura,
            //            IdUsuarioCadastro = idUsuario,
            //            DataCadastro = dataCadastro,
            //            IdTipoVeiculo = (int)TipoVeiculoDto.TipoVeiculo.VeiculoJurídica
            //        };

            //        if (!string.IsNullOrEmpty(proprietario))
            //        {
            //            var VeiculoProprietarioDto = veiculoBusiness.Listar(new VeiculoDto() { Nome = proprietario }).FirstOrDefault();

            //            if (VeiculoProprietarioDto != null)
            //                VeiculoDto.IdProprietario = VeiculoProprietarioDto.Id;
            //        }

            //        veiculoBusiness.Incluir(VeiculoDto);

            //        mensagem += $"Veículo ({nome}) incluída com sucesso" + Environment.NewLine;
            //    }
            //    catch (BusinessException ex)
            //    {
            //        mensagem += $"Erro ao incluir Veículo ({nome}): " + ex.Message + Environment.NewLine;
            //    }
            //    catch (Exception ex)
            //    {
            //        mensagem += $"Erro ao incluir Veículo ({nome}): " + ex.Message + Environment.NewLine;
            //    }
            //}

            return Json(new { Sucesso = true, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }
    }
}