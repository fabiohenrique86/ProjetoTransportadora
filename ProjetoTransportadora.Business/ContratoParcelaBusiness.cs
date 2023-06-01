using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoTransportadora.Business
{
    public class ContratoParcelaBusiness : BaseBusiness
    {
        FeriadoBusiness feriadoBusiness;
        ContratoParcelaRepository contratoParcelaRepository;

        public ContratoParcelaBusiness()
        {
            feriadoBusiness = new FeriadoBusiness();
            contratoParcelaRepository = new ContratoParcelaRepository();
        }

        public List<ContratoParcelaDto> Listar(ContratoParcelaDto contratoParcelaDto)
        {
            return contratoParcelaRepository.Listar(contratoParcelaDto);
        }

        public void Antecipar(ContratoParcelaDto contratoParcelaDto)
        {
            if (contratoParcelaDto == null)
                throw new BusinessException("contratoParcelaDto é nulo");

            if (contratoParcelaDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeContratoParcela = contratoParcelaRepository.Existe(new ContratoParcelaDto() { Id = contratoParcelaDto.Id });

            if (!existeContratoParcela)
                throw new BusinessException($"ContratoParcelaDto Id ({contratoParcelaDto.Id}) não está cadastrado");

            contratoParcelaRepository.Antecipar(contratoParcelaDto);
        }

        public void Baixar(ContratoParcelaDto contratoParcelaDto)
        {
            if (contratoParcelaDto == null)
                throw new BusinessException("contratoParcelaDto é nulo");

            if (contratoParcelaDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeContratoParcela = contratoParcelaRepository.Existe(new ContratoParcelaDto() { Id = contratoParcelaDto.Id });

            if (!existeContratoParcela)
                throw new BusinessException($"ContratoParcelaDto Id ({contratoParcelaDto.Id}) não está cadastrado");

            contratoParcelaRepository.Baixar(contratoParcelaDto);
        }

        public int Incluir(ContratoParcelaDto contratoParcelaDto)
        {
            var idContratoParcela = 0;

            if (contratoParcelaDto == null)
                throw new BusinessException("contratoParcelaDto é nulo");

            if (contratoParcelaDto.NumeroParcela <= 0)
                throw new BusinessException("Número Parcela é obrigatório");

            if (contratoParcelaDto.IdContrato <= 0)
                throw new BusinessException("Contrato é obrigatório");

            if (contratoParcelaDto.IdSituacaoParcela <= 0)
                throw new BusinessException("IdSituacaoParcela é obrigatório");

            if (contratoParcelaDto.DataInicio == DateTime.MinValue)
                throw new BusinessException("Data Início é obrigatório");

            if (contratoParcelaDto.DataVencimento == DateTime.MinValue)
                throw new BusinessException("Data Vencimento é obrigatório");

            if (contratoParcelaDto.ValorParcela <= 0)
                throw new BusinessException("Valor Parcela é obrigatório");

            if (contratoParcelaDto.ValorOriginal <= 0)
                throw new BusinessException("Valor Original é obrigatório");

            idContratoParcela = contratoParcelaRepository.Incluir(contratoParcelaDto);

            return idContratoParcela;
        }

        public List<ContratoParcelaDto> Gerar(SimulacaoDto simulacaoDto)
        {
            var parcelasDto = new List<ContratoParcelaDto>();

            if (simulacaoDto == null)
                throw new BusinessException("simulacaoDto é obrigatório");

            if (simulacaoDto.DataInicio == DateTime.MinValue)
                throw new BusinessException("Data de Início é obrigatório ou é inválida");

            if (simulacaoDto.DataPrimeiraParcela == DateTime.MinValue)
                throw new BusinessException("Data da Primeira Parcela é obrigatório ou é inválida");

            if (simulacaoDto.ValorFinanciado <= 0)
                throw new BusinessException("Valor Financiado é obrigatório");

            if (simulacaoDto.QuantidadeParcela <= 0)
                throw new BusinessException("Quantidade de Parcelas é obrigatório");

            if (simulacaoDto.TaxaMensalJuros <= 0)
                throw new BusinessException("Taxa Mensal Juros é obrigatório");

            if (simulacaoDto.DataPrimeiraParcela < simulacaoDto.DataInicio)
                throw new BusinessException("Data Primeira Parcela deve ser posterior a Data de Início.");

            if (simulacaoDto.DataInicio.DayOfWeek == DayOfWeek.Saturday || simulacaoDto.DataInicio.DayOfWeek == DayOfWeek.Sunday)
                throw new BusinessException("Data de Início deve ser dia útil");

            DateTime dataVencimento;
            double diasContrato = 0;
            double diasParcela = 0;
            double fator = 0;
            double fatorInvertido = 0;
            double valorSaldoAtual = 0;
            double valorParcela = 0;
            double valorSaldoAnterior = 0;
            double valorJuros = 0;
            double valorAmortizacao = 0;
            double somaFatorInvertido = 0;

            for (int i = 1; i <= simulacaoDto.QuantidadeParcela; i++)
            {
                diasContrato = 0;
                diasParcela = 0;
                fator = 0;
                fatorInvertido = 0;
                var parcelaAnterior = i == 1 ? null : parcelasDto[i - 2];

                dataVencimento = this.CalcularDataVencimento(i == 1 ? simulacaoDto.DataPrimeiraParcela : simulacaoDto.DataPrimeiraParcela.AddMonths(i - 1));
                diasContrato = Convert.ToDouble(dataVencimento.Subtract(simulacaoDto.DataInicio).Days);
                diasParcela = parcelaAnterior == null ? diasContrato : Convert.ToDouble(dataVencimento.Subtract(parcelaAnterior.DataVencimento).Days);

                fator = Math.Pow((1 + (simulacaoDto.TaxaMensalJuros / 100)), (diasContrato / 30D));
                fatorInvertido = 1 / fator;
                somaFatorInvertido += fatorInvertido;

                parcelasDto.Add(new ContratoParcelaDto()
                {
                    NumeroParcela = i,
                    DataInicio = (i == 1 ? simulacaoDto.DataInicio : parcelaAnterior.DataVencimento),
                    DataVencimento = dataVencimento,                    
                    DataEmissao = DateTime.UtcNow.ToLocalTime(),
                    DiasContrato = Convert.ToInt32(diasContrato),
                    DiasParcela = Convert.ToInt32(diasParcela),
                    Fator = fator,
                    FatorInvertido = fatorInvertido
                });
            }

            //somaFatorInvertido = Math.Round(somaFatorInvertido, 6);

            for (int i = 0; i < parcelasDto.Count(); i++)
            {
                valorSaldoAtual = 0;
                valorParcela = 0;
                valorSaldoAnterior = 0;
                valorJuros = 0;
                valorAmortizacao = 0;
                var parcelaAnterior = i == 0 ? null : parcelasDto[i - 1];

                valorParcela = Math.Round(simulacaoDto.ValorFinanciado / somaFatorInvertido, 2);
                valorSaldoAnterior = i == 0 ? simulacaoDto.ValorFinanciado : parcelaAnterior.ValorSaldoAnterior - parcelaAnterior.ValorAmortizacao.GetValueOrDefault();
                valorJuros = i == simulacaoDto.QuantidadeParcela - 1 ? Math.Round(valorParcela - valorSaldoAnterior, 2) : Math.Round(valorSaldoAnterior * (Math.Pow((1 + (simulacaoDto.TaxaMensalJuros / 100)), (Convert.ToDouble(parcelasDto[i].DiasParcela) / 30D)) - 1), 2);
                valorAmortizacao = i == simulacaoDto.QuantidadeParcela - 1 ? valorSaldoAnterior - valorAmortizacao : valorParcela - valorJuros;
                valorSaldoAtual = i == 0 ? simulacaoDto.ValorFinanciado - valorAmortizacao : valorSaldoAnterior - valorAmortizacao;

                parcelasDto[i].ValorParcela = valorParcela;
                parcelasDto[i].ValorSaldoAnterior = valorSaldoAnterior;
                parcelasDto[i].ValorJuros = valorJuros;
                parcelasDto[i].ValorAmortizacao = valorAmortizacao;
                parcelasDto[i].ValorSaldoAtual = valorSaldoAtual;

                parcelasDto[i].ValorOriginal = valorParcela;
                parcelasDto[i].ValorMora = 0;
                parcelasDto[i].ValorDesconto = 0;
                parcelasDto[i].ValorMulta = 0;
                parcelasDto[i].IdSituacaoParcela = SituacaoParcelaDto.EnumSituacaoParcela.Pendente.GetHashCode();
                parcelasDto[i].SituacaoParcelaDto = new SituacaoParcelaDto() { Id = SituacaoParcelaDto.EnumSituacaoParcela.Pendente.GetHashCode(), Nome = SituacaoParcelaDto.EnumSituacaoParcela.Pendente.ToString() };
            }

            return parcelasDto;
        }

        private DateTime CalcularDataVencimento(DateTime dataVencimentoOriginal)
        {
            var dataVencimentoCalculada = dataVencimentoOriginal;
            var existeFeriadoOuFimDeSemana = false;

            if (dataVencimentoOriginal.DayOfWeek == DayOfWeek.Saturday)
                dataVencimentoCalculada = dataVencimentoOriginal.AddDays(2);
            else if (dataVencimentoOriginal.DayOfWeek == DayOfWeek.Sunday)
                dataVencimentoCalculada = dataVencimentoOriginal.AddDays(1);

            do
            {
                // verifica se existe feriado para a nova data de vencimento
                existeFeriadoOuFimDeSemana = feriadoBusiness.Existe(new FeriadoDto() { DataFeriado = dataVencimentoCalculada });

                if (existeFeriadoOuFimDeSemana)
                    dataVencimentoCalculada = dataVencimentoCalculada.AddDays(1); // se existe feriado, adiciona 1 dia

                if (dataVencimentoCalculada.DayOfWeek == DayOfWeek.Saturday)
                {
                    existeFeriadoOuFimDeSemana = true;
                    dataVencimentoCalculada = dataVencimentoCalculada.AddDays(2); // data de vencimento deve ser dia útil                    
                }
                else if (dataVencimentoCalculada.DayOfWeek == DayOfWeek.Sunday)
                {
                    existeFeriadoOuFimDeSemana = true;
                    dataVencimentoCalculada = dataVencimentoCalculada.AddDays(1); // data de vencimento deve ser dia útil
                }
            }
            while (existeFeriadoOuFimDeSemana);

            if (dataVencimentoOriginal.Month != dataVencimentoCalculada.Month)
            {
                // obtém último dia útil do vencimento original
                dataVencimentoCalculada = new DateTime(dataVencimentoOriginal.Year, dataVencimentoOriginal.Month, DateTime.DaysInMonth(dataVencimentoOriginal.Year, dataVencimentoOriginal.Month));

                // nova data de vencimento deve ser dia útil
                if (dataVencimentoCalculada.DayOfWeek == DayOfWeek.Saturday)
                    dataVencimentoCalculada = dataVencimentoCalculada.AddDays(-1);
                else if (dataVencimentoCalculada.DayOfWeek == DayOfWeek.Sunday)
                    dataVencimentoCalculada = dataVencimentoCalculada.AddDays(-2);

                do
                {
                    // verifica se existe feriado para a nova data de vencimento
                    existeFeriadoOuFimDeSemana = feriadoBusiness.Existe(new FeriadoDto() { DataFeriado = dataVencimentoCalculada });

                    if (existeFeriadoOuFimDeSemana)
                        dataVencimentoCalculada = dataVencimentoCalculada.AddDays(-1); // se existe feriado, substrai 1 dia

                    if (dataVencimentoCalculada.DayOfWeek == DayOfWeek.Saturday)
                    {
                        dataVencimentoCalculada = dataVencimentoCalculada.AddDays(-2); // data de vencimento deve ser dia útil
                        existeFeriadoOuFimDeSemana = true;
                    }
                    else if (dataVencimentoCalculada.DayOfWeek == DayOfWeek.Sunday)
                    {
                        existeFeriadoOuFimDeSemana = true;
                        dataVencimentoCalculada = dataVencimentoCalculada.AddDays(-1); // data de vencimento deve ser dia útil
                    }
                }
                while (existeFeriadoOuFimDeSemana);
            }

            return dataVencimentoCalculada;
        }
    }
}
