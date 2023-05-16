namespace ProjetoTransportadora.Dto
{
    public class ContratoHistoricoDto
    {
        public int Id { get; set; }
        public int IdContrato { get; set; }
        public System.DateTime DataHistorico { get; set; }
        public string Descricao { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
    }
}
