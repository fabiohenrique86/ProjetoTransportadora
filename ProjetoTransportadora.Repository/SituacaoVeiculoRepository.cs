using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class SituacaoVeiculoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public SituacaoVeiculoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public SituacaoVeiculoDto Obter(SituacaoVeiculoDto situacaoVeiculoDto)
        {
            IQueryable<SituacaoVeiculo> query = projetoTransportadoraEntities.SituacaoVeiculo;

            if (situacaoVeiculoDto.Id > 0)
                query = query.Where(x => x.Id == situacaoVeiculoDto.Id);

            if (!string.IsNullOrEmpty(situacaoVeiculoDto.Nome))
                query = query.Where(x => x.Nome == situacaoVeiculoDto.Nome);

            return query.Select(x => new SituacaoVeiculoDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(SituacaoVeiculoDto situacaoVeiculoDto)
        {
            IQueryable<SituacaoVeiculo> query = projetoTransportadoraEntities.SituacaoVeiculo;

            if (situacaoVeiculoDto.Id > 0)
                query = query.Where(x => x.Id == situacaoVeiculoDto.Id);

            if (!string.IsNullOrEmpty(situacaoVeiculoDto.Nome))
                query = query.Where(x => x.Nome == situacaoVeiculoDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<SituacaoVeiculoDto> Listar(SituacaoVeiculoDto situacaoVeiculoDto = null)
        {
            IQueryable<SituacaoVeiculo> query = projetoTransportadoraEntities.SituacaoVeiculo;

            if (situacaoVeiculoDto != null)
            {

                if (situacaoVeiculoDto.Id > 0)
                    query = query.Where(x => x.Id == situacaoVeiculoDto.Id);

                if (!string.IsNullOrEmpty(situacaoVeiculoDto.Nome))
                    query = query.Where(x => x.Nome.Contains(situacaoVeiculoDto.Nome));

                if (situacaoVeiculoDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == situacaoVeiculoDto.Ativo.Value);
            }

            return query.Select(x => new SituacaoVeiculoDto()
            {
                Id = x.Id,
                Ativo = x.Ativo,
                Nome = x.Nome,
                DataCadastro = x.DataCadastro,
                IdUsuarioCadastro = x.IdUsuarioCadastro,
                DataInativacao = x.DataInativacao,
                IdUsuarioInativacao = x.IdUsuarioInativacao
            }).OrderBy(x => x.Nome).ToList();
        }

        public int Incluir(SituacaoVeiculoDto situacaoVeiculoDto)
        {
            var SituacaoVeiculo = new SituacaoVeiculo()
            {
                Nome = situacaoVeiculoDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = situacaoVeiculoDto.IdUsuarioCadastro,
                DataCadastro = situacaoVeiculoDto.DataCadastro
            };

            projetoTransportadoraEntities.SituacaoVeiculo.Add(SituacaoVeiculo);
            projetoTransportadoraEntities.SaveChanges();

            return SituacaoVeiculo.Id;
        }

        public void Alterar(SituacaoVeiculoDto situacaoVeiculoDto)
        {
            var situacaoVeiculo = projetoTransportadoraEntities.SituacaoVeiculo.FirstOrDefault(x => x.Id == situacaoVeiculoDto.Id);

            if (!string.IsNullOrEmpty(situacaoVeiculoDto.Nome))
                situacaoVeiculo.Nome = situacaoVeiculoDto.Nome;

            if (situacaoVeiculoDto.Ativo.HasValue)
            {
                situacaoVeiculo.Ativo = situacaoVeiculoDto.Ativo.Value;
                situacaoVeiculo.IdUsuarioInativacao = situacaoVeiculoDto.IdUsuarioInativacao;
                situacaoVeiculo.DataInativacao = situacaoVeiculoDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(situacaoVeiculo).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
