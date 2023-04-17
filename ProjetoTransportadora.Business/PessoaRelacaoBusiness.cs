using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;

namespace ProjetoTransportadora.Business
{
    public class PessoaRelacaoBusiness
    {
        PessoaRelacaoRepository pessoaRelacaoRepository;
        public PessoaRelacaoBusiness()
        {
            pessoaRelacaoRepository = new PessoaRelacaoRepository();
        }

        //public int Incluir(PessoaRelacaoDto pessoaRelacaoDto)
        //{
        //    var idPessoaRelacao = 0;

        //    if (pessoaRelacaoDto == null)
        //        throw new BusinessException("PessoaRelacaoDto é nulo");

        //    if (pessoaRelacaoDto.IdPessoa <= 0)
        //        throw new BusinessException("IdPessoa é obrigatório");

        //    if (pessoaRelacaoDto.IdTipoRelacao <= 0)
        //        throw new BusinessException("IdTipoRelacao é obrigatório");

        //    idPessoaRelacao = pessoaRelacaoRepository.Incluir(pessoaRelacaoDto);

        //    return idPessoaRelacao;
        //}

        //public void Excluir(int idPessoa)
        //{
        //    if (idPessoa <= 0)
        //        throw new BusinessException("idPessoa é obrigatório");

        //    pessoaRelacaoRepository.Excluir(idPessoa);
        //}
    }
}
