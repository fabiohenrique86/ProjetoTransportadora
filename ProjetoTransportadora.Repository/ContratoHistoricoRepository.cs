using ProjetoTransportadora.Dto;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class ContratoHistoricoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public ContratoHistoricoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public int Incluir(ContratoHistoricoDto contratoHistoricoDto)
        {
            var contratoHistorico = new ContratoHistorico()
            {
                IdContrato = contratoHistoricoDto.IdContrato,
                DataHistorico = contratoHistoricoDto.DataHistorico,
                Descricao = contratoHistoricoDto.Descricao,
                IdUsuarioCadastro = contratoHistoricoDto.IdUsuarioCadastro,
                DataCadastro = contratoHistoricoDto.DataCadastro
            };

            projetoTransportadoraEntities.ContratoHistorico.Add(contratoHistorico);
            projetoTransportadoraEntities.SaveChanges();

            return contratoHistorico.Id;
        }

        public void Excluir(int idContrato)
        {
            var contratoHistorico = projetoTransportadoraEntities.ContratoHistorico.Where(x => x.IdContrato == idContrato);

            projetoTransportadoraEntities.ContratoHistorico.RemoveRange(contratoHistorico);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
