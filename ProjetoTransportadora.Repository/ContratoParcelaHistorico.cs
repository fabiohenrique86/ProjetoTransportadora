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
    
    public partial class ContratoParcelaHistorico
    {
        public int Id { get; set; }
        public int IdContratoParcela { get; set; }
        public System.DateTime DataHistorico { get; set; }
        public string Descricao { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
    
        public virtual ContratoParcela ContratoParcela { get; set; }
    }
}
