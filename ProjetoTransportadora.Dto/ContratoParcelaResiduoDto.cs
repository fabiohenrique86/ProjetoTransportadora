using System;

namespace ProjetoTransportadora.Dto
{
    public class ContratoParcelaResiduoDto
    {
        public int NumeroParcela { get; set; }
        public DateTime DataVencimento { get; set; }
        public double? ValorResiduo { get; set; }
    }
}
