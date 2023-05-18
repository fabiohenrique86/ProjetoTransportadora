using ProjetoTransportadora.Dto;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class PessoaAvalistaRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public PessoaAvalistaRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public bool Existe(PessoaAvalistaDto pessoaAvalistaDto)
        {
            IQueryable<PessoaAvalista> query = projetoTransportadoraEntities.PessoaAvalista;

            if (pessoaAvalistaDto.Id > 0)
                query = query.Where(x => x.Id == pessoaAvalistaDto.Id);

            if (pessoaAvalistaDto.IdPessoa > 0)
                query = query.Where(x => x.IdPessoa == pessoaAvalistaDto.IdPessoa);

            if (pessoaAvalistaDto.IdAvalista > 0)
                query = query.Where(x => x.IdAvalista == pessoaAvalistaDto.IdAvalista);

            return query.FirstOrDefault() != null ? true : false;
        }

        public int Incluir(PessoaAvalistaDto pessoaAvalistaDto)
        {
            var pessoaAvalista = new PessoaAvalista()
            {
                IdPessoa = pessoaAvalistaDto.IdPessoa,
                IdAvalista = pessoaAvalistaDto.IdAvalista
            };

            projetoTransportadoraEntities.PessoaAvalista.Add(pessoaAvalista);
            projetoTransportadoraEntities.SaveChanges();

            return pessoaAvalista.Id;
        }

        public void Excluir(int idPessoa)
        {
            var pessoaAvalista = projetoTransportadoraEntities.PessoaAvalista.Where(x => x.IdPessoa == idPessoa);

            projetoTransportadoraEntities.PessoaAvalista.RemoveRange(pessoaAvalista);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
