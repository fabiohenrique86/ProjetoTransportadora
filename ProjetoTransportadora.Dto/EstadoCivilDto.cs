using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoTransportadora.Dto
{
    public class EstadoCivilDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool? Ativo { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<int> IdUsuarioInativacao { get; set; }
        public Nullable<System.DateTime> DataInativacao { get; set; }
    }
}
