namespace ProjetoTransportadora.Dto
{
    public class TipoPessoaDto
    {
        public enum TipoPessoa
        {
            PessoaFísica = 1,
            PessoaJurídica = 2
        }
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
