namespace ProjetoTransportadora.Dto
{
    public class PessoaRelacaoDto
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public int IdTipoRelacao { get; set; }
        public string Nome { get; set; }
        public virtual TipoRelacaoDto TipoReferenciaDto { get; set; }
    }
}
