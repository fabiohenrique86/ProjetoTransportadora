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
    public class PessoaJuridicaController : BaseController
    {
        private PessoaBusiness pessoaBusiness;
        private EstadoBusiness estadoBusiness;
        private TipoTelefoneBusiness tipoTelefoneBusiness;
        private ProdutoBusiness produtoBusiness;

        public PessoaJuridicaController()
        {
            pessoaBusiness = new PessoaBusiness();
            estadoBusiness = new EstadoBusiness();
            tipoTelefoneBusiness = new TipoTelefoneBusiness();
            produtoBusiness = new ProdutoBusiness();
        }

        public ActionResult Index()
        {
            ViewBag.Estado = estadoBusiness.Listar(new EstadoDto());
            ViewBag.TipoTelefone = tipoTelefoneBusiness.Listar(new TipoTelefoneDto() { Ativo = true });
            ViewBag.Produto = produtoBusiness.Listar(new ProdutoDto() { Ativo = true });

            return View();
        }

        [HttpGet]
        public JsonResult ListarGrid(PessoaDto pessoaDto)
        {
            var listaPessoaDto = pessoaBusiness.ListarGrid(pessoaDto);

            return Json(new { Sucesso = true, Mensagem = "Pessoa listada com sucesso", Data = listaPessoaDto }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarAutoComplete(PessoaDto pessoaDto)
        {
            var lista = pessoaBusiness.ListarAutoComplete(pessoaDto);

            return Json(new { Sucesso = true, Mensagem = "Pessoa listada com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Listar(PessoaDto pessoaDto)
        {
            var lista = pessoaBusiness.Listar(pessoaDto);

            return Json(new { Sucesso = true, Mensagem = "Pessoa listada com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IncluirAuxiliar(PessoaDto pessoaDto)
        {
            var id = pessoaBusiness.Incluir(pessoaDto);

            return Json(new { Sucesso = true, Mensagem = "Pessoa cadastrada com sucesso", Id = id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(PessoaDto pessoaDto)
        {
            pessoaBusiness.Incluir(pessoaDto);

            var lista = pessoaBusiness.ListarGrid(new PessoaDto() { IdTipoPessoa = (int)TipoPessoaDto.TipoPessoa.PessoaJurídica });

            return Json(new { Sucesso = true, Mensagem = "Pessoa cadastrada com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(PessoaDto pessoaDto)
        {
            pessoaBusiness.Alterar(pessoaDto);

            var lista = pessoaBusiness.ListarGrid(new PessoaDto() { Id = pessoaDto.Id });

            return Json(new { Sucesso = true, Mensagem = "Pessoa alterada com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(PessoaDto pessoaDto)
        {
            pessoaBusiness.AlterarStatus(pessoaDto);

            var lista = pessoaBusiness.ListarGrid(new PessoaDto() { Id = pessoaDto.Id });

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult Exportar(PessoaDto pessoaDto)
        {
            var lista = pessoaBusiness.Listar(pessoaDto);

            string csv = "Nome Empresa; Cnpj; Proprietário; Cep; Logradouro; Número; Complemento; Bairro; Cidade; Uf; Produto; Data Abertura" + Environment.NewLine;

            foreach (var item in lista)
            {
                csv += item.Nome + ";";
                csv += item.Cnpj + ";";
                csv += (item.PessoaProprietarioDto == null ? "" : item.PessoaProprietarioDto.Nome) + ";";
                csv += item.CepResidencia + ";";
                csv += item.LogradouroResidencia + ";";
                csv += item.NumeroResidencia + ";";
                csv += item.ComplementoResidencia + ";";
                csv += item.BairroResidencia + ";";
                csv += item.CidadeResidencia + ";";
                csv += item.UfResidencia + ";";
                csv += (item.ProdutoDto == null ? "" : item.ProdutoDto.Nome) + ";";
                csv += (item.DataAbertura == null ? "" : item.DataAbertura.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";

                csv += Environment.NewLine;
            }

            return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray(), "text/csv", "pessoajuridica-exportar-" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv");
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
            var nome = string.Empty;

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

                    nome = linhaArquivo[0];
                    var cnpj = linhaArquivo[1];
                    var proprietario = linhaArquivo[2];
                    var cepResidencia = linhaArquivo[3];
                    var logradouroResidencia = linhaArquivo[4];
                    var numeroResidencia = linhaArquivo[5];
                    var complementoResidencia = linhaArquivo[6];
                    var bairroResidencia = linhaArquivo[7];
                    var cidadeResidencia = linhaArquivo[8];
                    var ufResidencia = linhaArquivo[9];
                    var produto = linhaArquivo[10];
                    var dataAbertura = linhaArquivo[11];

                    DateTime dtAbertura;
                    DateTime.TryParse(dataAbertura, out dtAbertura);

                    int nroResidencia;
                    int.TryParse(numeroResidencia, out nroResidencia);

                    var idProduto = produtoBusiness.Listar(new ProdutoDto() { Nome = produto }).FirstOrDefault()?.Id;

                    var pessoaDto = new PessoaDto()
                    {
                        Nome = nome,
                        Cnpj = cnpj,
                        CepResidencia = cepResidencia,
                        LogradouroResidencia = logradouroResidencia,
                        NumeroResidencia = nroResidencia,
                        ComplementoResidencia = complementoResidencia,
                        BairroResidencia = bairroResidencia,
                        CidadeResidencia = cidadeResidencia,
                        UfResidencia = ufResidencia,
                        IdProduto = idProduto,
                        DataAbertura = dtAbertura,
                        IdUsuarioCadastro = idUsuario,
                        DataCadastro = dataCadastro,
                        IdTipoPessoa = (int)TipoPessoaDto.TipoPessoa.PessoaJurídica
                    };

                    if (!string.IsNullOrEmpty(proprietario))
                    {
                        var pessoaProprietarioDto = pessoaBusiness.Listar(new PessoaDto() { Nome = proprietario }).FirstOrDefault();

                        if (pessoaProprietarioDto != null)
                            pessoaDto.IdProprietario = pessoaProprietarioDto.Id;
                    }

                    pessoaBusiness.Incluir(pessoaDto);

                    mensagem += $"Pessoa ({nome}) incluída com sucesso" + Environment.NewLine;
                }
                catch (BusinessException ex)
                {
                    mensagem += $"Erro ao incluir pessoa ({nome}): " + ex.Message + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    mensagem += $"Erro ao incluir pessoa ({nome}): " + ex.Message + Environment.NewLine;
                }
            }

            return Json(new { Sucesso = true, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }
    }
}