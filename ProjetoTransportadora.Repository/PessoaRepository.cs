using ProjetoTransportadora.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class PessoaRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public PessoaRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public int ListarTotal(PessoaDto pessoaDto)
        {
            IQueryable<Pessoa> query = projetoTransportadoraEntities.Pessoa;

            if (pessoaDto.DataCadastro != DateTime.MinValue)
                query = query.Where(x => x.DataCadastro.Month == pessoaDto.DataCadastro.Month && x.DataCadastro.Year == pessoaDto.DataCadastro.Year);

            if (pessoaDto.IdTipoPessoa > 0)
                query = query.Where(x => x.IdTipoPessoa == pessoaDto.IdTipoPessoa);

            if (pessoaDto.Ativo.HasValue)
                query = query.Where(x => x.Ativo == pessoaDto.Ativo.Value);

            return query.Count();
        }

        public bool Existe(PessoaDto pessoaDto)
        {
            IQueryable<Pessoa> query = projetoTransportadoraEntities.Pessoa;

            if (pessoaDto.Id > 0)
                query = query.Where(x => x.Id == pessoaDto.Id);

            if (!string.IsNullOrEmpty(pessoaDto.Cpf))
                query = query.Where(x => x.Cpf == pessoaDto.Cpf);

            if (!string.IsNullOrEmpty(pessoaDto.Cnpj))
                query = query.Where(x => x.Cnpj == pessoaDto.Cnpj);

            return query.FirstOrDefault() != null ? true : false;
        }

        public PessoaDto Obter(PessoaDto pessoaDto)
        {
            IQueryable<Pessoa> query = projetoTransportadoraEntities.Pessoa;

            if (pessoaDto.Id > 0)
                query = query.Where(x => x.Id == pessoaDto.Id);

            if (!string.IsNullOrEmpty(pessoaDto.Cpf))
                query = query.Where(x => x.Cpf == pessoaDto.Cpf);

            if (!string.IsNullOrEmpty(pessoaDto.Cnpj))
                query = query.Where(x => x.Cnpj == pessoaDto.Cnpj);

            return query.Select(x => new PessoaDto() { Id = x.Id, Cpf = x.Cpf, Cnpj = x.Cnpj }).FirstOrDefault();
        }

        public List<PessoaDto> ListarAutoComplete(PessoaDto pessoaDto)
        {
            IQueryable<Pessoa> query = projetoTransportadoraEntities.Pessoa;

            if (pessoaDto != null)
            {
                if (pessoaDto.Id > 0)
                    query = query.Where(x => x.Id == pessoaDto.Id);

                if (!string.IsNullOrEmpty(pessoaDto.Nome))
                    query = query.Where(x => x.Nome.Contains(pessoaDto.Nome));

                if (!string.IsNullOrEmpty(pessoaDto.Cpf) || !string.IsNullOrEmpty(pessoaDto.Cnpj))
                    query = query.Where(x => x.Cpf.Contains(pessoaDto.Cpf) || x.Cnpj.Contains(pessoaDto.Cnpj));

                query = query.Where(x => x.Ativo == pessoaDto.Ativo.Value);

                if (pessoaDto.IdTipoPessoa > 0)
                    query = query.Where(x => x.IdTipoPessoa == pessoaDto.IdTipoPessoa);
            }

            return query.Select(x => new PessoaDto()
            {
                Id = x.Id,
                Nome = x.Nome,
                Cpf = x.Cpf,
                Cnpj = x.Cnpj,
                PessoaTelefoneDto = x.PessoaTelefone.Select(w => new PessoaTelefoneDto()
                {
                    Id = w.Id,
                    DDD = w.DDD,
                    IdPessoa = w.IdPessoa,
                    IdTipoTelefone = w.IdTipoTelefone,
                    NomeContato = w.NomeContato,
                    Numero = w.Numero,
                    Pais = w.Pais
                }).ToList(),
                PessoaEmailDto = x.PessoaEmail.Select(w => new PessoaEmailDto()
                {
                    Id = w.Id,
                    IdPessoa = w.IdPessoa,
                    Email = w.Email,
                    NomeContato = w.NomeContato
                }).ToList(),
            }).OrderBy(x => x.Nome).ToList();
        }

        public dynamic ListarGrid(PessoaDto pessoaDto)
        {
            IQueryable<Pessoa> query = projetoTransportadoraEntities.Pessoa;

            if (pessoaDto != null)
            {
                if (pessoaDto.Id > 0)
                    query = query.Where(x => x.Id == pessoaDto.Id);

                if (!string.IsNullOrEmpty(pessoaDto.Nome))
                    query = query.Where(x => x.Nome.Contains(pessoaDto.Nome));

                if (!string.IsNullOrEmpty(pessoaDto.Cpf))
                    query = query.Where(x => x.Cpf.Contains(pessoaDto.Cpf.Replace(".", "").Replace("-", "").Trim()));

                if (!string.IsNullOrEmpty(pessoaDto.Rg))
                    query = query.Where(x => x.Rg.Contains(pessoaDto.Rg));

                if (!string.IsNullOrEmpty(pessoaDto.Cnpj))
                    query = query.Where(x => x.Cnpj.Contains(pessoaDto.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "")));

                if (pessoaDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == pessoaDto.Ativo.Value);

                if (pessoaDto.IdTipoPessoa > 0)
                    query = query.Where(x => x.IdTipoPessoa == pessoaDto.IdTipoPessoa);

                if (pessoaDto.IdProprietario > 0)
                    query = query.Where(x => x.IdProprietario == pessoaDto.IdProprietario);
            }

            return query.Select(x => new
            {
                Id = x.Id,
                Nome = x.Nome,
                Cpf = x.Cpf,
                Cnpj = x.Cnpj,
                Rg = x.Rg,
                DataNascimento = x.DataNascimento,
                NomeProprietario = x.PessoaProprietario == null ? string.Empty : x.PessoaProprietario.Nome,
                Ativo = x.Ativo
            }).ToList();
        }

        public List<PessoaDto> Listar(PessoaDto pessoaDto)
        {
            IQueryable<Pessoa> query = projetoTransportadoraEntities.Pessoa;

            if (pessoaDto != null)
            {
                if (pessoaDto.Id > 0)
                    query = query.Where(x => x.Id == pessoaDto.Id);

                if (!string.IsNullOrEmpty(pessoaDto.Nome))
                    query = query.Where(x => x.Nome.Contains(pessoaDto.Nome));

                if (!string.IsNullOrEmpty(pessoaDto.Cpf))
                    query = query.Where(x => x.Cpf.Contains(pessoaDto.Cpf.Replace(".", "").Replace("-", "").Trim()));

                if (!string.IsNullOrEmpty(pessoaDto.Rg))
                    query = query.Where(x => x.Rg.Contains(pessoaDto.Rg));

                if (!string.IsNullOrEmpty(pessoaDto.Cnpj))
                    query = query.Where(x => x.Cnpj.Contains(pessoaDto.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "")));

                if (pessoaDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == pessoaDto.Ativo.Value);

                if (pessoaDto.IdTipoPessoa > 0)
                    query = query.Where(x => x.IdTipoPessoa == pessoaDto.IdTipoPessoa);

                if (pessoaDto.IdProprietario > 0)
                    query = query.Where(x => x.IdProprietario == pessoaDto.IdProprietario);
            }

            var pessoasDto = query.Select(x => new PessoaDto()
            {
                Id = x.Id,
                Nome = x.Nome,
                IdEstadoCivil = x.IdEstadoCivil,
                EstadoCivilDto = x.EstadoCivil == null ? null : new EstadoCivilDto() { Id = x.EstadoCivil.Id, Nome = x.EstadoCivil.Nome },
                Rg = x.Rg,
                Cpf = x.Cpf,
                DataNascimento = x.DataNascimento,
                CidadeNascimento = x.CidadeNascimento,
                UfNascimento = x.UfNascimento,
                CepResidencia = x.CepResidencia,
                LogradouroResidencia = x.LogradouroResidencia,
                ComplementoResidencia = x.ComplementoResidencia,
                NumeroResidencia = x.NumeroResidencia,
                BairroResidencia = x.BairroResidencia,
                CidadeResidencia = x.CidadeResidencia,
                UfResidencia = x.UfResidencia,
                Profissao = x.Profissao,
                IdTipoResidencia = x.IdTipoResidencia,
                TipoResidenciaDto = x.TipoResidencia == null ? null : new TipoResidenciaDto() { Id = x.TipoResidencia.Id, Nome = x.TipoResidencia.Nome },
                TempoResidencial = x.TempoResidencial,
                ValorAluguel = x.ValorAluguel,
                IdRegimeCasamento = x.IdRegimeCasamento,
                RegimeCasamentoDto = x.RegimeCasamento == null ? null : new RegimeCasamentoDto() { Id = x.RegimeCasamento.Id, Nome = x.RegimeCasamento.Nome },
                DataAdmissao = x.DataAdmissao,
                Cargo = x.Cargo,
                ValorSalario = x.ValorSalario,
                DataReferenciaSalario = x.DataReferenciaSalario,
                Cnpj = x.Cnpj,
                IdProduto = x.IdProduto,
                ProdutoDto = x.Produto == null ? null : new ProdutoDto() { Id = x.Produto.Id, Nome = x.Produto.Nome },
                DataAbertura = x.DataAbertura,
                Ativo = x.Ativo,
                IdUsuarioCadastro = x.IdUsuarioCadastro,
                DataCadastro = x.DataCadastro,
                IdUsuarioInativacao = x.IdUsuarioInativacao,
                DataInativacao = x.DataInativacao,
                IdTipoPessoa = x.IdTipoPessoa,
                TipoPessoaDto = new TipoPessoaDto() { Id = x.TipoPessoa.Id, Nome = x.TipoPessoa.Nome },
                IdPai = x.IdPai,
                IdMae = x.IdMae,
                IdConjuge = x.IdConjuge,
                IdProprietario = x.IdProprietario,
                EmpresaPessoal = x.EmpresaPessoal,
                EmpresaTrabalho = x.EmpresaTrabalho,
                TelefoneEmpresa = x.TelefoneEmpresa,
                EmailEmpresa = x.EmailEmpresa,
                PessoaAvalistaDto = x.PessoaAvalista.Select(w => new PessoaAvalistaDto()
                {
                    Id = w.Id,
                    IdPessoa = w.IdPessoa,
                    IdAvalista = w.IdAvalista
                }).ToList(),
                PessoaTelefoneDto = x.PessoaTelefone.Select(w => new PessoaTelefoneDto()
                {
                    Id = w.Id,
                    DDD = w.DDD,
                    IdPessoa = w.IdPessoa,
                    IdTipoTelefone = w.IdTipoTelefone,
                    NomeContato = w.NomeContato,
                    Numero = w.Numero,
                    Pais = w.Pais
                }).ToList(),
                PessoaEmailDto = x.PessoaEmail.Select(w => new PessoaEmailDto()
                {
                    Id = w.Id,
                    IdPessoa = w.IdPessoa,
                    Email = w.Email,
                    NomeContato = w.NomeContato
                }).ToList(),
                PessoaHistoricoDto = x.PessoaHistorico.Select(w => new PessoaHistoricoDto()
                {
                    Id = w.Id,
                    IdPessoa = w.IdPessoa,
                    DataHistorico = w.DataHistorico,
                    Descricao = w.Descricao
                }).ToList(),
                PessoaReferenciaDto = x.PessoaReferencia.Select(w => new PessoaReferenciaDto()
                {
                    Id = w.Id,
                    IdPessoa = w.IdPessoa,
                    IdTipoReferencia = w.IdTipoReferencia,
                    DataReferencia = w.DataReferencia,
                    Descricao = w.Descricao,
                    Telefone = w.Telefone,
                    Nome = w.Nome
                }).ToList(),
                PessoaContratoDto = x.Contrato.Select(w => new ContratoDto()
                {
                    Id = w.Id,
                    NumeroContrato = w.NumeroContrato,
                    DataContrato = w.DataContrato,
                    ValorFinanciado = w.ValorFinanciado,
                    IdCliente = w.IdCliente,
                    IdVeiculo = w.IdVeiculo,
                    VeiculoDto = w.Veiculo == null ? null : new VeiculoDto { Id = w.Veiculo.Id, Placa = w.Veiculo.Placa, Modelo = w.Veiculo.Modelo, Chassi = w.Veiculo.Chassi, Renavam = w.Veiculo.Renavam, Ativo = w.Veiculo.Ativo },
                    IdSituacaoContrato = w.IdSituacaoContrato,
                    SituacaoContratoDto = w.SituacaoContrato == null ? null : new SituacaoContratoDto() { Id = w.SituacaoContrato.Id, Ativo = w.SituacaoContrato.Ativo, DataCadastro = w.SituacaoContrato.DataCadastro, DataInativacao = w.SituacaoContrato.DataInativacao, IdUsuarioCadastro = w.SituacaoContrato.IdUsuarioCadastro, IdUsuarioInativacao = w.SituacaoContrato.IdUsuarioInativacao, Nome = w.SituacaoContrato.Nome },
                }).ToList()
            }).ToList();

            foreach (var p in pessoasDto)
            {
                if (p.IdPai > 0)
                {
                    var pessoaPai = projetoTransportadoraEntities.Pessoa.FirstOrDefault(x => x.Id == p.IdPai);

                    if (pessoaPai != null)
                        p.PessoaPaiDto = new PessoaDto() { Id = pessoaPai.Id, Nome = pessoaPai.Nome, Cpf = pessoaPai.Cpf, Cnpj = pessoaPai.Cnpj, IdTipoPessoa = pessoaPai.IdTipoPessoa };
                }

                if (p.IdMae > 0)
                {
                    var pessoaMae = projetoTransportadoraEntities.Pessoa.FirstOrDefault(x => x.Id == p.IdMae);

                    if (pessoaMae != null)
                        p.PessoaMaeDto = new PessoaDto() { Id = pessoaMae.Id, Nome = pessoaMae.Nome, Cpf = pessoaMae.Cpf, Cnpj = pessoaMae.Cnpj, IdTipoPessoa = pessoaMae.IdTipoPessoa };
                }

                if (p.IdConjuge > 0)
                {
                    var pessoaConjuge = projetoTransportadoraEntities.Pessoa.FirstOrDefault(x => x.Id == p.IdConjuge);

                    if (pessoaConjuge != null)
                        p.PessoaConjugeDto = new PessoaDto() { Id = pessoaConjuge.Id, Nome = pessoaConjuge.Nome, Cpf = pessoaConjuge.Cpf, Cnpj = pessoaConjuge.Cnpj, IdTipoPessoa = pessoaConjuge.IdTipoPessoa };
                }

                if (p.IdProprietario > 0)
                {
                    var pessoaProprietario = projetoTransportadoraEntities.Pessoa.FirstOrDefault(x => x.Id == p.IdProprietario);

                    if (pessoaProprietario != null)
                        p.PessoaProprietarioDto = new PessoaDto() { Id = pessoaProprietario.Id, Nome = pessoaProprietario.Nome, Cpf = pessoaProprietario.Cpf, Cnpj = pessoaProprietario.Cnpj, IdTipoPessoa = pessoaProprietario.IdTipoPessoa };
                }

                foreach (var item in p.PessoaAvalistaDto)
                {
                    var pessoaAvalista = projetoTransportadoraEntities.Pessoa.FirstOrDefault(x => x.Id == item.IdAvalista);

                    if (pessoaAvalista != null)
                    {
                        item.AvalistaDto = new PessoaDto()
                        {
                            Id = pessoaAvalista.Id,
                            Nome = pessoaAvalista.Nome,
                            IdTipoPessoa = pessoaAvalista.IdTipoPessoa,
                            Cpf = pessoaAvalista.Cpf,
                            Rg = pessoaAvalista.Rg,
                            DataNascimento = pessoaAvalista.DataNascimento,
                            CidadeNascimento = pessoaAvalista.CidadeNascimento,
                            UfNascimento = pessoaAvalista.UfNascimento,                            
                            IdEstadoCivil = pessoaAvalista.IdEstadoCivil,
                            IdRegimeCasamento = pessoaAvalista.IdRegimeCasamento,
                            PessoaConjugeDto = pessoaAvalista.PessoaConjuge == null ? null : new PessoaDto() { Id = pessoaAvalista.PessoaConjuge.Id, Nome = pessoaAvalista.PessoaConjuge.Nome },
                            PessoaPaiDto = pessoaAvalista.PessoaPai == null ? null : new PessoaDto() { Id = pessoaAvalista.PessoaPai.Id, Nome = pessoaAvalista.PessoaPai.Nome },
                            PessoaMaeDto = pessoaAvalista.PessoaMae == null ? null : new PessoaDto() { Id = pessoaAvalista.PessoaMae.Id, Nome = pessoaAvalista.PessoaMae.Nome },
                            CepResidencia = pessoaAvalista.CepResidencia,
                            LogradouroResidencia = pessoaAvalista.LogradouroResidencia,
                            NumeroResidencia = pessoaAvalista.NumeroResidencia,
                            ComplementoResidencia = pessoaAvalista.ComplementoResidencia,
                            BairroResidencia = pessoaAvalista.BairroResidencia,
                            CidadeResidencia = pessoaAvalista.CidadeResidencia,
                            UfResidencia = pessoaAvalista.UfResidencia,
                            IdTipoResidencia = pessoaAvalista.IdTipoResidencia,
                            ValorAluguel = pessoaAvalista.ValorAluguel,
                            TempoResidencial = pessoaAvalista.TempoResidencial,
                            Profissao = pessoaAvalista.Profissao,
                            EmpresaTrabalho = pessoaAvalista.EmpresaTrabalho,
                            Cargo = pessoaAvalista.Cargo,
                            DataAdmissao = pessoaAvalista.DataAdmissao,
                            ValorSalario = pessoaAvalista.ValorSalario,
                            DataReferenciaSalario = pessoaAvalista.DataReferenciaSalario,
                            EmpresaPessoal = pessoaAvalista.EmpresaPessoal,
                            TelefoneEmpresa = pessoaAvalista.TelefoneEmpresa,
                            EmailEmpresa = pessoaAvalista.EmailEmpresa,
                            PessoaTelefoneDto = pessoaAvalista.PessoaTelefone.Select(y => new PessoaTelefoneDto()
                            {
                                Id = y.Id,
                                Pais = y.Pais,
                                DDD = y.DDD,
                                Numero = y.Numero,
                                NomeContato = y.NomeContato,
                                IdTipoTelefone = y.IdTipoTelefone
                            }).ToList(),
                            PessoaEmailDto = pessoaAvalista.PessoaEmail.Select(y => new PessoaEmailDto()
                            {
                                Id = y.Id,
                                Email = y.Email,
                                NomeContato = y.NomeContato
                            }).ToList()
                        };
                    }
                }

                foreach (var item in p.PessoaContratoDto)
                {
                    var pessoaCliente = projetoTransportadoraEntities.Pessoa.FirstOrDefault(x => x.Id == item.IdCliente);

                    if (pessoaCliente != null)
                        item.PessoaClienteDto = new PessoaDto() { Id = pessoaCliente.Id, Nome = pessoaCliente.Nome, IdTipoPessoa = pessoaCliente.IdTipoPessoa, Cpf = pessoaCliente.Cpf, Cnpj = pessoaCliente.Cnpj };
                }
            }

            return pessoasDto;
        }

        public int Incluir(PessoaDto pessoaDto)
        {
            var pessoa = new Pessoa()
            {
                Nome = pessoaDto.Nome,
                IdEstadoCivil = pessoaDto.IdEstadoCivil,
                Rg = pessoaDto.Rg,
                Cpf = pessoaDto.Cpf,
                DataNascimento = pessoaDto.DataNascimento ?? null,
                CidadeNascimento = pessoaDto.CidadeNascimento,
                UfNascimento = pessoaDto.UfNascimento,
                CepResidencia = pessoaDto.CepResidencia,
                LogradouroResidencia = pessoaDto.LogradouroResidencia,
                ComplementoResidencia = pessoaDto.ComplementoResidencia,
                NumeroResidencia = pessoaDto.NumeroResidencia,
                BairroResidencia = pessoaDto.BairroResidencia,
                CidadeResidencia = pessoaDto.CidadeResidencia,
                UfResidencia = pessoaDto.UfResidencia,
                Profissao = pessoaDto.Profissao,
                IdTipoResidencia = pessoaDto.IdTipoResidencia,
                TempoResidencial = pessoaDto.TempoResidencial,
                ValorAluguel = pessoaDto.ValorAluguel,
                IdRegimeCasamento = pessoaDto.IdRegimeCasamento,
                DataAdmissao = pessoaDto.DataAdmissao ?? null,
                Cargo = pessoaDto.Cargo,
                ValorSalario = pessoaDto.ValorSalario,
                DataReferenciaSalario = pessoaDto.DataReferenciaSalario ?? null,
                Cnpj = pessoaDto.Cnpj,
                IdProduto = pessoaDto.IdProduto,
                DataAbertura = pessoaDto.DataAbertura ?? null,
                Ativo = true,
                IdUsuarioCadastro = pessoaDto.IdUsuarioCadastro,
                DataCadastro = pessoaDto.DataCadastro,
                IdTipoPessoa = pessoaDto.IdTipoPessoa,
                IdPai = pessoaDto.IdPai,
                IdMae = pessoaDto.IdMae,
                IdConjuge = pessoaDto.IdConjuge,
                IdProprietario = pessoaDto.IdProprietario,
                EmpresaPessoal = pessoaDto.EmpresaPessoal,
                EmpresaTrabalho = pessoaDto.EmpresaTrabalho,
                TelefoneEmpresa = pessoaDto.TelefoneEmpresa,
                EmailEmpresa = pessoaDto.EmailEmpresa
            };

            projetoTransportadoraEntities.Pessoa.Add(pessoa);
            projetoTransportadoraEntities.SaveChanges();

            return pessoa.Id;
        }

        public void AlterarStatus(PessoaDto pessoaDto)
        {
            var pessoa = projetoTransportadoraEntities.Pessoa.FirstOrDefault(x => x.Id == pessoaDto.Id);

            if (pessoaDto.Ativo.HasValue)
            {
                pessoa.Ativo = pessoaDto.Ativo.Value;
                pessoa.IdUsuarioInativacao = pessoaDto.IdUsuarioInativacao;
                pessoa.DataInativacao = pessoaDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(pessoa).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }

        public void Alterar(PessoaDto pessoaDto)
        {
            var pessoa = projetoTransportadoraEntities.Pessoa.FirstOrDefault(x => x.Id == pessoaDto.Id);

            pessoa.Nome = pessoaDto.Nome;
            pessoa.IdEstadoCivil = pessoaDto.IdEstadoCivil;
            pessoa.Rg = pessoaDto.Rg;
            pessoa.Cpf = pessoaDto.Cpf;
            pessoa.DataNascimento = pessoaDto.DataNascimento ?? null;
            pessoa.CidadeNascimento = pessoaDto.CidadeNascimento;
            pessoa.UfNascimento = pessoaDto.UfNascimento;
            pessoa.CepResidencia = pessoaDto.CepResidencia;
            pessoa.LogradouroResidencia = pessoaDto.LogradouroResidencia;
            pessoa.ComplementoResidencia = pessoaDto.ComplementoResidencia;
            pessoa.NumeroResidencia = pessoaDto.NumeroResidencia;
            pessoa.BairroResidencia = pessoaDto.BairroResidencia;
            pessoa.CidadeResidencia = pessoaDto.CidadeResidencia;
            pessoa.UfResidencia = pessoaDto.UfResidencia;
            pessoa.Profissao = pessoaDto.Profissao;
            pessoa.IdTipoResidencia = pessoaDto.IdTipoResidencia;
            pessoa.TempoResidencial = pessoaDto.TempoResidencial;
            pessoa.ValorAluguel = pessoaDto.ValorAluguel;
            pessoa.IdRegimeCasamento = pessoaDto.IdRegimeCasamento;
            pessoa.DataAdmissao = pessoaDto.DataAdmissao ?? null;
            pessoa.Cargo = pessoaDto.Cargo;
            pessoa.ValorSalario = pessoaDto.ValorSalario;
            pessoa.DataReferenciaSalario = pessoaDto.DataReferenciaSalario ?? null;
            pessoa.Cnpj = pessoaDto.Cnpj;
            pessoa.IdProduto = pessoaDto.IdProduto;
            pessoa.DataAbertura = pessoaDto.DataAbertura ?? null;
            pessoa.IdPai = pessoaDto.IdPai;
            pessoa.IdMae = pessoaDto.IdMae;
            pessoa.IdConjuge = pessoaDto.IdConjuge;
            pessoa.IdProprietario = pessoaDto.IdProprietario;
            pessoa.EmpresaPessoal = pessoaDto.EmpresaPessoal;
            pessoa.EmpresaTrabalho = pessoaDto.EmpresaTrabalho;
            pessoa.TelefoneEmpresa = pessoaDto.TelefoneEmpresa;
            pessoa.EmailEmpresa = pessoaDto.EmailEmpresa;

            if (pessoaDto.Ativo.HasValue)
            {
                pessoa.Ativo = pessoaDto.Ativo.Value;
                pessoa.IdUsuarioInativacao = pessoaDto.IdUsuarioInativacao;
                pessoa.DataInativacao = pessoaDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(pessoa).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
