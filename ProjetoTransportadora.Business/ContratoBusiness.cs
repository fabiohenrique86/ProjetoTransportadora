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

        public dynamic ListarGridParcela(ContratoDto contratoDto = null)
        {
            return contratoRepository.ListarGridParcela(contratoDto);
        }

        public dynamic ListarGrid(ContratoDto contratoDto = null)
        {
            return contratoRepository.ListarGrid(contratoDto);
        }

        public dynamic ListarSituacaoContratoResumo(ContratoDto contratoDto = null)
        {
            return contratoRepository.ListarSituacaoContratoResumo(contratoDto);
        }
        public dynamic ListarSituacaoParcelaResumo(ContratoDto contratoDto = null)
        {
            return contratoRepository.ListarSituacaoParcelaResumo(contratoDto);
        }

        public int ListarTotal(ContratoDto contratoDto = null)
        {
            return contratoRepository.ListarTotal(contratoDto);
        }

        public bool Existe(ContratoDto contratoDto)
        {
            return contratoRepository.Existe(contratoDto);
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

            if (contratoDto.ContratoParcelaDto == null || contratoDto.ContratoParcelaDto.Count <= 0)
                throw new BusinessException("Quantidade de parcelas do Contrato é obrigatório");

            if (contratoDto.IdTipoContrato <= 0)
                throw new BusinessException("Tipo de Cálculo é obrigatório");

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

        public void Alterar(ContratoDto contratoDto)
        {
            if (contratoDto == null)
                throw new BusinessException("ContratoDto é nulo");

            if (contratoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (contratoDto.DataContrato >= contratoDto.DataPrimeiraParcela)
                throw new BusinessException("Data do Contrato tem que menor do que a Data Primeira Parcela do contrato");

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                contratoRepository.Alterar(contratoDto);

                contratoHistoricoBusiness.Excluir(contratoDto.Id);
                foreach (var contratoHistoricoDto in contratoDto.ContratoHistoricoDto)
                    contratoHistoricoBusiness.Incluir(contratoHistoricoDto);

                contratoParcelaBusiness.Excluir(contratoDto.Id);
                foreach (var contratoparcelaDto in contratoDto.ContratoParcelaDto)
                    contratoParcelaBusiness.Incluir(contratoparcelaDto);

                scope.Complete();
            }
        }

        public List<ContratoParcelaDto> Antecipar(ContratoDto contratoDto)
        {
            if (contratoDto == null)
                throw new BusinessException("ContratoDto é nulo");

            if (contratoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!contratoDto.SimulacaoAntecipacao)
            {
                if (contratoDto.IdSituacaoContrato <= 0)
                    throw new BusinessException("Situação do Contrato é obrigatório");
            }

            if (contratoDto.DataAntecipacao.GetValueOrDefault() == DateTime.MinValue)
                throw new BusinessException("Data da Antecipação é obrigatório");

            if (!contratoDto.SimulacaoAntecipacao)
            {
                if (contratoDto.ValorAntecipacao <= 0)
                    throw new BusinessException("Valor da Antecipação é obrigatório");
            }

            if (!contratoDto.SimulacaoAntecipacao)
            {
                if (contratoDto.IdUsuarioAntecipacao <= 0)
                    throw new BusinessException("Id Usuário Antecipação é obrigatório");
            }

            var contrato = contratoRepository.Obter(new ContratoDto() { Id = contratoDto.Id });

            if (contrato == null)
                throw new BusinessException($"Contrato Id ({contratoDto.Id}) não existe");

            if (contratoDto.DataAntecipacao < contrato.DataContrato)
                throw new BusinessException("Data da Antecipação deve ser maior ou igual à Data do Contrato");

            if (contratoDto.DataAntecipacao.GetValueOrDefault().DayOfWeek == DayOfWeek.Saturday || contratoDto.DataAntecipacao.GetValueOrDefault().DayOfWeek == DayOfWeek.Sunday)
                throw new BusinessException("Data da Antecipação deve ser dia útil");

            var existeFeriado = feriadoBusiness.Existe(new FeriadoDto() { DataFeriado = contratoDto.DataAntecipacao.GetValueOrDefault() });

            if (existeFeriado)
                throw new BusinessException("Data da Antecipação deve ser dia útil");

            var listaContratoParcelaDto = contratoParcelaBusiness.Listar(new ContratoParcelaDto() { IdContrato = contratoDto.Id, ListaIdSituacaoParcela = new List<int>() { SituacaoParcelaDto.EnumSituacaoParcela.Pendente.GetHashCode(), SituacaoParcelaDto.EnumSituacaoParcela.BoletoEmitido.GetHashCode(), SituacaoParcelaDto.EnumSituacaoParcela.Atraso.GetHashCode() } });

            if (listaContratoParcelaDto == null || listaContratoParcelaDto.Count() <= 0)
                throw new BusinessException($"Não existem parcelas a serem antecipadas para o Contrato {contratoDto.Id}");

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                for (int i = 0; i < listaContratoParcelaDto.Count; i++)
                {
                    if (!contratoDto.SimulacaoAntecipacao)
                        listaContratoParcelaDto[i].IdSituacaoParcela = SituacaoParcelaDto.EnumSituacaoParcela.Antecipado.GetHashCode();

                    if (listaContratoParcelaDto[i].DataInicio >= contratoDto.DataAntecipacao) // parcela a vencer
                    {
                        listaContratoParcelaDto[i].ValorDescontoJuros = listaContratoParcelaDto[i].ValorJuros;
                        listaContratoParcelaDto[i].ValorParcela = listaContratoParcelaDto[i].ValorAmortizacao.GetValueOrDefault();
                        listaContratoParcelaDto[i].ValorMulta = 0;
                        listaContratoParcelaDto[i].ValorMora = 0;
                    }
                    else
                    {
                        if (listaContratoParcelaDto[i].DataVencimento < contratoDto.DataAntecipacao) // parcela vencida
                        {
                            double diasCorridos = contratoDto.DataAntecipacao.GetValueOrDefault().Subtract(listaContratoParcelaDto[i].DataVencimento).Days;

                            listaContratoParcelaDto[i].ValorMora = Math.Round(listaContratoParcelaDto[i].ValorOriginal * (contrato.TaxaMora.GetValueOrDefault() / 100) * (diasCorridos / 30D), 2);
                            listaContratoParcelaDto[i].ValorMulta = Math.Round(listaContratoParcelaDto[i].ValorOriginal * (contrato.TaxaMulta.GetValueOrDefault() / 100), 2);
                            listaContratoParcelaDto[i].ValorDescontoJuros = 0;
                            listaContratoParcelaDto[i].ValorParcela = Math.Round(listaContratoParcelaDto[i].ValorAmortizacao.GetValueOrDefault() +
                                                                                listaContratoParcelaDto[i].ValorJuros.GetValueOrDefault() +
                                                                                listaContratoParcelaDto[i].ValorMora.GetValueOrDefault() +
                                                                                listaContratoParcelaDto[i].ValorMulta.GetValueOrDefault() -
                                                                                listaContratoParcelaDto[i].ValorDescontoJuros.GetValueOrDefault() -
                                                                                listaContratoParcelaDto[i].ValorDescontoParcela.GetValueOrDefault(), 2);
                        }
                        else // parcela atual
                        {
                            double diasCorridos = contratoDto.DataAntecipacao.GetValueOrDefault().Subtract(listaContratoParcelaDto[i].DataInicio).Days;

                            listaContratoParcelaDto[i].ValorMora = 0;
                            listaContratoParcelaDto[i].ValorMulta = 0;
                            listaContratoParcelaDto[i].ValorDescontoJuros = listaContratoParcelaDto[i].ValorJuros - Math.Round(listaContratoParcelaDto[i].ValorAmortizacao.GetValueOrDefault() * (contrato.TaxaJuros / 100) * (diasCorridos / 30D), 2);
                            listaContratoParcelaDto[i].ValorParcela = Math.Round(listaContratoParcelaDto[i].ValorAmortizacao.GetValueOrDefault() +
                                                                                listaContratoParcelaDto[i].ValorJuros.GetValueOrDefault() +
                                                                                listaContratoParcelaDto[i].ValorMora.GetValueOrDefault() +
                                                                                listaContratoParcelaDto[i].ValorMulta.GetValueOrDefault() -
                                                                                listaContratoParcelaDto[i].ValorDescontoJuros.GetValueOrDefault() -
                                                                                listaContratoParcelaDto[i].ValorDescontoParcela.GetValueOrDefault(), 2);
                        }
                    }

                    if (!contratoDto.SimulacaoAntecipacao)
                        contratoParcelaBusiness.Antecipar(listaContratoParcelaDto[i]);
                }

                if (!contratoDto.SimulacaoAntecipacao)
                    contratoRepository.Antecipar(contratoDto);

                transactionScope.Complete();
            }

            return listaContratoParcelaDto;
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
                throw new BusinessException("Data da Baixa é obrigatório");

            if (contratoDto.IdUsuarioBaixa <= 0)
                throw new BusinessException("IdUsuarioBaixa é obrigatório");

            var contrato = contratoRepository.Obter(new ContratoDto() { Id = contratoDto.Id });

            if (contrato == null)
                throw new BusinessException($"Contrato Id ({contratoDto.Id}) não está cadastrado");

            if (contratoDto.DataBaixa < contrato.DataContrato)
                throw new BusinessException("Data da Baixa deve ser maior ou igual à Data do Contrato");

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
