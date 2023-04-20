using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class SituacaoMultaBusiness
    {
        SituacaoMultaRepository situacaoMultaRepository;
        public SituacaoMultaBusiness()
        {
            situacaoMultaRepository = new SituacaoMultaRepository();
        }

        public List<SituacaoMultaDto> Listar(SituacaoMultaDto situacaoMultaDto = null)
        {
            return situacaoMultaRepository.Listar(situacaoMultaDto);
        }

        public int Incluir(SituacaoMultaDto situacaoMultaDto)
        {
            var idSituacaoMulta = 0;

            if (situacaoMultaDto == null)
                throw new BusinessException("SituacaoMultaDto é nulo");

            if (string.IsNullOrEmpty(situacaoMultaDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            var existeSituacaoMulta = situacaoMultaRepository.Existe(new SituacaoMultaDto() { Nome = situacaoMultaDto.Nome });

            if (existeSituacaoMulta)
                throw new BusinessException($"SituacaoMulta ({situacaoMultaDto.Nome}) já está cadastrado");

            idSituacaoMulta = situacaoMultaRepository.Incluir(situacaoMultaDto);

            return idSituacaoMulta;
        }

        public void Alterar(SituacaoMultaDto situacaoMultaDto)
        {
            if (situacaoMultaDto == null)
                throw new BusinessException("SituacaoMultaDto é nulo");

            if (situacaoMultaDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeSituacaoMultaPorId = situacaoMultaRepository.Obter(new SituacaoMultaDto() { Id = situacaoMultaDto.Id });

            if (existeSituacaoMultaPorId == null)
                throw new BusinessException($"SituacaoMulta ({situacaoMultaDto.Id}) não está cadastrado");

            if (!string.IsNullOrEmpty(situacaoMultaDto.Nome))
            {
                if (situacaoMultaDto.Nome != existeSituacaoMultaPorId.Nome)
                {
                    var existeSituacaoMultaPorNome = situacaoMultaRepository.Existe(new SituacaoMultaDto() { Nome = situacaoMultaDto.Nome });

                    if (existeSituacaoMultaPorNome)
                        throw new BusinessException($"Situação de Veículo ({situacaoMultaDto.Nome}) já está cadastrado");
                }
            }

            situacaoMultaRepository.Alterar(situacaoMultaDto);
        }
    }
}
