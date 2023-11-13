using ProjetoTransportadora.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class ContratoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public ContratoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public int ListarTotal(ContratoDto contratoDto)
        {
            IQueryable<Contrato> query = projetoTransportadoraEntities.Contrato;

            if (contratoDto.DataCadastro != DateTime.MinValue)
                query = query.Where(x => x.DataCadastro.Month == contratoDto.DataCadastro.Month && x.DataCadastro.Year == contratoDto.DataCadastro.Year);

            if (contratoDto.Ativo.HasValue)
                query = query.Where(x => x.Ativo == contratoDto.Ativo.Value);

            return query.Count();
        }

        public bool Existe(ContratoDto contratoDto)
        {
            IQueryable<Contrato> query = projetoTransportadoraEntities.Contrato;

            if (contratoDto.Id > 0)
                query = query.Where(x => x.Id == contratoDto.Id);

            if (contratoDto.NumeroContrato > 0)
                query = query.Where(x => x.NumeroContrato == contratoDto.NumeroContrato);

            return query.Count() > 0 ? true : false;
        }

        public ContratoDto Obter(ContratoDto contratoDto)
        {
            IQueryable<Contrato> query = projetoTransportadoraEntities.Contrato;

            if (contratoDto.Id > 0)
                query = query.Where(x => x.Id == contratoDto.Id);

            if (contratoDto.NumeroContrato > 0)
                query = query.Where(x => x.NumeroContrato == contratoDto.NumeroContrato);

            return query.Select(x => new ContratoDto()
            {
                Id = x.Id,
                DataContrato = x.DataContrato,
                TaxaJuros = x.TaxaJuros,
                ValorFinanciado = x.ValorFinanciado,
                TaxaMora = x.TaxaMora,
                TaxaMulta = x.TaxaMulta
            }).FirstOrDefault();
        }

        public dynamic ListarGridParcela(ContratoDto contratoDto = null)
        {
            var idContratoParcela = contratoDto.ContratoParcelaDto?.FirstOrDefault()?.Id;
            var placa = contratoDto.VeiculoDto?.Placa;
            var idSituacaoParcela = contratoDto.ContratoParcelaDto?.FirstOrDefault()?.IdSituacaoParcela;
            var dataVencimentoInicio = contratoDto.ContratoParcelaDto?.FirstOrDefault()?.DataVencimentoInicio;
            var dataVencimentoFim = contratoDto.ContratoParcelaDto?.FirstOrDefault()?.DataVencimentoFim;

            var query = (from cp in projetoTransportadoraEntities.ContratoParcela
                         join c in projetoTransportadoraEntities.Contrato on cp.IdContrato equals c.Id
                         join p in projetoTransportadoraEntities.Pessoa on c.PessoaCliente.Id equals p.Id
                         join sp in projetoTransportadoraEntities.SituacaoParcela on cp.IdSituacaoParcela equals sp.Id
                         where c.Id == (contratoDto.Id > 0 ? contratoDto.Id : c.Id)
                         && c.IdCliente == (contratoDto.IdCliente > 0 ? contratoDto.IdCliente : c.IdCliente)
                         && c.IdProduto == (contratoDto.IdProduto > 0 ? contratoDto.IdProduto : c.IdProduto)
                         && c.Veiculo.Placa.Contains(!string.IsNullOrEmpty(placa) ? placa : c.Veiculo.Placa)
                         && c.IdSituacaoContrato == (contratoDto.IdSituacaoContrato > 0 ? contratoDto.IdSituacaoContrato : c.IdSituacaoContrato)
                         && c.DataContrato >= (contratoDto.DataContratoInicial != DateTime.MinValue ? contratoDto.DataContratoInicial : c.DataContrato)
                         && c.DataContrato <= (contratoDto.DataContratoFinal != DateTime.MinValue ? contratoDto.DataContratoFinal : c.DataContrato)
                         && cp.DataVencimento >= (dataVencimentoInicio != DateTime.MinValue ? dataVencimentoInicio : cp.DataVencimento)
                         && cp.DataVencimento <= (dataVencimentoFim != DateTime.MinValue ? dataVencimentoFim : cp.DataVencimento)
                         && cp.Id == (idContratoParcela > 0 ? idContratoParcela : cp.Id)
                         && cp.IdSituacaoParcela == (idSituacaoParcela > 0 ? idSituacaoParcela : cp.IdSituacaoParcela)
                         select new
                         {
                             Id = c.Id,
                             NumeroParcela = cp.NumeroParcela,
                             IdContratoParcela = cp.Id,
                             NomeCliente = p.Nome,
                             DataVencimento = cp.DataVencimento,
                             ValorParcela = cp.ValorParcela,
                             IdSituacaoParcela = sp.Id,
                             NomeSituacaoParcela = sp.Nome
                         });

            return query.ToList();
        }

        public dynamic ListarGrid(ContratoDto contratoDto = null)
        {
            IQueryable<Contrato> query = projetoTransportadoraEntities.Contrato;

            if (contratoDto != null)
            {
                if (contratoDto.Id > 0)
                    query = query.Where(x => x.Id == contratoDto.Id);

                if (contratoDto.IdCliente > 0)
                    query = query.Where(x => x.IdCliente == contratoDto.IdCliente);

                if (contratoDto.IdProduto > 0)
                    query = query.Where(x => x.IdProduto == contratoDto.IdProduto);

                if (!string.IsNullOrEmpty(contratoDto.VeiculoDto?.Placa))
                    query = query.Where(x => x.Veiculo.Placa.Contains(contratoDto.VeiculoDto.Placa));

                if (contratoDto.IdSituacaoContrato > 0)
                    query = query.Where(x => x.IdSituacaoContrato == contratoDto.IdSituacaoContrato);

                if (contratoDto.DataContratoInicial != DateTime.MinValue && contratoDto.DataContratoFinal != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato >= contratoDto.DataContratoInicial && x.DataContrato <= contratoDto.DataContratoFinal);
                else if (contratoDto.DataContratoInicial != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato >= contratoDto.DataContratoInicial);
                else if (contratoDto.DataContratoFinal != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato <= contratoDto.DataContratoFinal);
            }

            return query.Select(x => new
            {
                Id = x.Id,
                NomeCliente = x.PessoaCliente.Nome,
                PlacaVeiculo = x.Veiculo.Placa,
                ModeloVeiculo = x.Veiculo.Modelo,
                DataContrato = x.DataContrato,
                ValorFinanciado = x.ValorFinanciado,
                IdSituacaoContrato = x.SituacaoContrato.Id,
                NomeSituacaoContrato = x.SituacaoContrato.Nome
            }).ToList();
        }
        public dynamic ListarSituacaoContratoResumo(ContratoDto contratoDto = null)
        {
            IQueryable<Contrato> query = projetoTransportadoraEntities.Contrato;

            if (contratoDto != null)
            {
                if (contratoDto.Id > 0)
                    query = query.Where(x => x.Id == contratoDto.Id);

                if (contratoDto.IdCliente > 0)
                    query = query.Where(x => x.IdCliente == contratoDto.IdCliente);

                if (contratoDto.IdProduto > 0)
                    query = query.Where(x => x.IdProduto == contratoDto.IdProduto);

                if (!string.IsNullOrEmpty(contratoDto.VeiculoDto?.Placa))
                    query = query.Where(x => x.Veiculo.Placa.Contains(contratoDto.VeiculoDto.Placa));

                if (contratoDto.IdSituacaoContrato > 0)
                    query = query.Where(x => x.IdSituacaoContrato == contratoDto.IdSituacaoContrato);

                if (contratoDto.DataContratoInicial != DateTime.MinValue && contratoDto.DataContratoFinal != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato >= contratoDto.DataContratoInicial && x.DataContrato <= contratoDto.DataContratoFinal);
                else if (contratoDto.DataContratoInicial != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato >= contratoDto.DataContratoInicial);
                else if (contratoDto.DataContratoFinal != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato <= contratoDto.DataContratoFinal);
            }

            return (from c in query
                    join sc in projetoTransportadoraEntities.SituacaoContrato on c.IdSituacaoContrato equals sc.Id
                    join p in projetoTransportadoraEntities.Produto on c.IdProduto equals p.Id
                    group c by new { scNome = sc.Nome, pNome = p.Nome } into g1
                    select new
                    {
                        SituacaoContrato = g1.Key.scNome,
                        Produto = g1.Key.pNome,
                        Quantidade = g1.Select(x => x.Id).Count(),
                        ValorEntrada = g1.Sum(x => x.ValorEntrada),
                        ValorFinanciado = g1.Sum(x => x.ValorFinanciado),
                    }).OrderBy(x => x.SituacaoContrato).ThenBy(x => x.Produto).ToList();
        }

        public dynamic ListarSituacaoParcelaResumo(ContratoDto contratoDto = null)
        {
            IQueryable<Contrato> query = projetoTransportadoraEntities.Contrato;

            if (contratoDto != null)
            {
                if (contratoDto.Id > 0)
                    query = query.Where(x => x.Id == contratoDto.Id);

                if (contratoDto.IdCliente > 0)
                    query = query.Where(x => x.IdCliente == contratoDto.IdCliente);

                if (contratoDto.IdProduto > 0)
                    query = query.Where(x => x.IdProduto == contratoDto.IdProduto);

                if (!string.IsNullOrEmpty(contratoDto.VeiculoDto?.Placa))
                    query = query.Where(x => x.Veiculo.Placa.Contains(contratoDto.VeiculoDto.Placa));

                if (contratoDto.IdSituacaoContrato > 0)
                    query = query.Where(x => x.IdSituacaoContrato == contratoDto.IdSituacaoContrato);

                if (contratoDto.DataContratoInicial != DateTime.MinValue && contratoDto.DataContratoFinal != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato >= contratoDto.DataContratoInicial && x.DataContrato <= contratoDto.DataContratoFinal);
                else if (contratoDto.DataContratoInicial != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato >= contratoDto.DataContratoInicial);
                else if (contratoDto.DataContratoFinal != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato <= contratoDto.DataContratoFinal);
            }

            return (from c in query
                    join cp in projetoTransportadoraEntities.ContratoParcela on c.Id equals cp.IdContrato
                    join sp in projetoTransportadoraEntities.SituacaoParcela on cp.IdSituacaoParcela equals sp.Id
                    group cp by new { spNome = sp.Nome } into g1
                    select new
                    {
                        SituacaoParcela = g1.Key.spNome,
                        Quantidade = g1.Select(x => x.Id).Count(),
                        ValorJuros = g1.Sum(x => x.ValorJuros),
                        ValorParcela = g1.Sum(x => x.ValorParcela),
                    }).OrderBy(x => x.SituacaoParcela).ToList();
        }

        public List<ContratoDto> Listar(ContratoDto contratoDto)
        {
            IQueryable<Contrato> query = projetoTransportadoraEntities.Contrato;

            if (contratoDto != null)
            {
                if (contratoDto.Id > 0)
                    query = query.Where(x => x.Id == contratoDto.Id);

                if (contratoDto.IdCliente > 0)
                    query = query.Where(x => x.IdCliente == contratoDto.IdCliente);

                if (contratoDto.IdProduto > 0)
                    query = query.Where(x => x.IdProduto == contratoDto.IdProduto);

                if (!string.IsNullOrEmpty(contratoDto.VeiculoDto?.Placa))
                    query = query.Where(x => x.Veiculo.Placa.Contains(contratoDto.VeiculoDto.Placa));

                if (contratoDto.IdSituacaoContrato > 0)
                    query = query.Where(x => x.IdSituacaoContrato == contratoDto.IdSituacaoContrato);

                if (contratoDto.DataContratoInicial != DateTime.MinValue && contratoDto.DataContratoFinal != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato >= contratoDto.DataContratoInicial && x.DataContrato <= contratoDto.DataContratoFinal);
                else if (contratoDto.DataContratoInicial != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato >= contratoDto.DataContratoInicial);
                else if (contratoDto.DataContratoFinal != DateTime.MinValue)
                    query = query.Where(x => x.DataContrato <= contratoDto.DataContratoFinal);
            }

            var contratosDto = query.Select(x => new ContratoDto()
            {
                Id = x.Id,
                NumeroContrato = x.NumeroContrato,
                IdCliente = x.IdCliente,
                PessoaClienteDto = x.PessoaCliente == null ? null : new PessoaDto { Id = x.PessoaCliente.Id, Nome = x.PessoaCliente.Nome, Cpf = x.PessoaCliente.Cpf, Cnpj = x.PessoaCliente.Cnpj, Ativo = x.PessoaCliente.Ativo },
                IdVeiculo = x.IdVeiculo,
                VeiculoDto = x.Veiculo == null ? null : new VeiculoDto { Id = x.Veiculo.Id, Placa = x.Veiculo.Placa, Modelo = x.Veiculo.Modelo, Chassi = x.Veiculo.Chassi, Renavam = x.Veiculo.Renavam, Ativo = x.Veiculo.Ativo, Cor = x.Veiculo.Cor, MontadoraDto = new MontadoraDto() { Id = x.Veiculo.Montadora.Id, Nome = x.Veiculo.Montadora.Nome } },
                IdProduto = x.IdProduto,
                ProdutoDto = x.Produto == null ? null : new ProdutoDto { Id = x.Produto.Id, Nome = x.Produto.Nome, Ativo = x.Produto.Ativo },
                IdSituacaoContrato = x.IdSituacaoContrato,
                SituacaoContratoDto = x.SituacaoContrato == null ? null : new SituacaoContratoDto() { Id = x.SituacaoContrato.Id, Ativo = x.SituacaoContrato.Ativo, DataCadastro = x.SituacaoContrato.DataCadastro, DataInativacao = x.SituacaoContrato.DataInativacao, IdUsuarioCadastro = x.SituacaoContrato.IdUsuarioCadastro, IdUsuarioInativacao = x.SituacaoContrato.IdUsuarioInativacao, Nome = x.SituacaoContrato.Nome },
                IdCanal = x.IdCanal,
                CanalDto = x.Canal == null ? null : new CanalDto() { Id = x.Canal.Id, Ativo = x.Canal.Ativo, DataCadastro = x.Canal.DataCadastro, DataInativacao = x.Canal.DataInativacao, IdUsuarioCadastro = x.Canal.IdUsuarioCadastro, IdUsuarioInativacao = x.Canal.IdUsuarioInativacao, Nome = x.Canal.Nome },
                DataContrato = x.DataContrato,
                IdFiador = x.IdFiador,
                IdIndicacao = x.IdIndicacao,
                IdPromotor = x.IdPromotor,
                DataPrimeiraParcela = x.DataPrimeiraParcela,
                DataBaixa = x.DataBaixa,
                DataAntecipacao = x.DataAntecipacao,
                ValorAntecipacao = x.ValorAntecipacao ?? 0D,
                ValorVeiculo = x.ValorVeiculo ?? 0D,
                ValorEntrada = x.ValorEntrada ?? 0D,
                ValorDocumentacao = x.ValorDocumentacao ?? 0D,
                ValorDesconto = x.ValorDesconto ?? 0D,
                ValorFinanciadoDocumentacao = x.ValorFinanciadoDocumentacao ?? 0D,
                ValorFinanciadoVeiculo = x.ValorFinanciadoVeiculo ?? 0D,
                IdVeiculoEntrada = x.IdVeiculoEntrada,
                VeiculoEntradaDto = x.VeiculoEntrada == null ? null : new VeiculoDto { Id = x.VeiculoEntrada.Id, Placa = x.VeiculoEntrada.Placa, Modelo = x.VeiculoEntrada.Modelo, Chassi = x.VeiculoEntrada.Chassi, Renavam = x.VeiculoEntrada.Renavam, Ativo = x.VeiculoEntrada.Ativo, Cor = x.VeiculoEntrada.Cor, MontadoraDto = new MontadoraDto() { Id = x.VeiculoEntrada.Montadora.Id, Nome = x.VeiculoEntrada.Montadora.Nome } },
                ValorVeiculoEntrada = x.ValorVeiculoEntrada ?? 0D,
                ValorFinanciado = x.ValorFinanciado,
                ValorDinheiro = x.ValorDinheiro ?? 0D,
                ValorDepositado = x.ValorDepositado ?? 0D,
                ValorTarifa = x.ValorTarifa ?? 0D,
                TaxaJuros = x.TaxaJuros,
                TaxaMora = x.TaxaMora ?? 0D,
                TaxaMulta = x.TaxaMulta ?? 0D,
                Ativo = x.Ativo,
                IdUsuarioCadastro = x.IdUsuarioCadastro,
                DataCadastro = x.DataCadastro,
                IdUsuarioInativacao = x.IdUsuarioInativacao,
                DataInativacao = x.DataInativacao,
                IdUsuarioAntecipacao = x.IdUsuarioAntecipacao,
                IdUsuarioBaixa = x.IdUsuarioBaixa,
                IdTipoContrato = x.IdTipoContrato,
                ContratoParcelaDto = x.ContratoParcela.Select(w => new ContratoParcelaDto()
                {
                    Id = w.Id,
                    NumeroParcela = w.NumeroParcela,
                    IdContrato = w.IdContrato,
                    IdSituacaoParcela = w.IdSituacaoParcela,
                    SituacaoParcelaDto = w.SituacaoParcela == null ? null : new SituacaoParcelaDto() { Id = w.SituacaoParcela.Id, Ativo = w.SituacaoParcela.Ativo, DataCadastro = w.SituacaoParcela.DataCadastro, DataInativacao = w.SituacaoParcela.DataInativacao, IdUsuarioCadastro = w.SituacaoParcela.IdUsuarioCadastro, IdUsuarioInativacao = w.SituacaoParcela.IdUsuarioInativacao, Nome = w.SituacaoParcela.Nome },
                    DataVencimento = w.DataVencimento,
                    DiasParcela = w.DiasParcela,
                    DiasContrato = w.DiasContrato,
                    DataPagamento = w.DataPagamento,
                    DataEmissao = w.DataEmissao,
                    ValorOriginal = w.ValorOriginal,
                    ValorAmortizacao = w.ValorAmortizacao ?? 0D,
                    ValorJuros = w.ValorJuros ?? 0D,
                    ValorMulta = w.ValorMulta ?? 0D,
                    ValorMora = w.ValorMora ?? 0D,
                    ValorParcela = w.ValorParcela,
                    DataInicio = w.DataInicio,
                    TaxaMulta = w.TaxaMulta ?? 0D,
                    TaxaMora = w.TaxaMora ?? 0D,
                    ValorDescontoJuros = w.ValorDescontoJuros ?? 0D,
                    ValorDescontoParcela = w.ValorDescontoParcela ?? 0D,
                    ValorAcrescimo = w.ValorAcrescimo ?? 0D,
                    ValorResiduo = w.ValorResiduo ?? 0D
                }).ToList(),
                ContratoHistoricoDto = x.ContratoHistorico.Select(w => new ContratoHistoricoDto()
                {
                    Id = w.Id,
                    IdContrato = w.IdContrato,
                    DataHistorico = w.DataHistorico,
                    Descricao = w.Descricao,
                    IdUsuarioCadastro = w.IdUsuarioCadastro,
                    DataCadastro = w.DataCadastro
                }).ToList()
            }).ToList();

            foreach (var c in contratosDto)
            {
                if (c.IdFiador > 0)
                {
                    var pessoaFiador = projetoTransportadoraEntities.Pessoa.FirstOrDefault(x => x.Id == c.IdFiador);

                    if (pessoaFiador != null)
                        c.PessoaFiadorDto = new PessoaDto() { Id = pessoaFiador.Id, Nome = pessoaFiador.Nome, Cpf = pessoaFiador.Cpf, Ativo = pessoaFiador.Ativo, Cnpj = pessoaFiador.Cnpj };
                }

                if (c.IdIndicacao > 0)
                {
                    var pessoaIndicacao = projetoTransportadoraEntities.Pessoa.FirstOrDefault(x => x.Id == c.IdIndicacao);

                    if (pessoaIndicacao != null)
                        c.PessoaIndicacaoDto = new PessoaDto() { Id = pessoaIndicacao.Id, Nome = pessoaIndicacao.Nome, Cpf = pessoaIndicacao.Cpf, Ativo = pessoaIndicacao.Ativo, Cnpj = pessoaIndicacao.Cnpj };
                }

                if (c.IdPromotor > 0)
                {
                    var pessoaPromotor = projetoTransportadoraEntities.Pessoa.FirstOrDefault(x => x.Id == c.IdPromotor);

                    if (pessoaPromotor != null)
                        c.PessoaPromotorDto = new PessoaDto() { Id = pessoaPromotor.Id, Nome = pessoaPromotor.Nome, Cpf = pessoaPromotor.Cpf, Ativo = pessoaPromotor.Ativo, Cnpj = pessoaPromotor.Cnpj };
                }
            }

            return contratosDto;
        }

        public int Incluir(ContratoDto contratoDto)
        {
            var contrato = new Contrato()
            {
                NumeroContrato = contratoDto.NumeroContrato,
                IdCliente = contratoDto.IdCliente,
                IdVeiculo = contratoDto.IdVeiculo,
                IdProduto = contratoDto.IdProduto,
                IdSituacaoContrato = contratoDto.IdSituacaoContrato,
                IdCanal = contratoDto.IdCanal,
                DataContrato = contratoDto.DataContrato,
                IdFiador = contratoDto.IdFiador,
                IdIndicacao = contratoDto.IdIndicacao,
                IdPromotor = contratoDto.IdPromotor,
                DataPrimeiraParcela = contratoDto.DataPrimeiraParcela,
                DataBaixa = contratoDto.DataBaixa,
                DataAntecipacao = contratoDto.DataAntecipacao,
                ValorAntecipacao = contratoDto.ValorAntecipacao,
                ValorVeiculo = contratoDto.ValorVeiculo,
                ValorEntrada = contratoDto.ValorEntrada,
                ValorDocumentacao = contratoDto.ValorDocumentacao,
                ValorDesconto = contratoDto.ValorDesconto,
                ValorFinanciadoVeiculo = contratoDto.ValorFinanciadoVeiculo,
                ValorFinanciadoDocumentacao = contratoDto.ValorFinanciadoDocumentacao,
                IdVeiculoEntrada = contratoDto.IdVeiculoEntrada,
                ValorVeiculoEntrada = contratoDto.ValorVeiculoEntrada,
                ValorFinanciado = contratoDto.ValorFinanciado,
                ValorDinheiro = contratoDto.ValorDinheiro,
                ValorDepositado = contratoDto.ValorDepositado,
                TaxaJuros = contratoDto.TaxaJuros,
                ValorTarifa = contratoDto.ValorTarifa,
                TaxaMora = contratoDto.TaxaMora,
                TaxaMulta = contratoDto.TaxaMulta,
                Ativo = true,
                IdUsuarioCadastro = contratoDto.IdUsuarioCadastro,
                DataCadastro = contratoDto.DataCadastro,
                IdTipoContrato = contratoDto.IdTipoContrato
            };

            projetoTransportadoraEntities.Contrato.Add(contrato);
            projetoTransportadoraEntities.SaveChanges();

            return contrato.Id;
        }

        public void Alterar(ContratoDto contratoDto)
        {
            var contrato = projetoTransportadoraEntities.Contrato.FirstOrDefault(x => x.Id == contratoDto.Id);

            contrato.NumeroContrato = contratoDto.NumeroContrato;
            contrato.IdCliente = contratoDto.IdCliente;
            contrato.IdVeiculo = contratoDto.IdVeiculo;
            contrato.IdProduto = contratoDto.IdProduto;
            //contrato.IdSituacaoContrato = contratoDto.IdSituacaoContrato;
            contrato.IdCanal = contratoDto.IdCanal;
            contrato.DataContrato = contratoDto.DataContrato;
            contrato.IdFiador = contratoDto.IdFiador;
            contrato.IdIndicacao = contratoDto.IdIndicacao;
            contrato.IdPromotor = contratoDto.IdPromotor;
            contrato.DataPrimeiraParcela = contratoDto.DataPrimeiraParcela;
            contrato.DataBaixa = contratoDto.DataBaixa;
            contrato.DataAntecipacao = contratoDto.DataAntecipacao;
            contrato.ValorAntecipacao = contratoDto.ValorAntecipacao;
            contrato.ValorVeiculo = contratoDto.ValorVeiculo;
            contrato.ValorEntrada = contratoDto.ValorEntrada;
            contrato.ValorDocumentacao = contratoDto.ValorDocumentacao;
            contrato.ValorDesconto = contratoDto.ValorDesconto;
            contrato.ValorFinanciadoVeiculo = contratoDto.ValorFinanciadoVeiculo;
            contrato.ValorFinanciadoDocumentacao = contratoDto.ValorFinanciadoDocumentacao;
            contrato.IdVeiculoEntrada = contratoDto.IdVeiculoEntrada;
            contrato.ValorVeiculoEntrada = contratoDto.ValorVeiculoEntrada;
            contrato.ValorFinanciado = contratoDto.ValorFinanciado;
            contrato.ValorDinheiro = contratoDto.ValorDinheiro;
            contrato.ValorDepositado = contratoDto.ValorDepositado;
            contrato.TaxaJuros = contratoDto.TaxaJuros;
            contrato.ValorTarifa = contratoDto.ValorTarifa;
            contrato.TaxaMora = contratoDto.TaxaMora;
            contrato.TaxaMulta = contratoDto.TaxaMulta;
            contrato.IdTipoContrato = contratoDto.IdTipoContrato;

            projetoTransportadoraEntities.Entry(contrato).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }

        public void Antecipar(ContratoDto contratoDto)
        {
            var contrato = projetoTransportadoraEntities.Contrato.FirstOrDefault(x => x.Id == contratoDto.Id);

            contrato.DataAntecipacao = contratoDto.DataAntecipacao;
            contrato.ValorAntecipacao = contratoDto.ValorAntecipacao;
            contrato.IdSituacaoContrato = contratoDto.IdSituacaoContrato;
            contrato.IdUsuarioAntecipacao = contratoDto.IdUsuarioAntecipacao;

            projetoTransportadoraEntities.Entry(contrato).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }

        public void Baixar(ContratoDto contratoDto)
        {
            var contrato = projetoTransportadoraEntities.Contrato.FirstOrDefault(x => x.Id == contratoDto.Id);

            contrato.DataBaixa = contratoDto.DataBaixa;
            contrato.IdSituacaoContrato = contratoDto.IdSituacaoContrato;
            contrato.IdUsuarioBaixa = contratoDto.IdUsuarioBaixa;

            projetoTransportadoraEntities.Entry(contrato).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }

        //public void Alterar(ContratoDto contratoDto)
        //{
        //    var contrato = projetoTransportadoraEntities.Contrato.FirstOrDefault(x => x.Id == contratoDto.Id);

        //    contrato.IdMontadora = contratoDto.IdMontadora;
        //    contrato.Modelo = contratoDto.Modelo;
        //    contrato.Placa = contratoDto.Placa;
        //    contrato.AnoFabricacao = contratoDto.AnoFabricacao;
        //    contrato.AnoModelo = contratoDto.AnoModelo;
        //    contrato.Cor = contratoDto.Cor;
        //    contrato.IdProprietarioAtual = contratoDto.IdProprietarioAtual;
        //    contrato.IdProprietarioAnterior = contratoDto.IdProprietarioAnterior;
        //    contrato.Renavam = contratoDto.Renavam;
        //    contrato.Chassi = contratoDto.Chassi;
        //    contrato.DataAquisicao = contratoDto.DataAquisicao ?? null;
        //    contrato.ValorAquisicao = contratoDto.ValorAquisicao;
        //    contrato.DataVenda = contratoDto.DataVenda;
        //    contrato.ValorVenda = contratoDto.ValorVenda;
        //    contrato.DataRecuperacao = contratoDto.DataRecuperacao ?? null;
        //    contrato.DataValorFIPE = contratoDto.DataValorFIPE ?? null;
        //    contrato.ValorFIPE = contratoDto.ValorFIPE;
        //    contrato.ValorTransportadora = contratoDto.ValorTransportadora;
        //    contrato.Implemento = contratoDto.Implemento;
        //    contrato.Comprimento = contratoDto.Comprimento;
        //    contrato.Altura = contratoDto.Altura;
        //    contrato.Largura = contratoDto.Largura;
        //    contrato.Rastreador = contratoDto.Rastreador;
        //    contrato.IdSituacaoContrato = contratoDto.IdSituacaoContrato;

        //    projetoTransportadoraEntities.Entry(contrato).State = EntityState.Modified;
        //    projetoTransportadoraEntities.SaveChanges();
        //}
    }
}
