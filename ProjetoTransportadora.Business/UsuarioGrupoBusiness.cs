using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;

namespace ProjetoTransportadora.Business
{
    public class UsuarioGrupoBusiness
    {
        UsuarioGrupoRepository usuarioGrupoRepository;
        public UsuarioGrupoBusiness()
        {
            usuarioGrupoRepository = new UsuarioGrupoRepository();
        }

        public int Incluir(UsuarioGrupoDto usuarioGrupoDto)
        {
            if (usuarioGrupoDto == null)
                throw new BusinessException("usuarioGrupoDto é nulo");

            if (usuarioGrupoDto.IdGrupo <= 0)
                throw new BusinessException("Grupo é obrigatório");

            if (usuarioGrupoDto.IdUsuario <= 0)
                throw new BusinessException("Usuario é obrigatório");

            return usuarioGrupoRepository.Incluir(usuarioGrupoDto);
        }

        public void Excluir(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new BusinessException("idUsuario é obrigatório");

            usuarioGrupoRepository.Excluir(idUsuario);
        }
    }
}
