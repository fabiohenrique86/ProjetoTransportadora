using ProjetoTransportadora.Dto;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class PessoaRelacaoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public PessoaRelacaoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        //public int Incluir(PessoaRelacaoDto pessoaRelacaoDto)
        //{
        //    var pessoaRelacao = new PessoaRelacao()
        //    {
        //        IdPessoa = pessoaRelacaoDto.IdPessoa,
        //        IdTipoRelacao = pessoaRelacaoDto.IdTipoRelacao,
        //        Nome = pessoaRelacaoDto.Nome
        //    };

        //    projetoTransportadoraEntities.PessoaRelacao.Add(pessoaRelacao);
        //    projetoTransportadoraEntities.SaveChanges();

        //    return pessoaRelacao.Id;
        //}

        //public void Excluir(int idPessoa)
        //{
        //    var pessoaRelacao = projetoTransportadoraEntities.PessoaRelacao.Where(x => x.IdPessoa == idPessoa);

        //    projetoTransportadoraEntities.PessoaRelacao.RemoveRange(pessoaRelacao);

        //    projetoTransportadoraEntities.SaveChanges();
        //}
    }
}
