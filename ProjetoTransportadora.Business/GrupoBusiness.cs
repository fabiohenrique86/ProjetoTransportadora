using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace ProjetoTransportadora.Business
{
    public class GrupoBusiness
    {
        GrupoRepository grupoRepository;
        GrupoFuncionalidadeBusiness grupoFuncionalidadeBusiness;
        public GrupoBusiness()
        {
            grupoRepository = new GrupoRepository();
            grupoFuncionalidadeBusiness = new GrupoFuncionalidadeBusiness();
        }

        public List<GrupoDto> Listar(GrupoDto grupoDto = null)
        {
            return grupoRepository.Listar(grupoDto);
        }

        public int Incluir(GrupoDto grupoDto)
        {
            var idGrupo = 0;

            if (grupoDto == null)
                throw new BusinessException("GrupoDto é nulo");

            if (string.IsNullOrEmpty(grupoDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            if (grupoDto.GrupoFuncionalidadeDto == null || grupoDto.GrupoFuncionalidadeDto.Count() <= 0)
                throw new BusinessException("Funcionalidade é obrigatório");

            var existeGrupoPorNome = grupoRepository.Existe(new GrupoDto() { Nome = grupoDto.Nome });

            if (existeGrupoPorNome)
                throw new BusinessException($"Grupo ({grupoDto.Nome}) já está cadastrado");

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                idGrupo = grupoRepository.Incluir(grupoDto);

                foreach (var grupoFuncionalidadeDto in grupoDto.GrupoFuncionalidadeDto)
                {
                    grupoFuncionalidadeDto.IdGrupo = idGrupo;
                    grupoFuncionalidadeBusiness.Incluir(grupoFuncionalidadeDto);
                }

                scope.Complete();
            }

            return idGrupo;
        }

        public void Alterar(GrupoDto grupoDto)
        {
            if (grupoDto == null)
                throw new BusinessException("GrupoDto é nulo");

            if (grupoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeGrupoPorId = grupoRepository.Obter(new GrupoDto() { Id = grupoDto.Id });

            if (existeGrupoPorId == null)
                throw new BusinessException($"Grupo ({grupoDto.Id}) não está cadastrado");

            if (!string.IsNullOrEmpty(grupoDto.Nome))
            {
                if (grupoDto.Nome != existeGrupoPorId.Nome)
                {
                    var existeGrupoPorNome = grupoRepository.Existe(new GrupoDto() { Nome = grupoDto.Nome });

                    if (existeGrupoPorNome)
                        throw new BusinessException($"Grupo ({grupoDto.Nome}) já está cadastrado");
                }
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                grupoRepository.Alterar(grupoDto);

                // grupo funcionalidade
                grupoFuncionalidadeBusiness.Excluir(grupoDto.Id);

                foreach (var grupoFuncionalidadeDto in grupoDto.GrupoFuncionalidadeDto)
                    grupoFuncionalidadeBusiness.Incluir(grupoFuncionalidadeDto);

                scope.Complete();
            }
        }

        public void AlterarStatus(GrupoDto grupoDto)
        {
            if (grupoDto == null)
                throw new BusinessException("GrupoDto é nulo");

            if (grupoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!grupoDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeGrupoPorId = grupoRepository.Existe(new GrupoDto() { Id = grupoDto.Id });

            if (!existeGrupoPorId)
                throw new BusinessException($"Grupo ({grupoDto.Id}) não está cadastrado");

            if (grupoDto.Ativo.HasValue)
            {
                if (!grupoDto.Ativo.Value)
                {
                    if (grupoDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (grupoDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            grupoRepository.Alterar(grupoDto);
        }
    }
}
