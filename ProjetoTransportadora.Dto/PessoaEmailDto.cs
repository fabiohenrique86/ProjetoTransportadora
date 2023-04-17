using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoTransportadora.Dto
{
    public class PessoaEmailDto
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public string Email { get; set; }
        public string NomeContato { get; set; }
    }
}
