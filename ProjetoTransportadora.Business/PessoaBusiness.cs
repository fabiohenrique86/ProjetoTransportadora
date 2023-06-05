using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace ProjetoTransportadora.Business
{
    public class PessoaBusiness : BaseBusiness
    {
        PessoaRepository pessoaRepository;
        PessoaTelefoneBusiness pessoaTelefoneBusiness;
        PessoaEmailBusiness pessoaEmailBusiness;
        PessoaHistoricoBusiness pessoaHistoricoBusiness;
        PessoaReferenciaBusiness pessoaReferenciaBusiness;
        PessoaAvalistaBusiness pessoaAvalistaBusiness;
        public PessoaBusiness()
        {
            pessoaRepository = new PessoaRepository();
            pessoaTelefoneBusiness = new PessoaTelefoneBusiness();
            pessoaEmailBusiness = new PessoaEmailBusiness();
            pessoaHistoricoBusiness = new PessoaHistoricoBusiness();
            pessoaReferenciaBusiness = new PessoaReferenciaBusiness();
            pessoaAvalistaBusiness = new PessoaAvalistaBusiness();
        }

        public dynamic ListarGrid(PessoaDto pessoaDto = null)
        {
            return pessoaRepository.ListarGrid(pessoaDto);
        }

        public int ListarTotal(PessoaDto pessoaDto = null)
        {
            return pessoaRepository.ListarTotal(pessoaDto);
        }

        public List<PessoaDto> ListarAutoComplete(PessoaDto pessoaDto = null)
        {
            return pessoaRepository.ListarAutoComplete(pessoaDto);
        }

        public bool Existe(PessoaDto pessoaDto)
        {
            return pessoaRepository.Existe(pessoaDto);
        }

        public List<PessoaDto> Listar(PessoaDto pessoaDto = null)
        {
            return pessoaRepository.Listar(pessoaDto);
        }

        public int Incluir(PessoaDto pessoaDto)
        {
            var idPessoa = 0;

            if (pessoaDto == null)
                throw new BusinessException("PessoaDto é nulo");

            if (pessoaDto.IdTipoPessoa <= 0)
                throw new BusinessException("TipoPessoa é obrigatório");

            if (pessoaDto.IdTipoPessoa == (int)TipoPessoaDto.TipoPessoa.PessoaFísica)
            {
                if (string.IsNullOrEmpty(pessoaDto.Nome))
                    throw new BusinessException("Nome é obrigatório");

                if (!pessoaDto.SemVinculo)
                {
                    pessoaDto.Cpf = pessoaDto.Cpf?.Replace(".", "").Replace("-", "").Trim();

                    if (string.IsNullOrEmpty(pessoaDto.Cpf))
                        throw new BusinessException("Cpf é obrigatório");

                    if (pessoaDto.Cpf.Length != 11)
                        throw new BusinessException("Cpf deve ter 11 dígitos");

                    if (!CpfValido(pessoaDto.Cpf))
                        throw new BusinessException("Cpf inválido");

                    var pessoaExistePorCpf = pessoaRepository.Existe(new PessoaDto() { Cpf = pessoaDto.Cpf });

                    if (pessoaExistePorCpf)
                        throw new BusinessException($"Pessoa com CPF ({pessoaDto.Cpf}) já está cadastrada");
                }

                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    idPessoa = pessoaRepository.Incluir(pessoaDto);

                    foreach (var pessoaAvalistaDto in pessoaDto.PessoaAvalistaDto)
                    {
                        pessoaAvalistaDto.IdPessoa = idPessoa;
                        pessoaAvalistaBusiness.Incluir(pessoaAvalistaDto);
                    }

                    foreach (var pessoaTelefoneDto in pessoaDto.PessoaTelefoneDto)
                    {
                        pessoaTelefoneDto.IdPessoa = idPessoa;
                        pessoaTelefoneBusiness.Incluir(pessoaTelefoneDto);
                    }

                    foreach (var pessoaEmailDto in pessoaDto.PessoaEmailDto)
                    {
                        pessoaEmailDto.IdPessoa = idPessoa;
                        pessoaEmailBusiness.Incluir(pessoaEmailDto);
                    }

                    foreach (var pessoaHistoricoDto in pessoaDto.PessoaHistoricoDto)
                    {
                        pessoaHistoricoDto.IdPessoa = idPessoa;
                        pessoaHistoricoBusiness.Incluir(pessoaHistoricoDto);
                    }

                    foreach (var pessoaReferenciaDto in pessoaDto.PessoaReferenciaDto)
                    {
                        pessoaReferenciaDto.IdPessoa = idPessoa;
                        pessoaReferenciaBusiness.Incluir(pessoaReferenciaDto);
                    }

                    transactionScope.Complete();
                }
            }
            else if (pessoaDto.IdTipoPessoa == (int)TipoPessoaDto.TipoPessoa.PessoaJurídica)
            {
                pessoaDto.Cnpj = pessoaDto.Cnpj?.Replace(".", "").Replace("-", "").Replace("/", "");

                if (!pessoaDto.SemVinculo)
                {
                    if (string.IsNullOrEmpty(pessoaDto.Cnpj))
                        throw new BusinessException("Cnpj é obrigatório");

                    if (pessoaDto.Cnpj.Length != 14)
                        throw new BusinessException("Cnpj deve ter 14 dígitos");

                    if (!CnpjValido(pessoaDto.Cnpj))
                        throw new BusinessException("Cnpj inválido");

                    var pessoaExistePorCnpj = pessoaRepository.Existe(new PessoaDto() { Cnpj = pessoaDto.Cnpj });

                    if (pessoaExistePorCnpj)
                        throw new BusinessException($"Pessoa com CNPJ ({pessoaDto.Cnpj}) já está cadastrada");
                }

                if (string.IsNullOrEmpty(pessoaDto.Nome))
                    throw new BusinessException("Nome Empresa é obrigatório");

                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    idPessoa = pessoaRepository.Incluir(pessoaDto);

                    foreach (var pessoaTelefoneDto in pessoaDto.PessoaTelefoneDto)
                    {
                        pessoaTelefoneDto.IdPessoa = idPessoa;
                        pessoaTelefoneBusiness.Incluir(pessoaTelefoneDto);
                    }

                    foreach (var pessoaEmailDto in pessoaDto.PessoaEmailDto)
                    {
                        pessoaEmailDto.IdPessoa = idPessoa;
                        pessoaEmailBusiness.Incluir(pessoaEmailDto);
                    }

                    foreach (var pessoaHistoricoDto in pessoaDto.PessoaHistoricoDto)
                    {
                        pessoaHistoricoDto.IdPessoa = idPessoa;
                        pessoaHistoricoBusiness.Incluir(pessoaHistoricoDto);
                    }

                    transactionScope.Complete();
                }
            }

            return idPessoa;
        }

        public void Alterar(PessoaDto pessoaDto)
        {
            if (pessoaDto == null)
                throw new BusinessException("PessoaDto é nulo");

            if (pessoaDto.Id <= 0)
                throw new BusinessException("IdPessoa é nulo");

            var pessoaExistePorId = pessoaRepository.Obter(new PessoaDto() { Id = pessoaDto.Id });

            if (pessoaExistePorId == null)
                throw new BusinessException($"Pessoa com Id {pessoaDto.Id} não está cadastrada");

            pessoaDto.Cpf = pessoaDto.Cpf?.Replace(".", "").Replace("-", "").Trim();
            pessoaDto.Cnpj = pessoaDto.Cnpj?.Replace(".", "").Replace("-", "").Replace("/", "");

            if (pessoaDto.IdTipoPessoa == (int)TipoPessoaDto.TipoPessoa.PessoaFísica)
            {
                if (pessoaDto.Cpf != pessoaExistePorId.Cpf)
                {
                    if (string.IsNullOrEmpty(pessoaDto.Cpf))
                        throw new BusinessException("Cpf é obrigatório");

                    if (pessoaDto.Cpf.Length != 11)
                        throw new BusinessException("Cpf deve ter 11 dígitos");

                    if (!CpfValido(pessoaDto.Cpf))
                        throw new BusinessException("Cpf inválido");

                    var pessoaExistePorCpf = pessoaRepository.Existe(new PessoaDto() { Cpf = pessoaDto.Cpf });

                    if (pessoaExistePorCpf)
                        throw new BusinessException($"Pessoa com Cpf {pessoaDto.Cpf} já está cadastrada");
                }
            }
            else if (pessoaDto.IdTipoPessoa == (int)TipoPessoaDto.TipoPessoa.PessoaJurídica)
            {
                if (pessoaDto.Cnpj != pessoaExistePorId.Cnpj)
                {
                    if (string.IsNullOrEmpty(pessoaDto.Cnpj))
                        throw new BusinessException("Cnpj é obrigatório");

                    if (pessoaDto.Cnpj.Length != 14)
                        throw new BusinessException("Cnpj deve ter 14 dígitos");

                    if (!CnpjValido(pessoaDto.Cnpj))
                        throw new BusinessException("Cnpj inválido");

                    var pessoaExistePorCnpj = pessoaRepository.Existe(new PessoaDto() { Cnpj = pessoaDto.Cnpj });

                    if (pessoaExistePorCnpj)
                        throw new BusinessException($"Pessoa com Cnpj {pessoaDto.Cnpj} já está cadastrada");
                }
            }
            else
            {
                throw new BusinessException("IdTipoPessoa é obrigatório");
            }

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                pessoaRepository.Alterar(pessoaDto);

                pessoaAvalistaBusiness.Excluir(pessoaDto.Id);
                foreach (var pessoaAvalistaDto in pessoaDto.PessoaAvalistaDto)
                    pessoaAvalistaBusiness.Incluir(pessoaAvalistaDto);

                pessoaTelefoneBusiness.Excluir(pessoaDto.Id);
                foreach (var pessoaTelefoneDto in pessoaDto.PessoaTelefoneDto)
                    pessoaTelefoneBusiness.Incluir(pessoaTelefoneDto);

                pessoaEmailBusiness.Excluir(pessoaDto.Id);
                foreach (var pessoaEmailDto in pessoaDto.PessoaEmailDto)
                    pessoaEmailBusiness.Incluir(pessoaEmailDto);

                pessoaHistoricoBusiness.Excluir(pessoaDto.Id);
                foreach (var pessoaHistoricoDto in pessoaDto.PessoaHistoricoDto)
                    pessoaHistoricoBusiness.Incluir(pessoaHistoricoDto);

                pessoaReferenciaBusiness.Excluir(pessoaDto.Id);
                foreach (var pessoaReferenciaDto in pessoaDto.PessoaReferenciaDto)
                    pessoaReferenciaBusiness.Incluir(pessoaReferenciaDto);

                transactionScope.Complete();
            }
        }

        public void AlterarStatus(PessoaDto pessoaDto)
        {
            if (pessoaDto == null)
                throw new BusinessException("PessoaDto é nulo");

            if (pessoaDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!pessoaDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existePessoa = pessoaRepository.Existe(new PessoaDto() { Id = pessoaDto.Id });

            if (!existePessoa)
                throw new BusinessException($"Pessoa Id ({pessoaDto.Id}) não está cadastrada");

            if (pessoaDto.Ativo.HasValue)
            {
                if (!pessoaDto.Ativo.Value)
                {
                    if (pessoaDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (pessoaDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            pessoaRepository.AlterarStatus(pessoaDto);
        }
    }
}
