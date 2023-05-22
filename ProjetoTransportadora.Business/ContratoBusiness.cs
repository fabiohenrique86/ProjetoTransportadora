using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace ProjetoTransportadora.Business
{
    public class ContratoBusiness : BaseBusiness
    {
        ContratoRepository contratoRepository;
        ContratoHistoricoBusiness contratoHistoricoBusiness;
        ContratoParcelaBusiness contratoParcelaBusiness;
        FeriadoBusiness feriadoBusiness;
        public ContratoBusiness()
        {
            contratoRepository = new ContratoRepository();
            contratoHistoricoBusiness = new ContratoHistoricoBusiness();
            contratoParcelaBusiness = new ContratoParcelaBusiness();
            feriadoBusiness = new FeriadoBusiness();
        }

        public object ListarSituacaoContratoResumo(List<ContratoDto> contratoDto)
        {
            return contratoRepository.ListarSituacaoContratoResumo(contratoDto);
        }
        public object ListarSituacaoParcelaResumo(List<ContratoDto> contratoDto)
        {
            return contratoRepository.ListarSituacaoParcelaResumo(contratoDto);
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

            if (contratoDto.ContratoParcelaDto == null || contratoDto.ContratoParcelaDto.Count <= 0)
                throw new BusinessException("Parcela do Contrato é obrigatório");

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

        public void Antecipar(ContratoDto contratoDto)
        {
            if (contratoDto == null)
                throw new BusinessException("ContratoDto é nulo");

            if (contratoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (contratoDto.IdSituacaoContrato <= 0)
                throw new BusinessException("Situação do Contrato é obrigatório");

            if (contratoDto.DataAntecipacao.GetValueOrDefault() == DateTime.MinValue)
                throw new BusinessException("Data Antecipação é obrigatório");

            if (contratoDto.IdUsuarioAntecipacao <= 0)
                throw new BusinessException("IdUsuarioAntecipação é obrigatório");

            var contrato = contratoRepository.Obter(new ContratoDto() { Id = contratoDto.Id });

            if (contrato == null)
                throw new BusinessException($"Contrato Id ({contratoDto.Id}) não está cadastrado");

            if (contratoDto.DataAntecipacao < contrato.DataContrato)
                throw new BusinessException("Data Antecipação deve ser maior ou igual à Data do Contrato");

            if (contratoDto.DataAntecipacao.GetValueOrDefault().DayOfWeek == DayOfWeek.Saturday || contratoDto.DataAntecipacao.GetValueOrDefault().DayOfWeek == DayOfWeek.Sunday)
                throw new BusinessException("Data Antecipação deve ser dia útil");

            var existeFeriado = feriadoBusiness.Existe(new FeriadoDto() { DataFeriado = contratoDto.DataAntecipacao.GetValueOrDefault() });

            if (existeFeriado)
                throw new BusinessException("Data Antecipação deve ser dia útil");

            var listaContratoParcelaDto = contratoParcelaBusiness.Listar(new ContratoParcelaDto() { IdContrato = contratoDto.Id, ListaIdSituacaoParcela = new List<int>() { SituacaoParcelaDto.EnumSituacaoParcela.Pendente.GetHashCode(), SituacaoParcelaDto.EnumSituacaoParcela.BoletoEmitido.GetHashCode() } });

            if (listaContratoParcelaDto == null || listaContratoParcelaDto.Count() <= 0)
                throw new BusinessException($"Não existem parcelas a serem antecipadas para o Contrato {contratoDto.Id}");

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                foreach (var contratoParcelaDto in listaContratoParcelaDto)
                {
                    contratoParcelaDto.ValorMulta = 0;
                    contratoParcelaDto.ValorMora = 0;

                    if (contratoParcelaDto.DataVencimento >= contratoDto.DataAntecipacao)
                    {
                        contratoParcelaDto.ValorDesconto = contratoParcelaDto.ValorJuros;
                        contratoParcelaDto.ValorParcela = contratoParcelaDto.ValorAmortizacao.GetValueOrDefault();
                    }
                    else
                    {
                        double diasCorridos = contratoDto.DataAntecipacao.GetValueOrDefault().Subtract(contratoParcelaDto.DataVencimento).Days;

                        var fatorCalculado = Math.Round(Math.Pow((1 + (contrato.TaxaJuros / 100)), (diasCorridos / 30D)), 6);
                        
                        contratoParcelaDto.ValorJuros = (fatorCalculado - 1) * 1; // ??

                        contratoParcelaDto.ValorDesconto = contratoParcelaDto.ValorJuros - 0; // ??
                        contratoParcelaDto.ValorParcela = contratoParcelaDto.ValorAmortizacao.GetValueOrDefault() + contratoParcelaDto.ValorJuros.GetValueOrDefault() - contratoParcelaDto.ValorDesconto.GetValueOrDefault();                        
                    }

                    contratoParcelaBusiness.Antecipar(contratoParcelaDto);
                }

                contratoRepository.Antecipar(contratoDto);

                transactionScope.Complete();
            }
        }

        public void Baixar(ContratoDto contratoDto)
        {
            if (contratoDto == null)
                throw new BusinessException("ContratoDto é nulo");

            if (contratoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (contratoDto.IdSituacaoContrato <= 0)
                throw new BusinessException("Situação do Contrato é obrigatório");

            if (contratoDto.DataBaixa.GetValueOrDefault() == DateTime.MinValue)
                throw new BusinessException("Data Baixa é obrigatório");

            if (contratoDto.IdUsuarioBaixa <= 0)
                throw new BusinessException("IdUsuarioBaixa é obrigatório");

            var contrato = contratoRepository.Obter(new ContratoDto() { Id = contratoDto.Id });

            if (contrato == null)
                throw new BusinessException($"Contrato Id ({contratoDto.Id}) não está cadastrado");

            if (contratoDto.DataBaixa < contrato.DataContrato)
                throw new BusinessException("Data Baixa deve ser maior ou igual à Data do Contrato");

            var listaContratoParcelaDto = contratoParcelaBusiness.Listar(new ContratoParcelaDto() { IdContrato = contratoDto.Id, ListaIdSituacaoParcela = new List<int>() { SituacaoParcelaDto.EnumSituacaoParcela.Pendente.GetHashCode(), SituacaoParcelaDto.EnumSituacaoParcela.BoletoEmitido.GetHashCode(), SituacaoParcelaDto.EnumSituacaoParcela.Atraso.GetHashCode() } });

            if (listaContratoParcelaDto == null || listaContratoParcelaDto.Count() <= 0)
                throw new BusinessException($"Não existem parcelas a serem baixadas para o Contrato {contratoDto.Id}");

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                foreach (var contratoParcelaDto in listaContratoParcelaDto)
                {
                    contratoParcelaDto.IdSituacaoParcela = SituacaoParcelaDto.EnumSituacaoParcela.Baixado.GetHashCode();
                    contratoParcelaBusiness.Baixar(contratoParcelaDto);
                }

                contratoRepository.Baixar(contratoDto);

                transactionScope.Complete();
            }
        }
    }
}
