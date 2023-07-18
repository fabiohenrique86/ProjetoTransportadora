using ProjetoTransportadora.Dto;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class PessoaReferenciaRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public PessoaReferenciaRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public int Incluir(PessoaReferenciaDto pessoaReferenciaDto)
        {
            var pessoaReferencia = new PessoaReferencia()
            {
                IdPessoa = pessoaReferenciaDto.IdPessoa,
                IdTipoReferencia = pessoaReferenciaDto.IdTipoReferencia,
                DataReferencia = pessoaReferenciaDto.DataReferencia,
                Descricao = pessoaReferenciaDto.Descricao,
                Telefone = pessoaReferenciaDto.Telefone,
                Nome = pessoaReferenciaDto.Nome
            };

            projetoTransportadoraEntities.PessoaReferencia.Add(pessoaReferencia);
            projetoTransportadoraEntities.SaveChanges();

            return pessoaReferencia.Id;
        }

        public void Excluir(int idPessoa)
        {
            var pessoaReferencia = projetoTransportadoraEntities.PessoaReferencia.Where(x => x.IdPessoa == idPessoa);

            projetoTransportadoraEntities.PessoaReferencia.RemoveRange(pessoaReferencia);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
