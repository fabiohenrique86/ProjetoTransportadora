using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;

namespace ProjetoTransportadora.Business
{
    public class VeiculoMultaBusiness
    {
        VeiculoMultaRepository veiculoMultaRepository;
        public VeiculoMultaBusiness()
        {
            veiculoMultaRepository = new VeiculoMultaRepository();
        }

        public int Incluir(VeiculoMultaDto veiculoMultaDto)
        {
            var idVeiculoMulta = 0;

            if (veiculoMultaDto == null)
                throw new BusinessException("VeiculoMultaDto é nulo");

            if (veiculoMultaDto.IdVeiculo <= 0)
                throw new BusinessException("IdVeiculo é obrigatório");

            if (veiculoMultaDto.DataMulta == DateTime.MinValue)
                throw new BusinessException("Data Multa é obrigatório");

            if (string.IsNullOrEmpty(veiculoMultaDto.Local))
                throw new BusinessException("Local Multa é obrigatório");

            if (veiculoMultaDto.IdCondutor <= 0)
                throw new BusinessException("Condutor Multa é obrigatório");

            if (veiculoMultaDto.DataVencimentoMulta == DateTime.MinValue)
                throw new BusinessException("Data Vencimento Multa é obrigatório");

            if (veiculoMultaDto.ValorMulta < 0)
                throw new BusinessException("Valor Multa é obrigatório");

            if (veiculoMultaDto.IdSituacaoMulta <= 0)
                throw new BusinessException("Situação Multa é obrigatório");

            idVeiculoMulta = veiculoMultaRepository.Incluir(veiculoMultaDto);

            return idVeiculoMulta;
        }

        public void Excluir(int idVeiculo)
        {
            if (idVeiculo <= 0)
                throw new BusinessException("idVeiculo é obrigatório");

            veiculoMultaRepository.Excluir(idVeiculo);
        }
    }
}
