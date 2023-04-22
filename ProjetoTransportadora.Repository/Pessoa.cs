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
    
    public partial class Pessoa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pessoa()
        {
            this.Pessoa1 = new HashSet<Pessoa>();
            this.Pessoa11 = new HashSet<Pessoa>();
            this.Pessoa12 = new HashSet<Pessoa>();
            this.Pessoa13 = new HashSet<Pessoa>();
            this.PessoaEmail = new HashSet<PessoaEmail>();
            this.PessoaHistorico = new HashSet<PessoaHistorico>();
            this.PessoaReferencia = new HashSet<PessoaReferencia>();
            this.PessoaTelefone = new HashSet<PessoaTelefone>();
            this.VeiculoMulta = new HashSet<VeiculoMulta>();
            this.Veiculo = new HashSet<Veiculo>();
            this.Veiculo1 = new HashSet<Veiculo>();
        }
    
        public int Id { get; set; }
        public string Nome { get; set; }
        public Nullable<int> IdEstadoCivil { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public Nullable<System.DateTime> DataNascimento { get; set; }
        public string CidadeNascimento { get; set; }
        public string UfNascimento { get; set; }
        public string CepResidencia { get; set; }
        public string LogradouroResidencia { get; set; }
        public string ComplementoResidencia { get; set; }
        public Nullable<int> NumeroResidencia { get; set; }
        public string BairroResidencia { get; set; }
        public string CidadeResidencia { get; set; }
        public string UfResidencia { get; set; }
        public string Profissao { get; set; }
        public Nullable<int> IdTipoResidencia { get; set; }
        public Nullable<int> TempoResidencial { get; set; }
        public Nullable<double> ValorAluguel { get; set; }
        public Nullable<int> IdRegimeCasamento { get; set; }
        public Nullable<System.DateTime> DataAdmissao { get; set; }
        public string Cargo { get; set; }
        public Nullable<double> ValorSalario { get; set; }
        public Nullable<System.DateTime> DataReferenciaSalario { get; set; }
        public Nullable<double> ValorFrete { get; set; }
        public string Cnpj { get; set; }
        public Nullable<int> IdProduto { get; set; }
        public Nullable<System.DateTime> DataAbertura { get; set; }
        public bool Ativo { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<int> IdUsuarioInativacao { get; set; }
        public Nullable<System.DateTime> DataInativacao { get; set; }
        public int IdTipoPessoa { get; set; }
        public Nullable<int> IdPai { get; set; }
        public Nullable<int> IdMae { get; set; }
        public Nullable<int> IdConjuge { get; set; }
        public Nullable<int> IdProprietario { get; set; }
        public string EmpresaPessoal { get; set; }
        public string EmpresaTrabalho { get; set; }
    
        public virtual EstadoCivil EstadoCivil { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pessoa> Pessoa1 { get; set; }
        public virtual Pessoa PessoaConjuge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pessoa> Pessoa11 { get; set; }
        public virtual Pessoa PessoaMae { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pessoa> Pessoa12 { get; set; }
        public virtual Pessoa PessoaPai { get; set; }
        public virtual Produto Produto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pessoa> Pessoa13 { get; set; }
        public virtual Pessoa PessoaProprietario { get; set; }
        public virtual RegimeCasamento RegimeCasamento { get; set; }
        public virtual TipoPessoa TipoPessoa { get; set; }
        public virtual TipoResidencia TipoResidencia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PessoaEmail> PessoaEmail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PessoaHistorico> PessoaHistorico { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PessoaReferencia> PessoaReferencia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PessoaTelefone> PessoaTelefone { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VeiculoMulta> VeiculoMulta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Veiculo> Veiculo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Veiculo> Veiculo1 { get; set; }
    }
}
