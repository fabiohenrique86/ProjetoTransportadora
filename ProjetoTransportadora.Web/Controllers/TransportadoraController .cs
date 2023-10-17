using ProjetoTransportadora.Business;
using ProjetoTransportadora.Dto;
using System;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class TransportadoraController : Controller
    {
        private PessoaBusiness pessoaBusiness;
        private UsuarioBusiness usuarioBusiness;
        private VeiculoBusiness veiculoBusiness;
        private ContratoBusiness contratoBusiness;
        private ContratoParcelaBusiness contratoParcelaBusiness;

        public TransportadoraController()
        {
            pessoaBusiness = new PessoaBusiness();
            usuarioBusiness = new UsuarioBusiness();
            veiculoBusiness = new VeiculoBusiness();
            contratoBusiness = new ContratoBusiness();
            contratoParcelaBusiness = new ContratoParcelaBusiness();
        }

        public ActionResult Index()
        {
            ViewBag.QuantidadePessoasFisicas = pessoaBusiness.ListarTotal(new PessoaDto() { Ativo = true, DataCadastro = DateTime.Now, IdTipoPessoa = (int)TipoPessoaDto.TipoPessoa.PessoaFísica });
            ViewBag.QuantidadePessoasJuridicas = pessoaBusiness.ListarTotal(new PessoaDto() { Ativo = true, DataCadastro = DateTime.Now, IdTipoPessoa = (int)TipoPessoaDto.TipoPessoa.PessoaJurídica });
            ViewBag.QuantidadeUsuarios = usuarioBusiness.ListarTotal(new UsuarioDto() { Ativo = true, DataCadastro = DateTime.Now });
            ViewBag.QuantidadeContratos = contratoBusiness.ListarTotal(new ContratoDto() { Ativo = true, DataCadastro = DateTime.Now });
            ViewBag.QuantidadeVeiculos = veiculoBusiness.ListarTotal(new VeiculoDto() { Ativo = true, DataCadastro = DateTime.Now });
            ViewBag.QuantidadeParcelaPaga = contratoParcelaBusiness.ListarParcelaPaga(new ContratoParcelaDto() { DataPagamento = DateTime.Now });

            return View();
        }
    }
}