using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoTransportadora.Business
{
    public class ParcelaBusiness : BaseBusiness
    {
        public List<ParcelaDto> Gerar(SimulacaoDto simulacaoDto)
        {
            var parcelasDto = new List<ParcelaDto>();

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

            double somaFatorInvertido = 0;
            DateTime dataVencimento;
            double diasContrato = 0;
            double diasParcela = 0;
            double fator = 0;
            double fatorInvertido = 0;

            for (int i = 1; i <= simulacaoDto.QuantidadeParcela; i++)
            {
                diasContrato = 0;
                diasParcela = 0;
                fator = 0;
                fatorInvertido = 0;
                var parcelaAnterior = i == 1 ? null : parcelasDto[i - 2];

                dataVencimento = i == 1 ? simulacaoDto.DataPrimeiraParcela : simulacaoDto.DataPrimeiraParcela.AddMonths(i - 1);
                diasContrato = Convert.ToDouble(dataVencimento.Subtract(simulacaoDto.DataInicio).Days);
                diasParcela = parcelaAnterior == null ? diasContrato : Convert.ToDouble(dataVencimento.Subtract(parcelaAnterior.DataVencimento).Days);

                fator = Math.Round(Math.Pow((1 + simulacaoDto.TaxaMensalJuros), (diasContrato / 30D)), 6);
                fatorInvertido = Math.Round(1 / fator, 6);
                somaFatorInvertido += fatorInvertido;

                parcelasDto.Add(new ParcelaDto()
                {
                    Id = i,
                    DataVencimento = dataVencimento,
                    DiasContrato = Convert.ToInt32(diasContrato),
                    DiasParcela = Convert.ToInt32(diasParcela),
                    Fator = fator,
                    FatorInvertido = fatorInvertido
                });
            }

            somaFatorInvertido = Math.Round(somaFatorInvertido, 6);
            double valorSaldoAtual = 0;
            double valorParcela = 0;
            double valorSaldoAnterior = 0;
            double valorJuros = 0;
            double valorAmortizacao = 0;

            for (int i = 0; i < parcelasDto.Count(); i++)
            {
                valorSaldoAtual = 0;
                valorParcela = 0;
                valorSaldoAnterior = 0;
                valorJuros = 0;
                valorAmortizacao = 0;
                var parcelaAnterior = i == 0 ? null : parcelasDto[i - 1];

                valorParcela = Math.Round(simulacaoDto.ValorFinanciado / somaFatorInvertido, 2);
                valorSaldoAnterior = i == 0 ? simulacaoDto.ValorFinanciado : parcelaAnterior.ValorSaldoAnterior - parcelaAnterior.ValorAmortizacao;
                valorJuros = i == simulacaoDto.QuantidadeParcela - 1 ? Math.Round(valorParcela - valorSaldoAnterior, 2) : Math.Round(valorSaldoAnterior * (Math.Pow((1 + simulacaoDto.TaxaMensalJuros), (Convert.ToDouble(parcelasDto[i].DiasParcela) / 30D)) - 1), 2);
                valorAmortizacao = i == simulacaoDto.QuantidadeParcela - 1 ? valorSaldoAnterior - valorAmortizacao : valorParcela - valorJuros;
                valorSaldoAtual = i == 0 ? simulacaoDto.ValorFinanciado - valorAmortizacao : valorSaldoAnterior - valorAmortizacao;

                parcelasDto[i].ValorParcela = valorParcela;
                parcelasDto[i].ValorSaldoAnterior = valorSaldoAnterior;                
                parcelasDto[i].ValorJuros = valorJuros;
                parcelasDto[i].ValorAmortizacao = valorAmortizacao;
                parcelasDto[i].ValorSaldoAtual = valorSaldoAtual;
            }

            return parcelasDto;
        }
    }
}
