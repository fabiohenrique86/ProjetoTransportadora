using ProjetoTransportadora.Dto;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class ContratoParcelaHistoricoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public ContratoParcelaHistoricoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public int Incluir(ContratoParcelaHistoricoDto contratoParcelaHistoricoDto)
        {
            var contratoParcelaHistorico = new ContratoParcelaHistorico()
            {
                IdContratoParcela = contratoParcelaHistoricoDto.IdContratoParcela,
                DataHistorico = contratoParcelaHistoricoDto.DataHistorico,
                Descricao = contratoParcelaHistoricoDto.Descricao,
                IdUsuarioCadastro = contratoParcelaHistoricoDto.IdUsuarioCadastro,
                DataCadastro = contratoParcelaHistoricoDto.DataCadastro
            };

            projetoTransportadoraEntities.ContratoParcelaHistorico.Add(contratoParcelaHistorico);
            projetoTransportadoraEntities.SaveChanges();

            return contratoParcelaHistorico.Id;
        }

        public void Excluir(int idContratoParcela)
        {
            var contratoParcelaHistorico = projetoTransportadoraEntities.ContratoParcelaHistorico.Where(x => x.IdContratoParcela == idContratoParcela);

            projetoTransportadoraEntities.ContratoParcelaHistorico.RemoveRange(contratoParcelaHistorico);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
