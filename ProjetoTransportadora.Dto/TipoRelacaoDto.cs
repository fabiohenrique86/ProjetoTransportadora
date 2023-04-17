using System;

namespace ProjetoTransportadora.Dto
{
    public class TipoRelacaoDto
    {
        public enum TipoRelacao
        {
            Pai = 1,
            Mãe = 2,
            Conjuge = 3,
            EmpresaPessoal = 4,
            EmpresaTrabalho = 5
        }
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
