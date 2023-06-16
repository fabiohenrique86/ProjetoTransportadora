namespace ProjetoTransportadora.Dto
{
    public class PessoaTelefoneDto
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public string Pais { get; set; }
        public int? DDD { get; set; }
        public int Numero { get; set; }
        public string NomeContato { get; set; }
        public int? IdTipoTelefone { get; set; }
        public virtual TipoTelefoneDto TipoTelefoneDto { get; set; }
    }
}
