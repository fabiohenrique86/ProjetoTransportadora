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
    
    public partial class UsuarioGrupo
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdGrupo { get; set; }
    
        public virtual Grupo Grupo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
