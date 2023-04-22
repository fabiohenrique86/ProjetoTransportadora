using ProjetoTransportadora.Dto;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class VeiculoHistoricoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public VeiculoHistoricoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public int Incluir(VeiculoHistoricoDto veiculoHistoricoDto)
        {
            var veiculoHistorico = new VeiculoHistorico()
            {
                IdVeiculo = veiculoHistoricoDto.IdVeiculo,
                DataHistorico = veiculoHistoricoDto.DataHistorico,
                Descricao = veiculoHistoricoDto.Descricao,
                IdUsuarioCadastro = veiculoHistoricoDto.IdUsuarioCadastro,
                DataCadastro = veiculoHistoricoDto.DataCadastro
            };

            projetoTransportadoraEntities.VeiculoHistorico.Add(veiculoHistorico);
            projetoTransportadoraEntities.SaveChanges();

            return veiculoHistorico.Id;
        }

        public void Excluir(int idVeiculo)
        {
            var veiculoHistorico = projetoTransportadoraEntities.VeiculoHistorico.Where(x => x.IdVeiculo == idVeiculo);

            projetoTransportadoraEntities.VeiculoHistorico.RemoveRange(veiculoHistorico);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
