using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;

namespace ProjetoTransportadora.Business
{
    public class PessoaTelefoneBusiness
    {
        PessoaTelefoneRepository pessoaTelefoneRepository;
        public PessoaTelefoneBusiness()
        {
            pessoaTelefoneRepository = new PessoaTelefoneRepository();
        }

        public int Incluir(PessoaTelefoneDto pessoaTelefoneDto)
        {
            var idPessoaTelefone = 0;

            if (pessoaTelefoneDto == null)
                throw new BusinessException("PessoaTelefoneDto é nulo");

            if (pessoaTelefoneDto.IdPessoa <= 0)
                throw new BusinessException("IdPessoa é obrigatório");

            if (string.IsNullOrEmpty(pessoaTelefoneDto.Pais))
                throw new BusinessException("País é obrigatório");

            if (pessoaTelefoneDto.DDD <= 0)
                throw new BusinessException("DDD é obrigatório");

            if (pessoaTelefoneDto.Numero <= 0)
                throw new BusinessException("Número é obrigatório");

            if (string.IsNullOrEmpty(pessoaTelefoneDto.NomeContato))
                throw new BusinessException("Nome do Contato é obrigatório");

            if (pessoaTelefoneDto.IdTipoTelefone <= 0)
                throw new BusinessException("IdTipoTelefone é obrigatório");

            idPessoaTelefone = pessoaTelefoneRepository.Incluir(pessoaTelefoneDto);

            return idPessoaTelefone;
        }

        public void Alterar(PessoaTelefoneDto pessoaTelefoneDto)
        {
            if (pessoaTelefoneDto == null)
                throw new BusinessException("PessoaTelefoneDto é nulo");

            if (pessoaTelefoneDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var pessoaTelefoneExiste = pessoaTelefoneRepository.Existe(new PessoaTelefoneDto() { Id = pessoaTelefoneDto.Id });

            if (!pessoaTelefoneExiste)
                throw new BusinessException($"Pessoa com Id {pessoaTelefoneDto.Id} não está cadastrada");

            pessoaTelefoneRepository.Alterar(pessoaTelefoneDto);
        }

        public void Excluir(int idPessoa)
        {
            if (idPessoa <= 0)
                throw new BusinessException("idPessoa é obrigatório");

            pessoaTelefoneRepository.Excluir(idPessoa);
        }
    }
}
