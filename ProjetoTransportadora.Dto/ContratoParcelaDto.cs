using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class ContratoParcelaDto
    {
        public ContratoParcelaDto()
        {
            this.ContratoParcelaHistoricoDto = new HashSet<ContratoParcelaHistoricoDto>();
            this.ContratoParcelaResiduoDto = new HashSet<ContratoParcelaResiduoDto>();
        }

        public int Id { get; set; }
        public int NumeroParcela { get; set; }
        public int IdContrato { get; set; }
        public int IdSituacaoParcela { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataVencimento { get; set; }
        public int DiasParcela { get; set; }
        public int DiasContrato { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime DataEmissao { get; set; }
        public double ValorOriginal { get; set; }        
        public double? ValorAmortizacao { get; set; }
        public double? ValorJuros { get; set; }        
        public double? ValorMulta { get; set; }
        public double? ValorMora { get; set; }
        public double? ValorDescontoJuros { get; set; }
        public double? ValorDescontoParcela { get; set; }
        public double ValorParcela { get; set; }
        public virtual SituacaoParcelaDto SituacaoParcelaDto { get; set; }
        public virtual ICollection<ContratoParcelaHistoricoDto> ContratoParcelaHistoricoDto { get; set; }
        public double? TaxaMulta { get; set; }
        public double? TaxaMora { get; set; }
        public double? ValorResiduo { get; set; }
        public double? ValorAcrescimo { get; set; }

        // campos referentes à simulação
        public double Fator { get; set; }
        public double FatorInvertido { get; set; }
        public double ValorSaldoAnterior { get; set; }
        public double ValorSaldoAtual { get; set; }

        // campos referentes a filtros
        public List<int> ListaIdSituacaoParcela { get; set; }
        public DateTime DataVencimentoInicio { get; set; }
        public DateTime DataVencimentoFim { get; set; }
        public List<int> ListaIdContratoParcela { get; set; }

        // campos referentes a última parcela
        public virtual ICollection<ContratoParcelaResiduoDto> ContratoParcelaResiduoDto { get; set; }
        public bool UltimaParcela { get; set; }
    }
}
