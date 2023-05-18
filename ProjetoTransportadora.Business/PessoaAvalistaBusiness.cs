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

            if (pessoaAvalistaDto.IdAvalista <= 0)
                throw new BusinessException("IdAvalista é obrigatório");

            if (pessoaAvalistaDto.IdPessoa == pessoaAvalistaDto.IdAvalista)
                throw new BusinessException("Avalista não pode ser a própria Pessoa");

            var pessoaAvalistaExistePorAvalista = pessoaAvalistaRepository.Existe(new PessoaAvalistaDto() { IdPessoa = pessoaAvalistaDto.IdPessoa, IdAvalista = pessoaAvalistaDto.IdAvalista });

            if (pessoaAvalistaExistePorAvalista)
                throw new BusinessException("Avalista duplicado");

            idPessoaAvalista = pessoaAvalistaRepository.Incluir(pessoaAvalistaDto);

            return idPessoaAvalista;
        }

        public void Excluir(int idPessoa)
        {
            if (idPessoa <= 0)
                throw new BusinessException("idPessoa é obrigatório");

            pessoaAvalistaRepository.Excluir(idPessoa);
        }
    }
}
