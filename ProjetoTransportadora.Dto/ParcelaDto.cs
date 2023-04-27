using System;

namespace ProjetoTransportadora.Dto
{
    public class ParcelaDto
    {
        public int Id { get; set; }
        public DateTime DataVencimento { get; set; }
        public int DiasContrato { get; set; }        
        public int DiasParcela { get; set; }
        public double Fator { get; set; }
        public double FatorInvertido { get; set; }
        public double ValorParcela { get; set; }
        public double ValorSaldoAnterior { get; set; }
        public double ValorJuros { get; set; }
        public double ValorAmortizacao { get; set; }
        public double ValorSaldoAtual { get; set; }
    }
}
