using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;

namespace ProjetoTransportadora.Business
{
    public class PessoaReferenciaBusiness
    {
        PessoaReferenciaRepository pessoaReferenciaRepository;
        public PessoaReferenciaBusiness()
        {
            pessoaReferenciaRepository = new PessoaReferenciaRepository();
        }

        public int Incluir(PessoaReferenciaDto pessoaReferenciaDto)
        {
            var idPessoaReferencia = 0;

            if (pessoaReferenciaDto == null)
                throw new BusinessException("PessoaReferenciaDto é nulo");

            if (pessoaReferenciaDto.IdPessoa <= 0)
                throw new BusinessException("IdPessoa é obrigatório");

            if (pessoaReferenciaDto.IdTipoReferencia <= 0)
                throw new BusinessException("IdTipoReferencia é obrigatório");

            if (pessoaReferenciaDto.DataReferencia == DateTime.MinValue)
                throw new BusinessException("Data da Referência é obrigatório");

            if (string.IsNullOrEmpty(pessoaReferenciaDto.Descricao))
                throw new BusinessException("Descrição é obrigatório");

            idPessoaReferencia = pessoaReferenciaRepository.Incluir(pessoaReferenciaDto);

            return idPessoaReferencia;
        }

        public void Excluir(int idPessoa)
        {
            if (idPessoa <= 0)
                throw new BusinessException("idPessoa é obrigatório");

            pessoaReferenciaRepository.Excluir(idPessoa);
        }
    }
}
