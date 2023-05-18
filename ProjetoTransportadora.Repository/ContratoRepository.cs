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

            return query.FirstOrDefault() != null ? true : false;
        }

        public ContratoDto Obter(ContratoDto contratoDto)
        {
            IQueryable<Contrato> query = projetoTransportadoraEntities.Contrato;

            if (contratoDto.Id > 0)
                query = query.Where(x => x.Id == contratoDto.Id);

            if (contratoDto.NumeroContrato > 0)
                query = query.Where(x => x.NumeroContrato == contratoDto.NumeroContrato);

            return query.Select(x => new ContratoDto() { Id = x.Id, NumeroContrato = x.NumeroContrato }).FirstOrDefault();
        }

        public object ListarResumo(List<ContratoDto> contratoDto)
        {
            var query = (from c in contratoDto
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
                         }).OrderBy(x => x.SituacaoContrato).ThenBy(x => x.Produto);

            return query.ToList();
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
                VeiculoDto = x.Veiculo == null ? null : new VeiculoDto { Id = x.Veiculo.Id, Placa = x.Veiculo.Placa, Modelo = x.Veiculo.Modelo, Chassi = x.Veiculo.Chassi, Renavam = x.Veiculo.Renavam, Ativo = x.Veiculo.Ativo },
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
                ValorAntecipacao = x.ValorAntecipacao,
                ValorVeiculo = x.ValorVeiculo,
                ValorEntrada = x.ValorEntrada,
                ValorDocumentacao = x.ValorDocumentacao,
                ValorDesconto = x.ValorDesconto,
                ValorFinanciadoDocumentacao = x.ValorFinanciadoDocumentacao,
                IdVeiculoEntrada = x.IdVeiculoEntrada,
                VeiculoEntradaDto = x.VeiculoEntrada == null ? null : new VeiculoDto { Id = x.VeiculoEntrada.Id, Placa = x.VeiculoEntrada.Placa, Modelo = x.VeiculoEntrada.Modelo, Chassi = x.VeiculoEntrada.Chassi, Renavam = x.VeiculoEntrada.Renavam, Ativo = x.VeiculoEntrada.Ativo },
                ValorFinanciado = x.ValorFinanciado,
                ValorCaixa = x.ValorCaixa,
                ValorDepositado = x.ValorDepositado,
                ValorTarifa = x.ValorTarifa,
                TaxaJuros = x.TaxaJuros,
                Ativo = x.Ativo,
                IdUsuarioCadastro = x.IdUsuarioCadastro,
                DataCadastro = x.DataCadastro,
                IdUsuarioInativacao = x.IdUsuarioInativacao,
                DataInativacao = x.DataInativacao,
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
                    ValorOriginal = w.ValorOriginal,
                    ValorAmortizacao = w.ValorAmortizacao,
                    ValorJuros = w.ValorJuros,
                    ValorMulta = w.ValorMulta,
                    ValorMora = w.ValorMora,
                    ValorDesconto = w.ValorDesconto,
                    ValorParcela = w.ValorParcela
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
                ValorFinanciado = contratoDto.ValorFinanciado,
                ValorCaixa = contratoDto.ValorCaixa,
                ValorDepositado = contratoDto.ValorDepositado,
                TaxaJuros = contratoDto.TaxaJuros,
                ValorTarifa = contratoDto.ValorTarifa,
                Ativo = true,
                IdUsuarioCadastro = contratoDto.IdUsuarioCadastro,
                DataCadastro = contratoDto.DataCadastro
            };

            projetoTransportadoraEntities.Contrato.Add(contrato);
            projetoTransportadoraEntities.SaveChanges();

            return contrato.Id;
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
