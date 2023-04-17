using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoTransportadora.Dto
{
    public class PessoaReferenciaDto
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public int IdTipoReferencia { get; set; }
        public System.DateTime DataReferencia { get; set; }
        public string Descricao { get; set; }
        public virtual TipoReferenciaDto TipoReferenciaDto { get; set; }
    }
}
