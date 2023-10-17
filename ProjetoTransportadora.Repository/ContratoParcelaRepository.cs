using ProjetoTransportadora.Dto;
using System;
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

        public int ListarParcelaPaga(ContratoParcelaDto contratoParcelaDto)
        {
            IQueryable<ContratoParcela> query = projetoTransportadoraEntities.ContratoParcela;

            var dataPagamento = contratoParcelaDto.DataPagamento.GetValueOrDefault();

            if (dataPagamento != DateTime.MinValue)
                query = query.Where(x => x.DataPagamento.Value.Month == dataPagamento.Month && x.DataPagamento.Value.Year == dataPagamento.Year);

            query = query.Where(x => x.IdSituacaoParcela == (int)SituacaoParcelaDto.EnumSituacaoParcela.Paga);

            return query.Count();
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
                ValorAmortizacao = x.ValorAmortizacao ?? 0D,
                ValorJuros = x.ValorJuros ?? 0D,
                ValorMulta = x.ValorMulta ?? 0D,
                ValorMora = x.ValorMora ?? 0D,
                ValorDescontoJuros = x.ValorDescontoJuros ?? 0D,
                ValorDescontoParcela = x.ValorDescontoParcela ?? 0D,
                ValorParcela = x.ValorParcela,
                TaxaMora = x.TaxaMora ?? 0D,
                TaxaMulta = x.TaxaMulta ?? 0D,
                ValorAcrescimo = x.ValorAcrescimo ?? 0D,
                ValorResiduo = x.ValorResiduo ?? 0D,
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
                ValorAmortizacao = x.ValorAmortizacao ?? 0D,
                ValorJuros = x.ValorJuros ?? 0D,
                ValorMulta = x.ValorMulta ?? 0D,
                ValorMora = x.ValorMora ?? 0D,
                ValorDescontoJuros = x.ValorDescontoJuros ?? 0D,
                ValorDescontoParcela = x.ValorDescontoParcela ?? 0D,
                ValorParcela = x.ValorParcela,
                TaxaMora = x.TaxaMora ?? 0D,
                TaxaMulta = x.TaxaMulta ?? 0D,
                ValorAcrescimo = x.ValorAcrescimo ?? 0D,
                ValorResiduo = x.ValorResiduo ?? 0D,
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
                            ValorResiduo = cpr.ValorResiduo ?? 0D
                        });
                    }

                    cp.ValorResiduo = listaContratoParcela.Sum(x => x.ValorResiduo);
                }
            }

            return lista;
        }

        public List<ContratoParcelaDto> ListarSimples(ContratoParcelaDto contratoParcelaDto)
        {
            IQueryable<ContratoParcela> query = projetoTransportadoraEntities.ContratoParcela;

            if (contratoParcelaDto.Id > 0)
                query = query.Where(x => x.Id == contratoParcelaDto.Id);

            if (contratoParcelaDto.IdContrato > 0)
                query = query.Where(x => x.IdContrato == contratoParcelaDto.IdContrato);

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
                ValorAmortizacao = x.ValorAmortizacao ?? 0D,
                ValorJuros = x.ValorJuros ?? 0D,
                ValorMulta = x.ValorMulta ?? 0D,
                ValorMora = x.ValorMora ?? 0D,
                ValorDescontoJuros = x.ValorDescontoJuros ?? 0D,
                ValorDescontoParcela = x.ValorDescontoParcela ?? 0D,
                ValorParcela = x.ValorParcela,
                TaxaMora = x.TaxaMora ?? 0D,
                TaxaMulta = x.TaxaMulta ?? 0D,
                ValorAcrescimo = x.ValorAcrescimo ?? 0D,
                ValorResiduo = x.ValorResiduo ?? 0D
            }).OrderBy(x => x.NumeroParcela).ToList();

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
                ValorAmortizacao = contratoParcelaDto.ValorAmortizacao ?? 0D,
                ValorJuros = contratoParcelaDto.ValorJuros ?? 0D,
                ValorMulta = contratoParcelaDto.ValorMulta ?? 0D,
                ValorMora = contratoParcelaDto.ValorMora ?? 0D,
                ValorDescontoJuros = contratoParcelaDto.ValorDescontoJuros ?? 0D,
                ValorDescontoParcela = contratoParcelaDto.ValorDescontoParcela ?? 0D,
                ValorParcela = contratoParcelaDto.ValorParcela,
                TaxaMulta = contratoParcelaDto.TaxaMulta ?? 0D,
                TaxaMora = contratoParcelaDto.TaxaMora ?? 0D,
                ValorAcrescimo = contratoParcelaDto.ValorAcrescimo ?? 0D,
                ValorResiduo = contratoParcelaDto.ValorResiduo ?? 0D
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
            var listaContratoParcela = projetoTransportadoraEntities.ContratoParcela.Where(x => x.IdContrato == idContrato);

            projetoTransportadoraEntities.ContratoParcela.RemoveRange(listaContratoParcela);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
