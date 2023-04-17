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
    public class PessoaFisicaController : BaseController
    {
        private PessoaBusiness pessoaBusiness;
        private TipoResidenciaBusiness tipoResidenciaBusiness;
        private EstadoCivilBusiness estadoCivilBusiness;
        private RegimeCasamentoBusiness regimeCasamentoBusiness;
        private EstadoBusiness estadoBusiness;
        private TipoTelefoneBusiness tipoTelefoneBusiness;
        private TipoReferenciaBusiness tipoReferenciaBusiness;

        public PessoaFisicaController()
        {
            pessoaBusiness = new PessoaBusiness();
            tipoResidenciaBusiness = new TipoResidenciaBusiness();
            estadoCivilBusiness = new EstadoCivilBusiness();
            regimeCasamentoBusiness = new RegimeCasamentoBusiness();
            estadoBusiness = new EstadoBusiness();
            tipoTelefoneBusiness = new TipoTelefoneBusiness();
            tipoReferenciaBusiness = new TipoReferenciaBusiness();
        }

        public ActionResult Index()
        {
            ViewBag.TipoResidencia = tipoResidenciaBusiness.Listar(new TipoResidenciaDto() { Ativo = true });
            ViewBag.EstadoCivil = estadoCivilBusiness.Listar(new EstadoCivilDto() { Ativo = true });
            ViewBag.RegimeCasamento = regimeCasamentoBusiness.Listar(new RegimeCasamentoDto() { Ativo = true });
            ViewBag.Estado = estadoBusiness.Listar(new EstadoDto());
            ViewBag.TipoTelefone = tipoTelefoneBusiness.Listar(new TipoTelefoneDto() { Ativo = true });
            ViewBag.TipoReferencia = tipoReferenciaBusiness.Listar(new TipoReferenciaDto() { Ativo = true });

            return View();
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
        public JsonResult Incluir(PessoaDto pessoaDto)
        {
            pessoaBusiness.Incluir(pessoaDto);

            var lista = pessoaBusiness.Listar(new PessoaDto() { IdTipoPessoa = (int)TipoPessoaDto.TipoPessoa.PessoaFísica });

            return Json(new { Sucesso = true, Mensagem = "Pessoa cadastrada com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(PessoaDto pessoaDto)
        {
            pessoaBusiness.Alterar(pessoaDto);

            var lista = pessoaBusiness.Listar(new PessoaDto() { Id = pessoaDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Pessoa alterada com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(PessoaDto pessoaDto)
        {
            pessoaBusiness.AlterarStatus(pessoaDto);

            var lista = pessoaBusiness.Listar(new PessoaDto() { Id = pessoaDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult Exportar(PessoaDto pessoaDto)
        {
            var lista = pessoaBusiness.Listar(pessoaDto);

            string csv = "Nome; Estado Civil; Rg; Cpf; Data de Nascimento; Cidade Nascimento; Uf Nascimento; Cep Residência; Logradouro Residência; Número Residência; Complemento Residência; Bairro Residência; Cidade Residência; Uf Residência; Profissão; Tipo Residência; Tempo Residencial; Valor Aluguel; Nome Mãe; Nome Pai; Nome Conjuge; Regime Casamento; Empresa Pessoal; Empresa Trabalho; Data Admissão; Cargo; Valor Salário; Data Referência Salário; Valor Frete" + Environment.NewLine;

            foreach (var item in lista)
            {
                csv += item.Nome + ";";
                csv += (item.EstadoCivilDto == null ? "" : item.EstadoCivilDto.Nome) + ";";
                csv += item.Rg + ";";
                csv += item.Cpf + ";";
                csv += (item.DataNascimento == null ? "" : item.DataNascimento.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";
                csv += item.CidadeNascimento + ";";
                csv += item.UfNascimento + ";";
                csv += item.CepResidencia + ";";
                csv += item.LogradouroResidencia + ";";
                csv += item.NumeroResidencia + ";";
                csv += item.ComplementoResidencia + ";";
                csv += item.BairroResidencia + ";";
                csv += item.CidadeResidencia + ";";
                csv += item.UfResidencia + ";";
                csv += item.Profissao + ";";
                csv += (item.TipoResidenciaDto == null ? "" : item.TipoResidenciaDto.Nome) + ";";
                csv += item.TempoResidencial + ";";
                csv += (item.ValorAluguel == null ? "" : item.ValorAluguel.GetValueOrDefault().ToString("C")) + ";";

                if (item.PessoaMaeDto != null)
                    csv += item.PessoaMaeDto.Nome + ";";
                else
                    csv += ";";

                if (item.PessoaPaiDto != null)
                    csv += item.PessoaPaiDto.Nome + ";";
                else
                    csv += ";";

                if (item.PessoaConjugeDto != null)
                    csv += item.PessoaConjugeDto.Nome + ";";
                else
                    csv += ";";

                csv += (item.RegimeCasamentoDto == null ? "" : item.RegimeCasamentoDto.Nome) + ";";

                csv += item.EmpresaPessoal + ";";
                csv += item.EmpresaTrabalho + ";";
                csv += (item.DataAdmissao == null ? "" : item.DataAdmissao.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";
                csv += item.Cargo + ";";
                csv += (item.ValorSalario == null ? "" : item.ValorSalario.GetValueOrDefault().ToString("C")) + ";";
                csv += (item.DataReferenciaSalario == null ? "" : item.DataReferenciaSalario.GetValueOrDefault().ToString("dd/MM/yyyy")) + ";";
                csv += (item.ValorFrete == null ? "" : item.ValorFrete.GetValueOrDefault().ToString("C")) + ";";

                csv += Environment.NewLine;
            }

            return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray(), "text/csv", "pessoafisica-exportar-" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv");
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

            foreach (var item in conteudoArquivo.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                try
                {
                    var linhaArquivo = item.Split(";".ToCharArray(), StringSplitOptions.None);

                    nome = linhaArquivo[0];
                    var estadoCivil = linhaArquivo[1];
                    var rg = linhaArquivo[2];
                    var cpf = linhaArquivo[3];
                    var dataNascimento = linhaArquivo[4];
                    var cidadeNascimento = linhaArquivo[5];
                    var ufNascimento = linhaArquivo[6];
                    var cepResidencia = linhaArquivo[7];
                    var logradouroResidencia = linhaArquivo[8];
                    var numeroResidencia = linhaArquivo[9];
                    var complementoResidencia = linhaArquivo[10];
                    var bairroResidencia = linhaArquivo[11];
                    var cidadeResidencia = linhaArquivo[12];
                    var ufResidencia = linhaArquivo[13];
                    var profissao = linhaArquivo[14];
                    var tipoResidencia = linhaArquivo[15];
                    var tempoResidencial = linhaArquivo[16];
                    var valorAluguel = linhaArquivo[17];
                    var nomeMae = linhaArquivo[18];
                    var nomePai = linhaArquivo[19];
                    var nomeConjuge = linhaArquivo[20];
                    var regimeCasamento = linhaArquivo[21];
                    var empresaPessoal = linhaArquivo[22];
                    var empresaTrabalho = linhaArquivo[23];
                    var dataAdmissao = linhaArquivo[24];
                    var cargo = linhaArquivo[25];
                    var valorSalario = linhaArquivo[26];
                    var dataReferenciaSalario = linhaArquivo[27];
                    var valorFrete = linhaArquivo[28];

                    var idEstadoCivil = estadoCivilBusiness.Listar(new EstadoCivilDto() { Nome = estadoCivil }).FirstOrDefault()?.Id;
                    var idTipoResidencia = tipoResidenciaBusiness.Listar(new TipoResidenciaDto() { Nome = tipoResidencia }).FirstOrDefault()?.Id;
                    var idRegimeCasamento = regimeCasamentoBusiness.Listar(new RegimeCasamentoDto() { Nome = regimeCasamento }).FirstOrDefault()?.Id;

                    DateTime dtNascimento;
                    DateTime.TryParse(dataNascimento, out dtNascimento);

                    DateTime dtAdmissao;
                    DateTime.TryParse(dataAdmissao, out dtAdmissao);

                    DateTime dtReferenciaSalario;
                    DateTime.TryParse(dataReferenciaSalario, out dtReferenciaSalario);

                    int nroResidencia;
                    int.TryParse(numeroResidencia, out nroResidencia);

                    int tpoResidencial;
                    int.TryParse(tempoResidencial, out tpoResidencial);

                    double vlAluguel;
                    double.TryParse(valorAluguel, out vlAluguel);

                    double vlSalario;
                    double.TryParse(valorSalario, out vlSalario);

                    double vlFrete;
                    double.TryParse(valorFrete, out vlFrete);

                    var pessoaDto = new PessoaDto()
                    {
                        Nome = nome,
                        IdEstadoCivil = idEstadoCivil,
                        Rg = rg,
                        Cpf = cpf,
                        DataNascimento = dtNascimento,
                        CidadeNascimento = cidadeNascimento,
                        UfNascimento = ufNascimento,
                        CepResidencia = cepResidencia,
                        LogradouroResidencia = logradouroResidencia,
                        NumeroResidencia = nroResidencia,
                        ComplementoResidencia = complementoResidencia,
                        BairroResidencia = bairroResidencia,
                        CidadeResidencia = cidadeResidencia,
                        UfResidencia = ufResidencia,
                        Profissao = profissao,
                        IdTipoResidencia = idTipoResidencia,
                        TempoResidencial = tpoResidencial,
                        ValorAluguel = vlAluguel,
                        IdRegimeCasamento = idRegimeCasamento,
                        DataAdmissao = dtAdmissao,
                        Cargo = cargo,
                        ValorSalario = vlSalario,
                        DataReferenciaSalario = dtReferenciaSalario,
                        ValorFrete = vlFrete,
                        EmpresaPessoal = empresaPessoal,
                        EmpresaTrabalho = empresaTrabalho,
                        DataCadastro = dataCadastro,
                        IdUsuarioCadastro = idUsuario,
                        IdTipoPessoa = (int)TipoPessoaDto.TipoPessoa.PessoaFísica
                    };

                    if (!string.IsNullOrEmpty(nomeMae))
                    {
                        var pessoaMaeDto = pessoaBusiness.Listar(new PessoaDto() { Nome = nomeMae, IdTipoPessoa = (int)TipoPessoaDto.TipoPessoa.PessoaFísica }).FirstOrDefault();

                        if (pessoaMaeDto != null)
                            pessoaDto.IdMae = pessoaMaeDto.Id;
                    }

                    if (!string.IsNullOrEmpty(nomePai))
                    {
                        var pessoaPaiDto = pessoaBusiness.Listar(new PessoaDto() { Nome = nomePai, IdTipoPessoa = (int)TipoPessoaDto.TipoPessoa.PessoaFísica }).FirstOrDefault();

                        if (pessoaPaiDto != null)
                            pessoaDto.IdPai = pessoaPaiDto.Id;
                    }

                    if (!string.IsNullOrEmpty(nomeConjuge))
                    {
                        var pessoaConjugeDto = pessoaBusiness.Listar(new PessoaDto() { Nome = nomeConjuge, IdTipoPessoa = (int)TipoPessoaDto.TipoPessoa.PessoaFísica }).FirstOrDefault();

                        if (pessoaConjugeDto != null)
                            pessoaDto.IdConjuge = pessoaConjugeDto.Id;
                    }

                    pessoaBusiness.Incluir(pessoaDto);

                    mensagem += $"Pessoa ({pessoaDto.Nome}) incluída com sucesso" + Environment.NewLine;
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