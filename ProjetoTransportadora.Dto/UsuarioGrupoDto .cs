namespace ProjetoTransportadora.Dto
{
    public class UsuarioGrupoDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdGrupo { get; set; }
        public virtual GrupoDto GrupoDto { get; set; }
        public virtual UsuarioDto UsuarioDto { get; set; }
    }
}
