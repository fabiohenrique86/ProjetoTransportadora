using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class SituacaoVeiculoBusiness
    {
        SituacaoVeiculoRepository situacaoVeiculoRepository;
        public SituacaoVeiculoBusiness()
        {
            situacaoVeiculoRepository = new SituacaoVeiculoRepository();
        }

        public List<SituacaoVeiculoDto> Listar(SituacaoVeiculoDto situacaoVeiculoDto = null)
        {
            return situacaoVeiculoRepository.Listar(situacaoVeiculoDto);
        }

        public int Incluir(SituacaoVeiculoDto situacaoVeiculoDto)
        {
            var idSituacaoVeiculo = 0;

            if (situacaoVeiculoDto == null)
                throw new BusinessException("SituacaoVeiculoDto é nulo");

            if (string.IsNullOrEmpty(situacaoVeiculoDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            var existeSituacaoVeiculo = situacaoVeiculoRepository.Existe(new SituacaoVeiculoDto() { Nome = situacaoVeiculoDto.Nome });

            if (existeSituacaoVeiculo)
                throw new BusinessException($"SituacaoVeiculo ({situacaoVeiculoDto.Nome}) já está cadastrado");

            idSituacaoVeiculo = situacaoVeiculoRepository.Incluir(situacaoVeiculoDto);

            return idSituacaoVeiculo;
        }

        public void Alterar(SituacaoVeiculoDto situacaoVeiculoDto)
        {
            if (situacaoVeiculoDto == null)
                throw new BusinessException("SituacaoVeiculoDto é nulo");

            if (situacaoVeiculoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeSituacaoVeiculoPorId = situacaoVeiculoRepository.Obter(new SituacaoVeiculoDto() { Id = situacaoVeiculoDto.Id });

            if (existeSituacaoVeiculoPorId == null)
                throw new BusinessException($"SituacaoVeiculo ({situacaoVeiculoDto.Id}) não está cadastrado");

            if (!string.IsNullOrEmpty(situacaoVeiculoDto.Nome))
            {
                if (situacaoVeiculoDto.Nome != existeSituacaoVeiculoPorId.Nome)
                {
                    var existeSituacaoVeiculoPorNome = situacaoVeiculoRepository.Existe(new SituacaoVeiculoDto() { Nome = situacaoVeiculoDto.Nome });

                    if (existeSituacaoVeiculoPorNome)
                        throw new BusinessException($"Situação de Veículo ({situacaoVeiculoDto.Nome}) já está cadastrado");
                }
            }

            situacaoVeiculoRepository.Alterar(situacaoVeiculoDto);
        }

        public void AlterarStatus(SituacaoVeiculoDto situacaoVeiculoDto)
        {
            if (situacaoVeiculoDto == null)
                throw new BusinessException("SituacaoVeiculoDto é nulo");

            if (situacaoVeiculoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!situacaoVeiculoDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeSituacaoVeiculo = situacaoVeiculoRepository.Existe(new SituacaoVeiculoDto() { Id = situacaoVeiculoDto.Id });

            if (!existeSituacaoVeiculo)
                throw new BusinessException($"SituacaoVeiculo ({situacaoVeiculoDto.Id}) não está cadastrado");

            if (situacaoVeiculoDto.Ativo.HasValue)
            {
                if (!situacaoVeiculoDto.Ativo.Value)
                {
                    if (situacaoVeiculoDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (situacaoVeiculoDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            situacaoVeiculoRepository.Alterar(situacaoVeiculoDto);
        }
    }
}
