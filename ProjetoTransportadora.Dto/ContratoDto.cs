using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class ContratoDto
    {
        public ContratoDto()
        {
            ContratoHistoricoDto = new List<ContratoHistoricoDto>();
            ContratoParcelaDto = new List<ContratoParcelaDto>();
        }

        public int Id { get; set; }
        public int NumeroContrato { get; set; }
        public int IdCliente { get; set; }
        public int IdVeiculo { get; set; }
        public int IdProduto { get; set; }
        public int IdSituacaoContrato { get; set; }
        public int? IdCanal { get; set; }
        public DateTime DataContrato { get; set; }
        public int? IdFiador { get; set; }
        public int? IdIndicacao { get; set; }
        public int? IdPromotor { get; set; }
        public System.DateTime DataPrimeiraParcela { get; set; }
        public int? IdUsuarioBaixa { get; set; }
        public DateTime? DataBaixa { get; set; }
        public int? IdUsuarioAntecipacao { get; set; }
        public DateTime? DataAntecipacao { get; set; }
        public double? ValorAntecipacao { get; set; }
        public double? ValorVeiculo { get; set; }
        public double? ValorEntrada { get; set; }
        public double? ValorDocumentacao { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorFinanciadoVeiculo { get; set; }
        public double? ValorFinanciadoDocumentacao { get; set; }
        public int? IdVeiculoEntrada { get; set; }
        public double? ValorVeiculoEntrada { get; set; }
        public double ValorFinanciado { get; set; }
        public double? ValorDinheiro { get; set; }
        public double? ValorDepositado { get; set; }
        public double? ValorTarifa { get; set; }
        public double TaxaJuros { get; set; }
        public double? TaxaMulta { get; set; }
        public double? TaxaMora { get; set; }
        public bool? Ativo { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public int? IdUsuarioInativacao { get; set; }
        public DateTime? DataInativacao { get; set; }
        public virtual CanalDto CanalDto { get; set; }
        public virtual PessoaDto PessoaClienteDto { get; set; }
        public virtual PessoaDto PessoaFiadorDto { get; set; }
        public virtual PessoaDto PessoaIndicacaoDto { get; set; }
        public virtual PessoaDto PessoaPromotorDto { get; set; }
        public virtual ProdutoDto ProdutoDto { get; set; }
        public virtual SituacaoContratoDto SituacaoContratoDto { get; set; }
        public virtual VeiculoDto VeiculoDto { get; set; }
        public virtual VeiculoDto VeiculoEntradaDto { get; set; }
        public virtual List<ContratoHistoricoDto> ContratoHistoricoDto { get; set; }
        public virtual List<ContratoParcelaDto> ContratoParcelaDto { get; set; }
        public DateTime DataContratoInicial { get; set; }
        public DateTime DataContratoFinal { get; set; }
    }
}
