using System;

namespace ProjetoTransportadora.Dto
{
    public class SituacaoParcelaDto
    {
        public enum EnumSituacaoParcela
        {
            Pendente = 1,
            BoletoEmitido = 2,
            Paga = 3,
            Atraso = 4,
            Antecipado = 5,
            Baixado = 6
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
