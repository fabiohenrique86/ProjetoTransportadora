using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class PessoaAvalistaDto
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public int IdAvalista { get; set; }
        public PessoaDto AvalistaDto { get; set; }
    }
}
