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
    
    public partial class PessoaTelefone
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public string Pais { get; set; }
        public int DDD { get; set; }
        public int Numero { get; set; }
        public string NomeContato { get; set; }
        public int IdTipoTelefone { get; set; }
    
        public virtual TipoTelefone TipoTelefone { get; set; }
        public virtual Pessoa Pessoa { get; set; }
    }
}
