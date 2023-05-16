namespace ProjetoTransportadora.Dto
{
    public class GrupoFuncionalidadeDto
    {
        public int Id { get; set; }
        public int IdGrupo { get; set; }
        public int IdFuncionalidade { get; set; }
        public bool? Inserir { get; set; }
        public bool? Ler { get; set; }
        public bool? Atualizar { get; set; }
        public bool? Excluir { get; set; }
        public bool? Executar { get; set; }
        public virtual GrupoDto GrupoDto { get; set; }
        public virtual FuncionalidadeDto FuncionalidadeDto { get; set; }        
    }
}
