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
    
    public partial class Contrato
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contrato()
        {
            this.ContratoHistorico = new HashSet<ContratoHistorico>();
            this.ContratoParcela = new HashSet<ContratoParcela>();
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
        public bool Ativo { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<int> IdUsuarioInativacao { get; set; }
        public Nullable<System.DateTime> DataInativacao { get; set; }
        public Nullable<double> ValorTarifa { get; set; }
    
        public virtual Canal Canal { get; set; }
        public virtual Pessoa PessoaCliente { get; set; }
        public virtual Pessoa PessoaFiador { get; set; }
        public virtual Pessoa PessoaIndicacao { get; set; }
        public virtual Pessoa PessoaPromotor { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Veiculo Veiculo { get; set; }
        public virtual Veiculo VeiculoEntrada { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContratoHistorico> ContratoHistorico { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContratoParcela> ContratoParcela { get; set; }
        public virtual SituacaoContrato SituacaoContrato { get; set; }
    }
}
