using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoTransportadora.Dto
{
    public class VeiculoDto
    {
        public int Id { get; set; }
        public string IdMontadora { get; set; }
        public string Modelo { get; set; }
        public int AnoFabricacao { get; set; }
        public int AnoModelo { get; set; }
        public string Cor { get; set; }
        public string Placa { get; set; }
        public PessoaDto ProprietarioAtual { get; set; }
        public PessoaDto ProprietarioAnterior { get; set; }
        public string Renavam { get; set; }
        public string Chassi { get; set; }
        public DateTime DataAquisicao { get; set; }
        public decimal ValorAquisicao { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal ValorVenda { get; set; }
        public DateTime DataRecuperacao { get; set; }
        public DateTime DataValorFipe { get; set; }
        public decimal ValorFipe { get; set; }
        public decimal ValorTransportadora { get; set; }
        public string Implemento { get; set; }
        public int Comprimento { get; set; }
        public int Altura { get; set; }
        public int Largura { get; set; }
        public string Rastreador { get; set; }
        public int IdSituacaoVeiculo { get; set; }
    }
}
