using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class ContratoParcelaDto
    {
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
        public double? ValorDesconto { get; set; }
        public double ValorParcela { get; set; }
        public virtual SituacaoParcelaDto SituacaoParcelaDto { get; set; }

        // campos referentes à simulação
        public double Fator { get; set; }
        public double FatorInvertido { get; set; }
        public double ValorSaldoAnterior { get; set; }
        public double ValorSaldoAtual { get; set; }

        // campos referentes a filtros
        public List<int> ListaIdSituacaoParcela { get; set; }
    }
}
