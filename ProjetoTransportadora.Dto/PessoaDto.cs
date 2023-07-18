using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class PessoaDto
    {
        public PessoaDto()
        {
            PessoaEmailDto = new List<PessoaEmailDto>();
            PessoaTelefoneDto = new List<PessoaTelefoneDto>();
            PessoaHistoricoDto = new List<PessoaHistoricoDto>();
            PessoaReferenciaDto = new List<PessoaReferenciaDto>();
            PessoaAvalistaDto = new List<PessoaAvalistaDto>();
            PessoaContratoDto = new List<ContratoDto>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int? IdEstadoCivil { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string CidadeNascimento { get; set; }
        public string UfNascimento { get; set; }
        public string CepResidencia { get; set; }
        public string LogradouroResidencia { get; set; }
        public string ComplementoResidencia { get; set; }
        public int? NumeroResidencia { get; set; }
        public string BairroResidencia { get; set; }
        public string CidadeResidencia { get; set; }
        public string UfResidencia { get; set; }
        public string Profissao { get; set; }
        public int? IdTipoResidencia { get; set; }
        public int? TempoResidencial { get; set; }
        public double? ValorAluguel { get; set; }
        public int? IdRegimeCasamento { get; set; }
        public string EmpresaPessoal { get; set; }
        public string EmpresaTrabalho { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public string Cargo { get; set; }
        public double? ValorSalario { get; set; }
        public DateTime? DataReferenciaSalario { get; set; }
        public string Cnpj { get; set; }
        public int? IdProduto { get; set; }
        public DateTime? DataAbertura { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public int? IdUsuarioInativacao { get; set; }
        public DateTime? DataInativacao { get; set; }
        public bool? Ativo { get; set; }
        public int IdTipoPessoa { get; set; }
        public int? IdPai { get; set; }
        public int? IdMae { get; set; }
        public int? IdConjuge { get; set; }
        public int? IdProprietario { get; set; }
        public bool SemVinculo { get; set; }
        public string TelefoneEmpresa { get; set; }
        public string EmailEmpresa { get; set; }
        public virtual EstadoCivilDto EstadoCivilDto { get; set; }
        public virtual PessoaDto PessoaPaiDto { get; set; }
        public virtual PessoaDto PessoaMaeDto { get; set; }
        public virtual PessoaDto PessoaConjugeDto { get; set; }
        public virtual PessoaDto PessoaProprietarioDto { get; set; }
        public virtual ProdutoDto ProdutoDto { get; set; }        
        public virtual RegimeCasamentoDto RegimeCasamentoDto { get; set; }
        public virtual TipoResidenciaDto TipoResidenciaDto { get; set; }
        public virtual TipoPessoaDto TipoPessoaDto { get; set; }
        public virtual List<PessoaEmailDto> PessoaEmailDto { get; set; }
        public virtual List<PessoaTelefoneDto> PessoaTelefoneDto { get; set; }
        public virtual List<PessoaHistoricoDto> PessoaHistoricoDto { get; set; }
        public virtual List<PessoaReferenciaDto> PessoaReferenciaDto { get; set; }
        public virtual List<PessoaAvalistaDto> PessoaAvalistaDto { get; set; }
        public virtual List<ContratoDto> PessoaContratoDto { get; set; }
    }
}
