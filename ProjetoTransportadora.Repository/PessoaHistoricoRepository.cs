using ProjetoTransportadora.Dto;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class PessoaHistoricoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public PessoaHistoricoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public int Incluir(PessoaHistoricoDto pessoaHistoricoDto)
        {
            var pessoaHistorico = new PessoaHistorico()
            {
                IdPessoa = pessoaHistoricoDto.IdPessoa,
                DataHistorico = pessoaHistoricoDto.DataHistorico,
                Descricao = pessoaHistoricoDto.Descricao
            };

            projetoTransportadoraEntities.PessoaHistorico.Add(pessoaHistorico);
            projetoTransportadoraEntities.SaveChanges();

            return pessoaHistorico.Id;
        }

        public void Excluir(int idPessoa)
        {
            var pessoaHistorico = projetoTransportadoraEntities.PessoaHistorico.Where(x => x.IdPessoa == idPessoa);

            projetoTransportadoraEntities.PessoaHistorico.RemoveRange(pessoaHistorico);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
