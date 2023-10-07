namespace ProjetoTransportadora.Dto
{
    public class TipoContratoDto
    {
        public enum EnumTipoContrato
        {
            Diario = 1,
            Mensal = 2
        }

        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
