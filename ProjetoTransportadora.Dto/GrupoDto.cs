using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class GrupoDto
    {
        public GrupoDto()
        {
            this.GrupoFuncionalidadeDto = new List<GrupoFuncionalidadeDto>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public bool? Ativo { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<int> IdUsuarioInativacao { get; set; }
        public Nullable<System.DateTime> DataInativacao { get; set; }
        public virtual List<GrupoFuncionalidadeDto> GrupoFuncionalidadeDto { get; set; }
    }
}
