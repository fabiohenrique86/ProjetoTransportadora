﻿using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class PessoaDto
    {
        public PessoaDto()
        {
            this.PessoaEmailDto = new List<PessoaEmailDto>();
            this.PessoaTelefoneDto = new List<PessoaTelefoneDto>();
            this.PessoaHistoricoDto = new List<PessoaHistoricoDto>();
            this.PessoaReferenciaDto = new List<PessoaReferenciaDto>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public Nullable<int> IdEstadoCivil { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public Nullable<System.DateTime> DataNascimento { get; set; }
        public string CidadeNascimento { get; set; }
        public string UfNascimento { get; set; }
        public string CepResidencia { get; set; }
        public string LogradouroResidencia { get; set; }
        public string ComplementoResidencia { get; set; }
        public Nullable<int> NumeroResidencia { get; set; }
        public string BairroResidencia { get; set; }
        public string CidadeResidencia { get; set; }
        public string UfResidencia { get; set; }
        public string Profissao { get; set; }
        public Nullable<int> IdTipoResidencia { get; set; }
        public Nullable<int> TempoResidencial { get; set; }
        public Nullable<double> ValorAluguel { get; set; }
        public Nullable<int> IdRegimeCasamento { get; set; }
        public string EmpresaPessoal { get; set; }
        public string EmpresaTrabalho { get; set; }
        public Nullable<System.DateTime> DataAdmissao { get; set; }
        public string Cargo { get; set; }
        public Nullable<double> ValorSalario { get; set; }
        public Nullable<System.DateTime> DataReferenciaSalario { get; set; }
        public Nullable<double> ValorFrete { get; set; }
        public string Cnpj { get; set; }
        public Nullable<int> IdProduto { get; set; }
        public Nullable<System.DateTime> DataAbertura { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<int> IdUsuarioInativacao { get; set; }
        public Nullable<System.DateTime> DataInativacao { get; set; }
        public bool? Ativo { get; set; }
        public int IdTipoPessoa { get; set; }
        public Nullable<int> IdPai { get; set; }
        public Nullable<int> IdMae { get; set; }
        public Nullable<int> IdConjuge { get; set; }
        public Nullable<int> IdProprietario { get; set; }
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
    }
}
