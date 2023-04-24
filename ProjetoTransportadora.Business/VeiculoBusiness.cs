using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace ProjetoTransportadora.Business
{
    public class VeiculoBusiness : BaseBusiness
    {
        VeiculoRepository veiculoRepository;
        VeiculoHistoricoBusiness veiculoHistoricoBusiness;
        VeiculoMultaBusiness veiculoMultaBusiness;
        public VeiculoBusiness()
        {
            veiculoRepository = new VeiculoRepository();
            veiculoHistoricoBusiness = new VeiculoHistoricoBusiness();
            veiculoMultaBusiness = new VeiculoMultaBusiness();
        }

        public List<VeiculoDto> Listar(VeiculoDto veiculoDto = null)
        {
            return veiculoRepository.Listar(veiculoDto);
        }

        public int Incluir(VeiculoDto veiculoDto)
        {
            var idVeiculo = 0;

            if (veiculoDto == null)
                throw new BusinessException("VeiculoDto é nulo");

            if (veiculoDto.IdMontadora <= 0)
                throw new BusinessException("Montadora é obrigatório");

            if (string.IsNullOrEmpty(veiculoDto.Modelo))
                throw new BusinessException("Modelo é obrigatório");

            if (string.IsNullOrEmpty(veiculoDto.Placa))
                throw new BusinessException("Placa é obrigatório");

            veiculoDto.Placa = veiculoDto.Placa?.Replace(".", "").Replace(",", "").Replace("-", "").Replace("/", "").Replace(" ", "").Replace("*", "").Replace("_", "").Trim();

            var veiculoExistePorPlaca = veiculoRepository.Existe(new VeiculoDto() { Placa = veiculoDto.Placa });

            if (veiculoExistePorPlaca)
                throw new BusinessException($"Veiculo com Placa ({veiculoDto.Placa}) já está cadastrado");

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                idVeiculo = veiculoRepository.Incluir(veiculoDto);

                foreach (var veiculoHistoricoDto in veiculoDto.VeiculoHistoricoDto)
                {
                    veiculoHistoricoDto.IdVeiculo = idVeiculo;
                    veiculoHistoricoBusiness.Incluir(veiculoHistoricoDto);
                }

                foreach (var veiculoMultaDto in veiculoDto.VeiculoMultaDto)
                {
                    veiculoMultaDto.IdVeiculo = idVeiculo;
                    veiculoMultaBusiness.Incluir(veiculoMultaDto);
                }

                scope.Complete();
            }

            return idVeiculo;
        }

        public void Alterar(VeiculoDto veiculoDto)
        {
            if (veiculoDto == null)
                throw new BusinessException("VeiculoDto é nulo");

            if (veiculoDto.Id <= 0)
                throw new BusinessException("IdVeiculo é nulo");

            var veiculoExistePorId = veiculoRepository.Obter(new VeiculoDto() { Id = veiculoDto.Id });

            if (veiculoExistePorId == null)
                throw new BusinessException($"Veiculo com Id {veiculoDto.Id} não está cadastrado");

            veiculoDto.Placa = veiculoDto.Placa?.Replace(".", "").Replace(",", "").Replace("-", "").Replace("/", "").Replace(" ", "").Replace("*", "").Replace("_", "").Trim();

            if (veiculoDto.Placa != veiculoExistePorId.Placa)
            {
                if (string.IsNullOrEmpty(veiculoDto.Placa))
                    throw new BusinessException("Placa é obrigatório");

                var veiculoExistePorPlaca = veiculoRepository.Existe(new VeiculoDto() { Placa = veiculoDto.Placa });

                if (veiculoExistePorPlaca)
                    throw new BusinessException($"Veiculo com Placa {veiculoDto.Placa} já está cadastrado");
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                veiculoRepository.Alterar(veiculoDto);

                // Veiculo historico
                veiculoHistoricoBusiness.Excluir(veiculoDto.Id);
                foreach (var veiculoHistoricoDto in veiculoDto.VeiculoHistoricoDto)
                    veiculoHistoricoBusiness.Incluir(veiculoHistoricoDto);

                // Veiculo multa
                veiculoMultaBusiness.Excluir(veiculoDto.Id);
                foreach (var veiculoMultaDto in veiculoDto.VeiculoMultaDto)
                    veiculoMultaBusiness.Incluir(veiculoMultaDto);

                scope.Complete();
            }
        }

        public void AlterarStatus(VeiculoDto veiculoDto)
        {
            if (veiculoDto == null)
                throw new BusinessException("VeiculoDto é nulo");

            if (veiculoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!veiculoDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeProduto = veiculoRepository.Existe(new VeiculoDto() { Id = veiculoDto.Id });

            if (!existeProduto)
                throw new BusinessException($"Veiculo Id ({veiculoDto.Id}) não está cadastrado");

            if (veiculoDto.Ativo.HasValue)
            {
                if (!veiculoDto.Ativo.Value)
                {
                    if (veiculoDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (veiculoDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            veiculoRepository.AlterarStatus(veiculoDto);
        }
    }
}
