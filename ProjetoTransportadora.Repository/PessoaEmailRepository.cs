using ProjetoTransportadora.Dto;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class PessoaEmailRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public PessoaEmailRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public bool Existe(PessoaEmailDto pessoaEmailDto)
        {
            IQueryable<PessoaEmail> query = projetoTransportadoraEntities.PessoaEmail;

            if (pessoaEmailDto.Id > 0)
                query = query.Where(x => x.Id == pessoaEmailDto.Id);

            if (!string.IsNullOrEmpty(pessoaEmailDto.Email))
                query = query.Where(x => x.Email == pessoaEmailDto.Email);

            if (pessoaEmailDto.IdPessoa > 0)
                query = query.Where(x => x.IdPessoa == pessoaEmailDto.IdPessoa);

            return query.FirstOrDefault() != null ? true : false;
        }

        public int Incluir(PessoaEmailDto pessoaEmailDto)
        {
            var pessoaEmail = new PessoaEmail()
            {
                IdPessoa = pessoaEmailDto.IdPessoa,
                Email = pessoaEmailDto.Email,
                NomeContato = pessoaEmailDto.NomeContato
            };

            projetoTransportadoraEntities.PessoaEmail.Add(pessoaEmail);
            projetoTransportadoraEntities.SaveChanges();

            return pessoaEmail.Id;
        }

        public void Alterar(PessoaEmailDto pessoaEmailDto)
        {
            var pessoaEmail = projetoTransportadoraEntities.PessoaEmail.FirstOrDefault(x => x.Id == pessoaEmailDto.Id);

            pessoaEmail.Email = pessoaEmailDto.Email;
            pessoaEmail.NomeContato = pessoaEmailDto.NomeContato;

            projetoTransportadoraEntities.Entry(pessoaEmail).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }

        public void Excluir(int idPessoa)
        {
            var pessoaEmail = projetoTransportadoraEntities.PessoaEmail.Where(x => x.IdPessoa == idPessoa);

            projetoTransportadoraEntities.PessoaEmail.RemoveRange(pessoaEmail);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
