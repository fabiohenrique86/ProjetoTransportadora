namespace ProjetoTransportadora.Dto
{
    public class VeiculoMultaDto
    {
        public int Id { get; set; }
        public int IdVeiculo { get; set; }
        public System.DateTime DataMulta { get; set; }
        public string Local { get; set; }
        public int IdCondutor { get; set; }
        public System.DateTime DataVencimentoMulta { get; set; }
        public decimal ValorMulta { get; set; }
        public int IdSituacaoMulta { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public PessoaDto PessoaCondutorDto { get; set; }
        public SituacaoMultaDto SituacaoMultaDto { get; set; }
    }
}
