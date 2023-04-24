using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class TransportadoraController : Controller
    {
        private PessoaBusiness pessoaBusiness;
        private UsuarioBusiness usuarioBusiness;
        private VeiculoBusiness veiculoBusiness;

        public TransportadoraController()
        {
            pessoaBusiness = new PessoaBusiness();
            usuarioBusiness = new UsuarioBusiness();
            veiculoBusiness = new VeiculoBusiness();
        }

        public ActionResult Index()
        {
            var listaPessoaDto = pessoaBusiness.Listar(new PessoaDto() { Ativo = true });

            ViewBag.QuantidadePessoasFisicas = listaPessoaDto?.Where(x => x.DataCadastro.Month == DateTime.Now.Month && x.DataCadastro.Year == DateTime.Now.Year && x.IdTipoPessoa == (int)TipoPessoaDto.TipoPessoa.PessoaFísica).Count();
            ViewBag.QuantidadePessoasJuridicas = listaPessoaDto?.Where(x => x.DataCadastro.Month == DateTime.Now.Month && x.DataCadastro.Year == DateTime.Now.Year && x.IdTipoPessoa == (int)TipoPessoaDto.TipoPessoa.PessoaJurídica).Count();
            ViewBag.QuantidadeUsuarios = usuarioBusiness.Listar(new UsuarioDto() { Ativo = true })?.Where(x => x.DataCadastro.Month == DateTime.Now.Month && x.DataCadastro.Year == DateTime.Now.Year).Count();
            ViewBag.QuantidadeVeiculos = veiculoBusiness.Listar(new VeiculoDto() { Ativo = true })?.Where(x => x.DataCadastro.Month == DateTime.Now.Month && x.DataCadastro.Year == DateTime.Now.Year).Count();

            return View();
        }
    }
}