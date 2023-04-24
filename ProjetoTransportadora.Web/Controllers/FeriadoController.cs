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
    public class FeriadoController : BaseController
    {
        private FeriadoBusiness feriadoBusiness;

        public FeriadoController()
        {
            feriadoBusiness = new FeriadoBusiness();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Listar(FeriadoDto feriadoDto)
        {
            var lista = feriadoBusiness.Listar(feriadoDto);

            return Json(new { Sucesso = true, Mensagem = "Feriado listado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Incluir(FeriadoDto feriadoDto)
        {
            feriadoBusiness.Incluir(feriadoDto);

            var lista = feriadoBusiness.Listar();

            return Json(new { Sucesso = true, Mensagem = "Feriado cadastrado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(FeriadoDto feriadoDto)
        {
            feriadoBusiness.Alterar(feriadoDto);

            var lista = feriadoBusiness.Listar(new FeriadoDto() { Id = feriadoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Feriado alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AlterarStatus(FeriadoDto feriadoDto)
        {
            feriadoBusiness.AlterarStatus(feriadoDto);

            var lista = feriadoBusiness.Listar(new FeriadoDto() { Id = feriadoDto.Id }).FirstOrDefault();

            return Json(new { Sucesso = true, Mensagem = "Status alterado com sucesso", Data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult Exportar(FeriadoDto feriadoDto)
        {
            var lista = feriadoBusiness.Listar(feriadoDto);

            string csv = "Nome Feriado; Data Feriado" + Environment.NewLine;

            foreach (var item in lista)
            {
                csv += item.Nome + ";";
                csv += item.DataFeriado.ToString("dd/MM/yyyy") + ";";

                csv += Environment.NewLine;
            }

            return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray(), "text/csv", "feriado-exportar-" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv");
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
            var dataFeriado = string.Empty;

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

                    nome = linhaArquivo[0]?.Trim();
                    dataFeriado = linhaArquivo[1];

                    DateTime dtFeriado;
                    DateTime.TryParse(dataFeriado, out dtFeriado);

                    feriadoBusiness.Incluir(new FeriadoDto() { Nome = nome, DataFeriado = dtFeriado, IdUsuarioCadastro = idUsuario, DataCadastro = dataCadastro });

                    mensagem += $"Feriado ({nome} - {dataFeriado}) incluído com sucesso" + Environment.NewLine;
                }
                catch (BusinessException ex)
                {
                    mensagem += $"Erro ao incluir feriado ({nome} - {dataFeriado}): " + ex.Message + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    mensagem += $"Erro ao incluir feriado ({nome} - {dataFeriado}): " + ex.Message + Environment.NewLine;
                }
            }

            return Json(new { Sucesso = true, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }
    }
}