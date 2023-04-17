using ProjetoTransportadora.Dto;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class UsuarioGrupoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public UsuarioGrupoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public int Incluir(UsuarioGrupoDto usuarioGrupoDto)
        {
            var usuarioGrupo = new UsuarioGrupo()
            {
                IdGrupo = usuarioGrupoDto.IdGrupo,
                IdUsuario = usuarioGrupoDto.IdUsuario
            };

            projetoTransportadoraEntities.UsuarioGrupo.Add(usuarioGrupo);
            projetoTransportadoraEntities.SaveChanges();

            return usuarioGrupo.Id;
        }

        public void Excluir(int idUsuario)
        {
            var usuarioGrupo = projetoTransportadoraEntities.UsuarioGrupo.Where(x => x.IdUsuario == idUsuario);

            projetoTransportadoraEntities.UsuarioGrupo.RemoveRange(usuarioGrupo);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
