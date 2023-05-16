using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class SituacaoContratoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public SituacaoContratoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public SituacaoContratoDto Obter(SituacaoContratoDto situacaoContratoDto)
        {
            IQueryable<SituacaoContrato> query = projetoTransportadoraEntities.SituacaoContrato;

            if (situacaoContratoDto.Id > 0)
                query = query.Where(x => x.Id == situacaoContratoDto.Id);

            if (!string.IsNullOrEmpty(situacaoContratoDto.Nome))
                query = query.Where(x => x.Nome == situacaoContratoDto.Nome);

            return query.Select(x => new SituacaoContratoDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(SituacaoContratoDto situacaoContratoDto)
        {
            IQueryable<SituacaoContrato> query = projetoTransportadoraEntities.SituacaoContrato;

            if (situacaoContratoDto.Id > 0)
                query = query.Where(x => x.Id == situacaoContratoDto.Id);

            if (!string.IsNullOrEmpty(situacaoContratoDto.Nome))
                query = query.Where(x => x.Nome == situacaoContratoDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<SituacaoContratoDto> Listar(SituacaoContratoDto situacaoContratoDto = null)
        {
            IQueryable<SituacaoContrato> query = projetoTransportadoraEntities.SituacaoContrato;

            if (situacaoContratoDto != null)
            {

                if (situacaoContratoDto.Id > 0)
                    query = query.Where(x => x.Id == situacaoContratoDto.Id);

                if (!string.IsNullOrEmpty(situacaoContratoDto.Nome))
                    query = query.Where(x => x.Nome.Contains(situacaoContratoDto.Nome));

                if (situacaoContratoDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == situacaoContratoDto.Ativo.Value);
            }

            return query.Select(x => new SituacaoContratoDto()
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

        public int Incluir(SituacaoContratoDto situacaoContratoDto)
        {
            var SituacaoContrato = new SituacaoContrato()
            {
                Nome = situacaoContratoDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = situacaoContratoDto.IdUsuarioCadastro,
                DataCadastro = situacaoContratoDto.DataCadastro
            };

            projetoTransportadoraEntities.SituacaoContrato.Add(SituacaoContrato);
            projetoTransportadoraEntities.SaveChanges();

            return SituacaoContrato.Id;
        }

        public void Alterar(SituacaoContratoDto situacaoContratoDto)
        {
            var situacaoContrato = projetoTransportadoraEntities.SituacaoContrato.FirstOrDefault(x => x.Id == situacaoContratoDto.Id);

            if (!string.IsNullOrEmpty(situacaoContratoDto.Nome))
                situacaoContrato.Nome = situacaoContratoDto.Nome;

            if (situacaoContratoDto.Ativo.HasValue)
            {
                situacaoContrato.Ativo = situacaoContratoDto.Ativo.Value;
                situacaoContrato.IdUsuarioInativacao = situacaoContratoDto.IdUsuarioInativacao;
                situacaoContrato.DataInativacao = situacaoContratoDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(situacaoContrato).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
