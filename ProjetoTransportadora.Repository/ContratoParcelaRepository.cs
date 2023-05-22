using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
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

        public bool Existe(ContratoParcelaDto contratoParcelaDto)
        {
            IQueryable<ContratoParcela> query = projetoTransportadoraEntities.ContratoParcela;

            if (contratoParcelaDto.Id > 0)
                query = query.Where(x => x.Id == contratoParcelaDto.Id);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<ContratoParcelaDto> Listar(ContratoParcelaDto contratoParcelaDto)
        {
            IQueryable<ContratoParcela> query = projetoTransportadoraEntities.ContratoParcela;

            if (contratoParcelaDto.Id > 0)
                query = query.Where(x => x.Id == contratoParcelaDto.Id);

            if (contratoParcelaDto.IdContrato > 0)
                query = query.Where(x => x.IdContrato == contratoParcelaDto.IdContrato);

            if (contratoParcelaDto.ListaIdSituacaoParcela != null && contratoParcelaDto.ListaIdSituacaoParcela.Count() > 0)
                query = query.Where(x => contratoParcelaDto.ListaIdSituacaoParcela.Contains(x.IdSituacaoParcela));

            return query.Select(x => new ContratoParcelaDto()
            {
                Id = x.Id,
                NumeroParcela = x.NumeroParcela,
                IdContrato = x.IdContrato,
                IdSituacaoParcela = x.IdSituacaoParcela,
                DataVencimento = x.DataVencimento,
                DiasParcela = x.DiasParcela,
                DiasContrato = x.DiasContrato,
                DataPagamento = x.DataPagamento,
                DataEmissao = x.DataEmissao,
                ValorOriginal = x.ValorOriginal,
                ValorAmortizacao = x.ValorAmortizacao,
                ValorJuros = x.ValorJuros,
                ValorMulta = x.ValorMulta,
                ValorMora = x.ValorMora,
                ValorDesconto = x.ValorDesconto,
                ValorParcela = x.ValorParcela
            }).OrderBy(x => x.NumeroParcela).ToList();
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

        public void Antecipar(ContratoParcelaDto contratoParcelaDto)
        {
            var contratoParcela = projetoTransportadoraEntities.ContratoParcela.FirstOrDefault(x => x.Id == contratoParcelaDto.Id);

            contratoParcela.IdSituacaoParcela = contratoParcelaDto.IdSituacaoParcela;
            contratoParcela.ValorJuros = contratoParcelaDto.ValorJuros;
            contratoParcela.ValorParcela = contratoParcelaDto.ValorParcela;

            contratoParcela.ValorMulta = contratoParcelaDto.ValorMulta;
            contratoParcela.ValorMora = contratoParcelaDto.ValorMora;
            contratoParcela.ValorDesconto = contratoParcelaDto.ValorDesconto;


            projetoTransportadoraEntities.Entry(contratoParcela).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }

        public void Baixar(ContratoParcelaDto contratoParcelaDto)
        {
            var contratoParcela = projetoTransportadoraEntities.ContratoParcela.FirstOrDefault(x => x.Id == contratoParcelaDto.Id);

            contratoParcela.IdSituacaoParcela = contratoParcelaDto.IdSituacaoParcela;

            projetoTransportadoraEntities.Entry(contratoParcela).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }

        public void Excluir(int idContrato)
        {
            var contratoParcela = projetoTransportadoraEntities.ContratoParcela.Where(x => x.IdContrato == idContrato);

            projetoTransportadoraEntities.ContratoParcela.RemoveRange(contratoParcela);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
