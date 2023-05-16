//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjetoTransportadora.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class ContratoParcela
    {
        public int Id { get; set; }
        public int NumeroParcela { get; set; }
        public int IdContrato { get; set; }
        public int IdSituacaoParcela { get; set; }
        public System.DateTime DataVencimento { get; set; }
        public int DiasParcela { get; set; }
        public int DiasContrato { get; set; }
        public Nullable<System.DateTime> DataPagamento { get; set; }
        public System.DateTime DataEmissao { get; set; }
        public double ValorOriginal { get; set; }
        public Nullable<double> ValorAmortizacao { get; set; }
        public Nullable<double> ValorJuros { get; set; }
        public Nullable<double> ValorMulta { get; set; }
        public Nullable<double> ValorMora { get; set; }
        public Nullable<double> ValorDesconto { get; set; }
        public double ValorParcela { get; set; }
    
        public virtual Contrato Contrato { get; set; }
        public virtual SituacaoParcela SituacaoParcela { get; set; }
    }
}
