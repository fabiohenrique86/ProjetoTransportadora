using ProjetoTransportadora.Dto;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class ContratoParcelaRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public ContratoParcelaRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public int Incluir(ContratoParcelaDto contratoParcelaDto)
        {
            var contratoParcela = new ContratoParcela()
            {
                NumeroParcela = contratoParcelaDto.NumeroParcela,
                IdContrato = contratoParcelaDto.IdContrato,
                IdSituacaoParcela = contratoParcelaDto.IdSituacaoParcela,
                DataVencimento = contratoParcelaDto.DataVencimento,
                DiasParcela = contratoParcelaDto.DiasParcela,
                DiasContrato = contratoParcelaDto.DiasContrato,
                DataPagamento = contratoParcelaDto.DataPagamento,
                DataEmissao = contratoParcelaDto.DataEmissao,
                ValorOriginal = contratoParcelaDto.ValorOriginal,
                ValorAmortizacao = contratoParcelaDto.ValorAmortizacao,
                ValorJuros = contratoParcelaDto.ValorJuros,
                ValorMulta = contratoParcelaDto.ValorMulta,
                ValorMora = contratoParcelaDto.ValorMora,
                ValorDesconto = contratoParcelaDto.ValorDesconto,
                ValorParcela = contratoParcelaDto.ValorParcela
            };

            projetoTransportadoraEntities.ContratoParcela.Add(contratoParcela);
            projetoTransportadoraEntities.SaveChanges();

            return contratoParcela.Id;
        }

        public void Excluir(int idContrato)
        {
            var ContratoParcela = projetoTransportadoraEntities.ContratoParcela.Where(x => x.IdContrato == idContrato);

            projetoTransportadoraEntities.ContratoParcela.RemoveRange(ContratoParcela);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
