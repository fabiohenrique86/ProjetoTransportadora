using System;

namespace ProjetoTransportadora.Dto
{
    public class TipoReferenciaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<int> IdUsuarioInativacao { get; set; }
        public Nullable<System.DateTime> DataInativacao { get; set; }
    }
}
