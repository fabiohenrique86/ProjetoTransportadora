using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class SituacaoContratoBusiness
    {
        SituacaoContratoRepository situacaoContratoRepository;
        public SituacaoContratoBusiness()
        {
            situacaoContratoRepository = new SituacaoContratoRepository();
        }

        public SituacaoContratoDto Obter(SituacaoContratoDto situacaoContratoDto = null)
        {
            return situacaoContratoRepository.Obter(situacaoContratoDto);
        }

        public List<SituacaoContratoDto> Listar(SituacaoContratoDto situacaoContratoDto = null)
        {
            return situacaoContratoRepository.Listar(situacaoContratoDto);
        }

        public int Incluir(SituacaoContratoDto situacaoContratoDto)
        {
            var idSituacaoContrato = 0;

            if (situacaoContratoDto == null)
                throw new BusinessException("SituacaoContratoDto é nulo");

            if (string.IsNullOrEmpty(situacaoContratoDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            var existeSituacaoContrato = situacaoContratoRepository.Existe(new SituacaoContratoDto() { Nome = situacaoContratoDto.Nome });

            if (existeSituacaoContrato)
                throw new BusinessException($"SituacaoContrato ({situacaoContratoDto.Nome}) já está cadastrado");

            idSituacaoContrato = situacaoContratoRepository.Incluir(situacaoContratoDto);

            return idSituacaoContrato;
        }

        public void Alterar(SituacaoContratoDto situacaoContratoDto)
        {
            if (situacaoContratoDto == null)
                throw new BusinessException("SituacaoContratoDto é nulo");

            if (situacaoContratoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeSituacaoContratoPorId = situacaoContratoRepository.Obter(new SituacaoContratoDto() { Id = situacaoContratoDto.Id });

            if (existeSituacaoContratoPorId == null)
                throw new BusinessException($"SituacaoContrato ({situacaoContratoDto.Id}) não está cadastrado");

            if (!string.IsNullOrEmpty(situacaoContratoDto.Nome))
            {
                if (situacaoContratoDto.Nome != existeSituacaoContratoPorId.Nome)
                {
                    var existeSituacaoContratoPorNome = situacaoContratoRepository.Existe(new SituacaoContratoDto() { Nome = situacaoContratoDto.Nome });

                    if (existeSituacaoContratoPorNome)
                        throw new BusinessException($"Situação de Veículo ({situacaoContratoDto.Nome}) já está cadastrado");
                }
            }

            situacaoContratoRepository.Alterar(situacaoContratoDto);
        }

        public void AlterarStatus(SituacaoContratoDto situacaoContratoDto)
        {
            if (situacaoContratoDto == null)
                throw new BusinessException("SituacaoContratoDto é nulo");

            if (situacaoContratoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!situacaoContratoDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeSituacaoContrato = situacaoContratoRepository.Existe(new SituacaoContratoDto() { Id = situacaoContratoDto.Id });

            if (!existeSituacaoContrato)
                throw new BusinessException($"SituacaoContrato ({situacaoContratoDto.Id}) não está cadastrado");

            if (situacaoContratoDto.Ativo.HasValue)
            {
                if (!situacaoContratoDto.Ativo.Value)
                {
                    if (situacaoContratoDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (situacaoContratoDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            situacaoContratoRepository.Alterar(situacaoContratoDto);
        }
    }
}
