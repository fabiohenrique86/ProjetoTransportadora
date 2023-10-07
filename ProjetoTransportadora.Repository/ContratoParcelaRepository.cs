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

        public bool Existe(int idContratoParcela)
        {
            IQueryable<ContratoParcela> query = projetoTransportadoraEntities.ContratoParcela;

            if (idContratoParcela > 0)
                query = query.Where(x => x.Id == idContratoParcela);

            return query.Count() > 0 ? true : false;
        }

        public ContratoParcelaDto Obter(ContratoParcelaDto contratoParcelaDto)
        {
            IQueryable<ContratoParcela> query = projetoTransportadoraEntities.ContratoParcela;

            if (contratoParcelaDto.Id > 0)
                query = query.Where(x => x.Id == contratoParcelaDto.Id);

            return query.Select(x => new ContratoParcelaDto()
            {
                Id = x.Id,
                NumeroParcela = x.NumeroParcela,
                IdContrato = x.IdContrato,
                IdSituacaoParcela = x.IdSituacaoParcela,
                DataInicio = x.DataInicio,
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
                ValorDescontoJuros = x.ValorDescontoJuros,
                ValorDescontoParcela = x.ValorDescontoParcela,
                ValorParcela = x.ValorParcela,
                TaxaMora = x.TaxaMora,
                TaxaMulta = x.TaxaMulta,
                ValorAcrescimo = x.ValorAcrescimo,
                ValorResiduo = x.ValorResiduo,
            }).FirstOrDefault();
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

            var lista = query.Select(x => new ContratoParcelaDto()
            {
                Id = x.Id,
                NumeroParcela = x.NumeroParcela,
                IdContrato = x.IdContrato,
                IdSituacaoParcela = x.IdSituacaoParcela,
                DataInicio = x.DataInicio,
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
                ValorDescontoJuros = x.ValorDescontoJuros,
                ValorDescontoParcela = x.ValorDescontoParcela,
                ValorParcela = x.ValorParcela,
                TaxaMora = x.TaxaMora,
                TaxaMulta = x.TaxaMulta,
                ValorAcrescimo = x.ValorAcrescimo,
                ValorResiduo = x.ValorResiduo,
                ContratoParcelaHistoricoDto = x.ContratoParcelaHistorico.Select(w => new ContratoParcelaHistoricoDto()
                {
                    Id = w.Id,
                    DataCadastro = w.DataCadastro,
                    DataHistorico = w.DataHistorico,
                    Descricao = w.Descricao,
                    IdContratoParcela = w.IdContratoParcela,
                    IdUsuarioCadastro = w.IdUsuarioCadastro
                }).ToList()
            }).OrderBy(x => x.NumeroParcela).ToList();

            foreach (var cp in lista)
            {
                var listaContratoParcela = projetoTransportadoraEntities.Contrato.FirstOrDefault(x => x.ContratoParcela.Any(y => y.Id == cp.Id)).ContratoParcela.OrderBy(x => x.NumeroParcela);

                cp.UltimaParcela = listaContratoParcela.LastOrDefault().Id == cp.Id ? true : false;

                if (cp.UltimaParcela)
                {
                    foreach (var cpr in listaContratoParcela.Where(x => x.ValorResiduo > 0).OrderBy(x => x.DataVencimento))
                    {
                        cp.ContratoParcelaResiduoDto.Add(new ContratoParcelaResiduoDto()
                        {
                            NumeroParcela = cpr.NumeroParcela,
                            DataVencimento = cpr.DataVencimento,
                            ValorResiduo = cpr.ValorResiduo
                        });
                    }

                    cp.ValorResiduo = listaContratoParcela.Sum(x => x.ValorResiduo);
                }
            }

            return lista;
        }

        public int Incluir(ContratoParcelaDto contratoParcelaDto)
        {
            var contratoParcela = new ContratoParcela()
            {
                NumeroParcela = contratoParcelaDto.NumeroParcela,
                IdContrato = contratoParcelaDto.IdContrato,
                IdSituacaoParcela = contratoParcelaDto.IdSituacaoParcela,
                DataInicio = contratoParcelaDto.DataInicio,
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
                ValorDescontoJuros = contratoParcelaDto.ValorDescontoJuros,
                ValorDescontoParcela = contratoParcelaDto.ValorDescontoParcela,
                ValorParcela = contratoParcelaDto.ValorParcela,
                TaxaMulta = contratoParcelaDto.TaxaMulta,
                TaxaMora = contratoParcelaDto.TaxaMora,
                ValorAcrescimo = contratoParcelaDto.ValorAcrescimo,
                ValorResiduo = contratoParcelaDto.ValorResiduo
            };

            projetoTransportadoraEntities.ContratoParcela.Add(contratoParcela);
            projetoTransportadoraEntities.SaveChanges();

            return contratoParcela.Id;
        }

        public void Alterar(ContratoParcelaDto contratoParcelaDto)
        {
            var contratoParcela = projetoTransportadoraEntities.ContratoParcela.FirstOrDefault(x => x.Id == contratoParcelaDto.Id);

            contratoParcela.DataPagamento = contratoParcelaDto.DataPagamento;
            contratoParcela.TaxaMora = contratoParcelaDto.TaxaMora;
            contratoParcela.TaxaMulta = contratoParcelaDto.TaxaMulta;
            contratoParcela.ValorResiduo = contratoParcelaDto.ValorResiduo;

            contratoParcela.DataEmissao = contratoParcelaDto.DataEmissao;
            contratoParcela.ValorDescontoParcela = contratoParcelaDto.ValorDescontoParcela;
            contratoParcela.ValorAcrescimo = contratoParcelaDto.ValorAcrescimo;
            contratoParcela.IdSituacaoParcela = contratoParcelaDto.IdSituacaoParcela;

            contratoParcela.ValorOriginal = contratoParcelaDto.ValorOriginal;
            contratoParcela.ValorJuros = contratoParcelaDto.ValorJuros;
            contratoParcela.ValorMulta = contratoParcelaDto.ValorMulta;

            contratoParcela.ValorDescontoJuros = contratoParcelaDto.ValorDescontoJuros;
            contratoParcela.ValorMora = contratoParcelaDto.ValorMora;
            contratoParcela.ValorAmortizacao = contratoParcelaDto.ValorAmortizacao;
            contratoParcela.ValorParcela = contratoParcelaDto.ValorParcela;

            projetoTransportadoraEntities.Entry(contratoParcela).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }

        public void Antecipar(ContratoParcelaDto contratoParcelaDto)
        {
            var contratoParcela = projetoTransportadoraEntities.ContratoParcela.FirstOrDefault(x => x.Id == contratoParcelaDto.Id);

            contratoParcela.IdSituacaoParcela = contratoParcelaDto.IdSituacaoParcela;
            contratoParcela.ValorJuros = contratoParcelaDto.ValorJuros;
            contratoParcela.ValorParcela = contratoParcelaDto.ValorParcela;

            contratoParcela.ValorMulta = contratoParcelaDto.ValorMulta;
            contratoParcela.ValorMora = contratoParcelaDto.ValorMora;
            contratoParcela.ValorDescontoJuros = contratoParcelaDto.ValorDescontoJuros;

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
