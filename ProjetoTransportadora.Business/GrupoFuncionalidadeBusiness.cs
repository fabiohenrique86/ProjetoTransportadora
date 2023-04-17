using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class GrupoFuncionalidadeBusiness
    {
        GrupoFuncionalidadeRepository grupoFuncionalidadeRepository;
        public GrupoFuncionalidadeBusiness()
        {
            grupoFuncionalidadeRepository = new GrupoFuncionalidadeRepository();
        }

        public List<GrupoFuncionalidadeDto> Listar(GrupoFuncionalidadeDto grupoFuncionalidadeDto = null)
        {
            return grupoFuncionalidadeRepository.Listar(grupoFuncionalidadeDto);
        }

        public int Incluir(GrupoFuncionalidadeDto grupoFuncionalidadeDto)
        {
            if (grupoFuncionalidadeDto == null)
                throw new BusinessException("GrupoFuncionalidadeDto é nulo");

            if (grupoFuncionalidadeDto.IdGrupo <= 0)
                throw new BusinessException("Grupo é obrigatório");

            if (grupoFuncionalidadeDto.IdFuncionalidade <= 0)
                throw new BusinessException("Funcionalidade é obrigatório");

            return grupoFuncionalidadeRepository.Incluir(grupoFuncionalidadeDto);
        }

        public void Excluir(int idGrupo)
        {
            if (idGrupo <= 0)
                throw new BusinessException("idGrupo é obrigatório");

            grupoFuncionalidadeRepository.Excluir(idGrupo);
        }

        public void AlterarStatus(GrupoFuncionalidadeDto grupoFuncionalidadeDto)
        {
            if (grupoFuncionalidadeDto == null)
                throw new BusinessException("usuarioDto é nulo");

            if (grupoFuncionalidadeDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeGrupoFuncionalidadePorId = grupoFuncionalidadeRepository.Existe(new GrupoFuncionalidadeDto() { Id = grupoFuncionalidadeDto.Id });

            if (!existeGrupoFuncionalidadePorId)
                throw new BusinessException($"GrupoFuncionalidade ({grupoFuncionalidadeDto.Id}) não está cadastrado");

            grupoFuncionalidadeRepository.AlterarStatus(grupoFuncionalidadeDto);
        }
    }
}
