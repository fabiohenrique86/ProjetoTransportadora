using ProjetoTransportadora.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class VeiculoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public VeiculoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public bool Existe(VeiculoDto veiculoDto)
        {
            IQueryable<Veiculo> query = projetoTransportadoraEntities.Veiculo;

            if (veiculoDto.Id > 0)
                query = query.Where(x => x.Id == veiculoDto.Id);

            if (!string.IsNullOrEmpty(veiculoDto.Placa))
                query = query.Where(x => x.Placa == veiculoDto.Placa);

            return query.FirstOrDefault() != null ? true : false;
        }

        public VeiculoDto Obter(VeiculoDto veiculoDto)
        {
            IQueryable<Veiculo> query = projetoTransportadoraEntities.Veiculo;

            if (veiculoDto.Id > 0)
                query = query.Where(x => x.Id == veiculoDto.Id);

            if (!string.IsNullOrEmpty(veiculoDto.Placa))
                query = query.Where(x => x.Placa == veiculoDto.Placa);

            return query.Select(x => new VeiculoDto() { Id = x.Id, Placa = x.Placa }).FirstOrDefault();
        }

        public List<VeiculoDto> Listar(VeiculoDto veiculoDto)
        {
            IQueryable<Veiculo> query = projetoTransportadoraEntities.Veiculo;

            if (veiculoDto != null)
            {
                if (veiculoDto.Id > 0)
                    query = query.Where(x => x.Id == veiculoDto.Id);

                if (!string.IsNullOrEmpty(veiculoDto.Placa))
                    query = query.Where(x => x.Placa.Contains(veiculoDto.Placa));

                if (!string.IsNullOrEmpty(veiculoDto.Renavam))
                    query = query.Where(x => x.Renavam.Contains(veiculoDto.Renavam));

                if (veiculoDto.IdProprietarioAtual > 0)
                    query = query.Where(x => x.IdProprietarioAtual == veiculoDto.IdProprietarioAtual);

                if (!string.IsNullOrEmpty(veiculoDto.Modelo))
                    query = query.Where(x => x.Modelo.Contains(veiculoDto.Modelo));

                if (veiculoDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == veiculoDto.Ativo.Value);
            }

            var veiculosDto = query.Select(x => new VeiculoDto()
            {
                Id = x.Id,
                IdMontadora = x.IdMontadora,
                MontadoraDto = new MontadoraDto { Id = x.Montadora.Id, Nome = x.Montadora.Nome, Ativo = x.Montadora.Ativo, DataCadastro = x.Montadora.DataCadastro, DataInativacao = x.Montadora.DataInativacao, IdUsuarioCadastro = x.Montadora.IdUsuarioCadastro, IdUsuarioInativacao = x.Montadora.IdUsuarioInativacao },
                Modelo = x.Modelo,
                Placa = x.Placa,
                AnoFabricacao = x.AnoFabricacao,
                AnoModelo = x.AnoModelo,
                Cor = x.Cor,
                IdProprietarioAtual = x.IdProprietarioAtual,
                PessoaProprietarioAtualDto = x.PessoaProprietarioAtual == null ? null : new PessoaDto { Id = x.PessoaProprietarioAtual.Id, Ativo = x.PessoaProprietarioAtual.Ativo, Cpf = x.PessoaProprietarioAtual.Cpf, Cnpj = x.PessoaProprietarioAtual.Cnpj },
                IdProprietarioAnterior = x.IdProprietarioAnterior,
                PessoaProprietarioAnteriorDto = x.PessoaProprietarioAnterior == null ? null : new PessoaDto { Id = x.PessoaProprietarioAnterior.Id, Ativo = x.PessoaProprietarioAnterior.Ativo, Cpf = x.PessoaProprietarioAnterior.Cpf, Cnpj = x.PessoaProprietarioAnterior.Cnpj },
                Renavam = x.Renavam,
                Chassi = x.Chassi,
                DataAquisicao = x.DataAquisicao,
                ValorAquisicao = x.ValorAquisicao,
                DataVenda = x.DataVenda,
                ValorVenda = x.ValorVenda,
                DataRecuperacao = x.DataRecuperacao,
                DataValorFIPE= x.DataValorFIPE,
                ValorFIPE = x.ValorFIPE,
                ValorTransportadora = x.ValorTransportadora,
                Implemento = x.Implemento,
                Comprimento = x.Comprimento,
                Altura = x.Altura,
                Largura = x.Largura,
                Rastreador = x.Rastreador,
                IdSituacaoVeiculo = x.IdSituacaoVeiculo,
                SituacaoVeiculoDto = x.SituacaoVeiculo == null ? null : new SituacaoVeiculoDto() { Id = x.SituacaoVeiculo.Id, Ativo = x.SituacaoVeiculo.Ativo, DataCadastro = x.SituacaoVeiculo.DataCadastro, DataInativacao = x.SituacaoVeiculo.DataInativacao, IdUsuarioCadastro = x.SituacaoVeiculo.IdUsuarioCadastro, IdUsuarioInativacao = x.SituacaoVeiculo.IdUsuarioInativacao, Nome = x.SituacaoVeiculo.Nome },
                Ativo = x.Ativo,
                IdUsuarioCadastro = x.IdUsuarioCadastro,
                DataCadastro = x.DataCadastro,
                IdUsuarioInativacao = x.IdUsuarioInativacao,
                DataInativacao = x.DataInativacao//,
                //VeiculoHistoricoDto = x.VeiculoHistorico.Select(w => new VeiculoHistoricoDto()
                //{
                //    Id = w.Id,
                //    IdVeiculo = w.IdVeiculo,
                //    DataHistorico = w.DataHistorico,
                //    Descricao = w.Descricao
                //}).ToList()
            }).ToList();

            //foreach (var p in VeiculosDto)
            //{
            //    if (p.IdPai > 0)
            //    {
            //        var VeiculoPai = projetoTransportadoraEntities.Veiculo.FirstOrDefault(x => x.Id == p.IdPai);

            //        if (VeiculoPai != null)
            //            p.VeiculoPaiDto = new VeiculoDto() { Id = VeiculoPai.Id, Nome = VeiculoPai.Nome, Cpf = VeiculoPai.Cpf, Cnpj = VeiculoPai.Cnpj, IdTipoVeiculo = VeiculoPai.IdTipoVeiculo };
            //    }

            //    if (p.IdMae > 0)
            //    {
            //        var VeiculoMae = projetoTransportadoraEntities.Veiculo.FirstOrDefault(x => x.Id == p.IdMae);

            //        if (VeiculoMae != null)
            //            p.VeiculoMaeDto = new VeiculoDto() { Id = VeiculoMae.Id, Nome = VeiculoMae.Nome, Cpf = VeiculoMae.Cpf, Cnpj = VeiculoMae.Cnpj, IdTipoVeiculo = VeiculoMae.IdTipoVeiculo };
            //    }

            //    if (p.IdConjuge > 0)
            //    {
            //        var VeiculoConjuge = projetoTransportadoraEntities.Veiculo.FirstOrDefault(x => x.Id == p.IdConjuge);

            //        if (VeiculoConjuge != null)
            //            p.VeiculoConjugeDto = new VeiculoDto() { Id = VeiculoConjuge.Id, Nome = VeiculoConjuge.Nome, Cpf = VeiculoConjuge.Cpf, Cnpj = VeiculoConjuge.Cnpj, IdTipoVeiculo = VeiculoConjuge.IdTipoVeiculo };
            //    }

            //    if (p.IdProprietario > 0)
            //    {
            //        var VeiculoProprietario = projetoTransportadoraEntities.Veiculo.FirstOrDefault(x => x.Id == p.IdProprietario);

            //        if (VeiculoProprietario != null)
            //            p.VeiculoProprietarioDto = new VeiculoDto() { Id = VeiculoProprietario.Id, Nome = VeiculoProprietario.Nome, Cpf = VeiculoProprietario.Cpf, Cnpj = VeiculoProprietario.Cnpj, IdTipoVeiculo = VeiculoProprietario.IdTipoVeiculo };
            //    }
            //}

            return veiculosDto;
        }

        public int Incluir(VeiculoDto veiculoDto)
        {
            var veiculo = new Veiculo()
            {
                IdMontadora = veiculoDto.IdMontadora,
                Modelo = veiculoDto.Modelo,
                Placa = veiculoDto.Placa,
                AnoFabricacao = veiculoDto.AnoFabricacao,
                AnoModelo = veiculoDto.AnoModelo,
                Cor = veiculoDto.Cor,
                IdProprietarioAtual = veiculoDto.IdProprietarioAtual,
                IdProprietarioAnterior = veiculoDto.IdProprietarioAnterior,
                Renavam = veiculoDto.Renavam,
                Chassi = veiculoDto.Chassi,
                DataAquisicao = veiculoDto.DataAquisicao ?? null,
                ValorAquisicao = veiculoDto.ValorAquisicao,
                DataVenda = veiculoDto.DataVenda,
                ValorVenda = veiculoDto.ValorVenda,
                DataRecuperacao = veiculoDto.DataRecuperacao ?? null,
                DataValorFIPE = veiculoDto.DataValorFIPE ?? null,
                ValorFIPE = veiculoDto.ValorFIPE,
                ValorTransportadora = veiculoDto.ValorTransportadora,
                Implemento = veiculoDto.Implemento,
                Comprimento = veiculoDto.Comprimento,
                Altura = veiculoDto.Altura,
                Largura = veiculoDto.Largura,
                Rastreador = veiculoDto.Rastreador,
                IdSituacaoVeiculo = veiculoDto.IdSituacaoVeiculo,
                Ativo = true,
                IdUsuarioCadastro = veiculoDto.IdUsuarioCadastro,
                DataCadastro = veiculoDto.DataCadastro
            };

            projetoTransportadoraEntities.Veiculo.Add(veiculo);
            projetoTransportadoraEntities.SaveChanges();

            return veiculo.Id;
        }

        public void AlterarStatus(VeiculoDto VeiculoDto)
        {
            var Veiculo = projetoTransportadoraEntities.Veiculo.FirstOrDefault(x => x.Id == VeiculoDto.Id);

            if (VeiculoDto.Ativo.HasValue)
            {
                Veiculo.Ativo = VeiculoDto.Ativo.Value;
                Veiculo.IdUsuarioInativacao = VeiculoDto.IdUsuarioInativacao;
                Veiculo.DataInativacao = VeiculoDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(Veiculo).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }

        public void Alterar(VeiculoDto veiculoDto)
        {
            var veiculo = projetoTransportadoraEntities.Veiculo.FirstOrDefault(x => x.Id == veiculoDto.Id);

            veiculo.IdMontadora = veiculoDto.IdMontadora;
            veiculo.Modelo = veiculoDto.Modelo;
            veiculo.Placa = veiculoDto.Placa;
            veiculo.AnoFabricacao = veiculoDto.AnoFabricacao;
            veiculo.AnoModelo = veiculoDto.AnoModelo;
            veiculo.Cor = veiculoDto.Cor;
            veiculo.IdProprietarioAtual = veiculoDto.IdProprietarioAtual;
            veiculo.IdProprietarioAnterior = veiculoDto.IdProprietarioAnterior;
            veiculo.Renavam = veiculoDto.Renavam;
            veiculo.Chassi = veiculoDto.Chassi;
            veiculo.DataAquisicao = veiculoDto.DataAquisicao ?? null;
            veiculo.ValorAquisicao = veiculoDto.ValorAquisicao;
            veiculo.DataVenda = veiculoDto.DataVenda;
            veiculo.ValorVenda = veiculoDto.ValorVenda;
            veiculo.DataRecuperacao = veiculoDto.DataRecuperacao ?? null;
            veiculo.DataValorFIPE = veiculoDto.DataValorFIPE ?? null;
            veiculo.ValorFIPE = veiculoDto.ValorFIPE;
            veiculo.ValorTransportadora = veiculoDto.ValorTransportadora;
            veiculo.Implemento = veiculoDto.Implemento;
            veiculo.Comprimento = veiculoDto.Comprimento;
            veiculo.Altura = veiculoDto.Altura;
            veiculo.Largura = veiculoDto.Largura;
            veiculo.Rastreador = veiculoDto.Rastreador;
            veiculo.IdSituacaoVeiculo = veiculoDto.IdSituacaoVeiculo;

            if (veiculoDto.Ativo.HasValue)
            {
                veiculo.Ativo = veiculoDto.Ativo.Value;
                veiculo.IdUsuarioInativacao = veiculoDto.IdUsuarioInativacao;
                veiculo.DataInativacao = veiculoDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(veiculo).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
