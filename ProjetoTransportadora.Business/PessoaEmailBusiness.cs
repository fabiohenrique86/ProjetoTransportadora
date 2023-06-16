using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;

namespace ProjetoTransportadora.Business
{
    public class PessoaEmailBusiness
    {
        PessoaEmailRepository pessoaEmailRepository;
        public PessoaEmailBusiness()
        {
            pessoaEmailRepository = new PessoaEmailRepository();
        }

        public int Incluir(PessoaEmailDto pessoaEmailDto)
        {
            var idPessoaEmail = 0;

            if (pessoaEmailDto == null)
                throw new BusinessException("PessoaEmailDto é nulo");

            if (pessoaEmailDto.IdPessoa <= 0)
                throw new BusinessException("IdPessoa é obrigatório");

            if (string.IsNullOrEmpty(pessoaEmailDto.Email))
                throw new BusinessException("E-mail é obrigatório");

            //if (string.IsNullOrEmpty(pessoaEmailDto.NomeContato))
            //    throw new BusinessException("Nome do Contato é obrigatório");

            var pessoaEmailExistePorEmail = pessoaEmailRepository.Existe(new PessoaEmailDto() { IdPessoa = pessoaEmailDto.IdPessoa, Email = pessoaEmailDto.Email });

            if (pessoaEmailExistePorEmail)
                throw new BusinessException($"Pessoa com e-mail ({pessoaEmailDto.Email}) já está cadastrada");

            idPessoaEmail = pessoaEmailRepository.Incluir(pessoaEmailDto);

            return idPessoaEmail;
        }

        public void Alterar(PessoaEmailDto pessoaEmailDto)
        {
            if (pessoaEmailDto == null)
                throw new BusinessException("PessoaEmailDto é nulo");

            if (pessoaEmailDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var pessoaEmailExistePorId = pessoaEmailRepository.Existe(new PessoaEmailDto() { Id = pessoaEmailDto.Id });

            if (!pessoaEmailExistePorId)
                throw new BusinessException($"Pessoa com Id {pessoaEmailDto.Id} não está cadastrada");

            pessoaEmailRepository.Alterar(pessoaEmailDto);
        }

        public void Excluir(int idPessoa)
        {
            if (idPessoa <= 0)
                throw new BusinessException("idPessoa é obrigatório");

            pessoaEmailRepository.Excluir(idPessoa);
        }
    }
}
