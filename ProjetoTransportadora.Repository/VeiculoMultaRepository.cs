using ProjetoTransportadora.Dto;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class VeiculoMultaRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public VeiculoMultaRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public int Incluir(VeiculoMultaDto veiculoMultaDto)
        {
            var veiculoMulta = new VeiculoMulta()
            {
                IdVeiculo = veiculoMultaDto.IdVeiculo,
                DataMulta = veiculoMultaDto.DataMulta,
                Local = veiculoMultaDto.Local,
                IdCondutor = veiculoMultaDto.IdCondutor,
                DataVencimentoMulta = veiculoMultaDto.DataVencimentoMulta,
                ValorMulta = veiculoMultaDto.ValorMulta,
                IdSituacaoMulta = veiculoMultaDto.IdSituacaoMulta,
                IdUsuarioCadastro = veiculoMultaDto.IdUsuarioCadastro,
                DataCadastro = veiculoMultaDto.DataCadastro
            };

            projetoTransportadoraEntities.VeiculoMulta.Add(veiculoMulta);
            projetoTransportadoraEntities.SaveChanges();

            return veiculoMulta.Id;
        }

        public void Excluir(int idVeiculo)
        {
            var veiculoMulta = projetoTransportadoraEntities.VeiculoMulta.Where(x => x.IdVeiculo == idVeiculo);

            projetoTransportadoraEntities.VeiculoMulta.RemoveRange(veiculoMulta);

            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
