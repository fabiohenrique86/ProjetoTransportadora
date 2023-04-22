using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;

namespace ProjetoTransportadora.Business
{
    public class VeiculoHistoricoBusiness
    {
        VeiculoHistoricoRepository veiculoHistoricoRepository;
        public VeiculoHistoricoBusiness()
        {
            veiculoHistoricoRepository = new VeiculoHistoricoRepository();
        }

        public int Incluir(VeiculoHistoricoDto veiculoHistoricoDto)
        {
            var idVeiculoHistorico = 0;

            if (veiculoHistoricoDto == null)
                throw new BusinessException("VeiculoHistoricoDto é nulo");

            if (veiculoHistoricoDto.IdVeiculo <= 0)
                throw new BusinessException("IdVeiculo é obrigatório");

            if (veiculoHistoricoDto.DataHistorico == DateTime.MinValue)
                throw new BusinessException("Data Histórico é obrigatório");

            if (string.IsNullOrEmpty(veiculoHistoricoDto.Descricao))
                throw new BusinessException("Descrição Histórico é obrigatório");

            idVeiculoHistorico = veiculoHistoricoRepository.Incluir(veiculoHistoricoDto);

            return idVeiculoHistorico;
        }

        public void Excluir(int idVeiculo)
        {
            if (idVeiculo <= 0)
                throw new BusinessException("idVeiculo é obrigatório");

            veiculoHistoricoRepository.Excluir(idVeiculo);
        }
    }
}
