using System;

namespace ProjetoTransportadora.Dto
{
    public class SituacaoContratoDto
    {
        public enum EnumSituacaoContrato
        {
            Ativo = 1,
            Antecipado = 2,
            Baixado = 3,
            Liquidado = 4
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public bool? Ativo { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<int> IdUsuarioInativacao { get; set; }
        public Nullable<System.DateTime> DataInativacao { get; set; }
    }
}
