using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;

namespace ProjetoTransportadora.Business
{
    public class PessoaAvalistaBusiness
    {
        PessoaAvalistaRepository pessoaAvalistaRepository;
        public PessoaAvalistaBusiness()
        {
            pessoaAvalistaRepository = new PessoaAvalistaRepository();
        }

        public int Incluir(PessoaAvalistaDto pessoaAvalistaDto)
        {
            var idPessoaAvalista = 0;

            if (pessoaAvalistaDto == null)
                throw new BusinessException("PessoaAvalistaDto é nulo");

            if (pessoaAvalistaDto.IdPessoa <= 0)
                throw new BusinessException("IdPessoa é obrigatório");

            var pessoaAvalistaExistePorAvalista = pessoaAvalistaRepository.Existe(new PessoaAvalistaDto() { IdPessoa = pessoaAvalistaDto.IdPessoa, IdAvalista = pessoaAvalistaDto.IdAvalista });

            if (pessoaAvalistaExistePorAvalista)
                throw new BusinessException($"Avalista com Id ({pessoaAvalistaDto.IdAvalista}) já está cadastrado para a pessoa ${pessoaAvalistaDto.IdPessoa}");

            idPessoaAvalista = pessoaAvalistaRepository.Incluir(pessoaAvalistaDto);

            return idPessoaAvalista;
        }

        public void Alterar(PessoaAvalistaDto pessoaAvalistaDto)
        {
            if (pessoaAvalistaDto == null)
                throw new BusinessException("PessoaAvalistaDto é nulo");

            if (pessoaAvalistaDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var pessoaAvalistaExistePorId = pessoaAvalistaRepository.Existe(new PessoaAvalistaDto() { Id = pessoaAvalistaDto.Id });

            if (!pessoaAvalistaExistePorId)
                throw new BusinessException($"Pessoa com Id {pessoaAvalistaDto.Id} não está cadastrada");

            pessoaAvalistaRepository.Alterar(pessoaAvalistaDto);
        }

        public void Excluir(int idPessoa)
        {
            if (idPessoa <= 0)
                throw new BusinessException("idPessoa é obrigatório");

            pessoaAvalistaRepository.Excluir(idPessoa);
        }
    }
}
