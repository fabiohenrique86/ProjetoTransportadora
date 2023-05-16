using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class ContratoDto
    {
        public ContratoDto()
        {
            this.ContratoHistoricoDto = new List<ContratoHistoricoDto>();
            this.ContratoParcelaDto = new List<ContratoParcelaDto>();
        }

        public int Id { get; set; }
        public int NumeroContrato { get; set; }
        public int IdCliente { get; set; }
        public int IdVeiculo { get; set; }
        public int IdProduto { get; set; }
        public int IdSituacaoContrato { get; set; }
        public Nullable<int> IdCanal { get; set; }
        public System.DateTime DataContrato { get; set; }
        public Nullable<int> IdFiador { get; set; }
        public Nullable<int> IdIndicacao { get; set; }
        public Nullable<int> IdPromotor { get; set; }
        public System.DateTime DataPrimeiraParcela { get; set; }
        public Nullable<System.DateTime> DataBaixa { get; set; }
        public Nullable<System.DateTime> DataAntecipacao { get; set; }
        public Nullable<double> ValorAntecipacao { get; set; }
        public Nullable<double> ValorVeiculo { get; set; }
        public Nullable<double> ValorEntrada { get; set; }
        public Nullable<double> ValorDocumentacao { get; set; }
        public Nullable<double> ValorDesconto { get; set; }
        public Nullable<double> ValorFinanciadoVeiculo { get; set; }
        public Nullable<double> ValorFinanciadoDocumentacao { get; set; }
        public Nullable<int> IdVeiculoEntrada { get; set; }
        public double ValorFinanciado { get; set; }
        public Nullable<double> ValorCaixa { get; set; }
        public Nullable<double> ValorDepositado { get; set; }
        public double TaxaJuros { get; set; }
        public bool? Ativo { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<int> IdUsuarioInativacao { get; set; }
        public Nullable<System.DateTime> DataInativacao { get; set; }
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
        public System.DateTime DataContratoInicial { get; set; }
        public System.DateTime DataContratoFinal { get; set; }
    }
}
