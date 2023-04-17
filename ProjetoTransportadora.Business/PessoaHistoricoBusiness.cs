using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;

namespace ProjetoTransportadora.Business
{
    public class PessoaHistoricoBusiness
    {
        PessoaHistoricoRepository pessoaHistoricoRepository;
        public PessoaHistoricoBusiness()
        {
            pessoaHistoricoRepository = new PessoaHistoricoRepository();
        }

        public int Incluir(PessoaHistoricoDto pessoaHistoricoDto)
        {
            var idPessoaHistorico = 0;

            if (pessoaHistoricoDto == null)
                throw new BusinessException("PessoaHistoricoDto é nulo");

            if (pessoaHistoricoDto.IdPessoa <= 0)
                throw new BusinessException("IdPessoa é obrigatório");

            if (pessoaHistoricoDto.DataHistorico == DateTime.MinValue)
                throw new BusinessException("Data Histórico é obrigatório");

            if (string.IsNullOrEmpty(pessoaHistoricoDto.Descricao))
                throw new BusinessException("Descrição Histórico é obrigatório");

            idPessoaHistorico = pessoaHistoricoRepository.Incluir(pessoaHistoricoDto);

            return idPessoaHistorico;
        }

        public void Excluir(int idPessoa)
        {
            if (idPessoa <= 0)
                throw new BusinessException("idPessoa é obrigatório");

            pessoaHistoricoRepository.Excluir(idPessoa);
        }
    }
}
