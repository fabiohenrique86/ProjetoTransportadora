using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace ProjetoTransportadora.Business
{
    public class ContratoBusiness : BaseBusiness
    {
        ContratoRepository contratoRepository;
        ContratoHistoricoBusiness contratoHistoricoBusiness;
        ContratoParcelaBusiness contratoParcelaBusiness;
        public ContratoBusiness()
        {
            contratoRepository = new ContratoRepository();
            contratoHistoricoBusiness = new ContratoHistoricoBusiness();
            contratoParcelaBusiness = new ContratoParcelaBusiness();
        }

        public int ListarTotal(ContratoDto contratoDto = null)
        {
            return contratoRepository.ListarTotal(contratoDto);
        }

        public List<ContratoDto> Listar(ContratoDto contratoDto = null)
        {
            return contratoRepository.Listar(contratoDto);
        }

        public int Incluir(ContratoDto contratoDto)
        {
            var idContrato = 0;

            if (contratoDto == null)
                throw new BusinessException("ContratoDto é nulo");

            if (contratoDto.IdCliente <= 0)
                throw new BusinessException("Cliente é obrigatório");

            if (contratoDto.DataContrato == DateTime.MinValue)
                throw new BusinessException("Data do Contrato é obrigatório");

            if (contratoDto.IdProduto <= 0)
                throw new BusinessException("Produto é obrigatório");

            if (contratoDto.IdVeiculo <= 0)
                throw new BusinessException("Veículo é obrigatório");

            if (contratoDto.DataBaixa < contratoDto.DataContrato)
                throw new BusinessException("Data da Baixa deve ser maior ou igual a Data do Contrato");

            if (contratoDto.DataAntecipacao < contratoDto.DataContrato)
                throw new BusinessException("Data da DataAntecipação deve ser maior ou igual a Data do Contrato");

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                idContrato = contratoRepository.Incluir(contratoDto);

                foreach (var contratoHistoricoDto in contratoDto.ContratoHistoricoDto)
                {
                    contratoHistoricoDto.IdContrato = idContrato;
                    contratoHistoricoBusiness.Incluir(contratoHistoricoDto);
                }

                foreach (var contratoparcelaDto in contratoDto.ContratoParcelaDto)
                {
                    contratoparcelaDto.IdContrato = idContrato;
                    contratoParcelaBusiness.Incluir(contratoparcelaDto);
                }

                scope.Complete();
            }

            return idContrato;
        }

        //public void Alterar(ContratoDto ContratoDto)
        //{
        //    if (ContratoDto == null)
        //        throw new BusinessException("ContratoDto é nulo");

        //    if (ContratoDto.Id <= 0)
        //        throw new BusinessException("IdContrato é nulo");

        //    var ContratoExistePorId = contratoRepository.Obter(new ContratoDto() { Id = ContratoDto.Id });

        //    if (ContratoExistePorId == null)
        //        throw new BusinessException($"Contrato com Id {ContratoDto.Id} não está cadastrado");

        //    ContratoDto.Placa = ContratoDto.Placa?.Replace(".", "").Replace(",", "").Replace("-", "").Replace("/", "").Replace(" ", "").Replace("*", "").Replace("_", "").Trim();

        //    if (ContratoDto.Placa != ContratoExistePorId.Placa)
        //    {
        //        if (string.IsNullOrEmpty(ContratoDto.Placa))
        //            throw new BusinessException("Placa é obrigatório");

        //        var ContratoExistePorPlaca = contratoRepository.Existe(new ContratoDto() { Placa = ContratoDto.Placa });

        //        if (ContratoExistePorPlaca)
        //            throw new BusinessException($"Contrato com Placa {ContratoDto.Placa} já está cadastrado");
        //    }

        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
        //    {
        //        contratoRepository.Alterar(ContratoDto);

        //        // Contrato historico
        //        contratoHistoricoBusiness.Excluir(ContratoDto.Id);
        //        foreach (var ContratoHistoricoDto in ContratoDto.ContratoHistoricoDto)
        //            contratoHistoricoBusiness.Incluir(ContratoHistoricoDto);

        //        // Contrato multa
        //        ContratoMultaBusiness.Excluir(ContratoDto.Id);
        //        foreach (var ContratoMultaDto in ContratoDto.ContratoMultaDto)
        //            ContratoMultaBusiness.Incluir(ContratoMultaDto);

        //        scope.Complete();
        //    }
        //}

        //public void AlterarStatus(ContratoDto ContratoDto)
        //{
        //    if (ContratoDto == null)
        //        throw new BusinessException("ContratoDto é nulo");

        //    if (ContratoDto.Id <= 0)
        //        throw new BusinessException("Id é obrigatório");

        //    if (!ContratoDto.Ativo.HasValue)
        //        throw new BusinessException("Ativo é obrigatório");

        //    var existeProduto = contratoRepository.Existe(new ContratoDto() { Id = ContratoDto.Id });

        //    if (!existeProduto)
        //        throw new BusinessException($"Contrato Id ({ContratoDto.Id}) não está cadastrado");

        //    if (ContratoDto.Ativo.HasValue)
        //    {
        //        if (!ContratoDto.Ativo.Value)
        //        {
        //            if (ContratoDto.IdUsuarioInativacao <= 0)
        //                throw new BusinessException("IdUsuarioInativação é obrigatório");

        //            if (ContratoDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
        //                throw new BusinessException("Data Inativação é obrigatório");
        //        }
        //    }

        //    contratoRepository.AlterarStatus(ContratoDto);
        //}
    }
}
