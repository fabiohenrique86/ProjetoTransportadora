using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class UsuarioDto
    {
        public UsuarioDto()
        {
            this.UsuarioGrupoDto = new List<UsuarioGrupoDto>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool? Ativo { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<int> IdUsuarioInativacao { get; set; }
        public Nullable<System.DateTime> DataInativacao { get; set; }
        public bool? TrocarSenha { get; set; }
        public virtual List<UsuarioGrupoDto> UsuarioGrupoDto { get; set; }
    }
}
