using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class VeiculoDto
    {
        public VeiculoDto()
        {
            this.VeiculoHistoricoDto = new List<VeiculoHistoricoDto>();
            this.VeiculoMultaDto = new List<VeiculoMultaDto>();
        }

        public int Id { get; set; }
        public int IdMontadora { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public Nullable<int> AnoFabricacao { get; set; }
        public Nullable<int> AnoModelo { get; set; }
        public string Cor { get; set; }
        public Nullable<int> IdProprietarioAtual { get; set; }
        public Nullable<int> IdProprietarioAnterior { get; set; }
        public string Renavam { get; set; }
        public string Chassi { get; set; }
        public Nullable<System.DateTime> DataAquisicao { get; set; }
        public Nullable<double> ValorAquisicao { get; set; }
        public Nullable<System.DateTime> DataVenda { get; set; }
        public Nullable<double> ValorVenda { get; set; }
        public Nullable<System.DateTime> DataRecuperacao { get; set; }
        public Nullable<System.DateTime> DataValorFIPE { get; set; }
        public Nullable<double> ValorFIPE { get; set; }
        public Nullable<double> ValorTransportadora { get; set; }
        public string Implemento { get; set; }
        public Nullable<double> Comprimento { get; set; }
        public Nullable<double> Altura { get; set; }
        public Nullable<double> Largura { get; set; }
        public string Rastreador { get; set; }
        public Nullable<int> IdSituacaoVeiculo { get; set; }
        public bool? Ativo { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<int> IdUsuarioInativacao { get; set; }
        public Nullable<System.DateTime> DataInativacao { get; set; }
        public virtual MontadoraDto MontadoraDto { get; set; }
        public virtual PessoaDto PessoaProprietarioAnteriorDto { get; set; }
        public virtual PessoaDto PessoaProprietarioAtualDto { get; set; }
        public virtual SituacaoVeiculoDto SituacaoVeiculoDto { get; set; }
        public virtual ICollection<VeiculoHistoricoDto> VeiculoHistoricoDto { get; set; }
        public virtual ICollection<VeiculoMultaDto> VeiculoMultaDto { get; set; }
    }
}