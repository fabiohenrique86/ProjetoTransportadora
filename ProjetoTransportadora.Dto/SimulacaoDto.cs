using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class SimulacaoDto
    {
        public SimulacaoDto()
        {
            ContratoParcelaDto = new List<ContratoParcelaDto>();
        }

        public DateTime DataInicio { get; set; }
        public double ValorPrincipal { get; set; }
        public DateTime DataPrimeiraParcela { get; set; }
        public int QuantidadeParcela { get; set; }
        public double TaxaMensalJuros { get; set; }
        public double ValorDocumentacao { get; set; }
        public double ValorTarifa { get; set; }
        public double ValorDesconto { get; set; }
        public double ValorBens { get; set; }
        public double ValorDinheiro { get; set; }
        public double ValorFinanciado { get; set; }
        public List<ContratoParcelaDto> ContratoParcelaDto { get; set; }
        public int IdTipoContrato { get; set; }

        // campos referentes a filtros
        public int IdContrato { get; set; }
    }
}
