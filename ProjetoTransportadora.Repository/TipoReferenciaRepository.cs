using ProjetoTransportadora.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class TipoReferenciaRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public TipoReferenciaRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public TipoReferenciaDto Obter(TipoReferenciaDto tipoReferenciaDto)
        {
            IQueryable<TipoReferencia> query = projetoTransportadoraEntities.TipoReferencia;

            if (tipoReferenciaDto.Id > 0)
                query = query.Where(x => x.Id == tipoReferenciaDto.Id);

            return query.Select(x => new TipoReferenciaDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(TipoReferenciaDto tipoReferenciaDto)
        {
            IQueryable<TipoReferencia> query = projetoTransportadoraEntities.TipoReferencia;

            if (tipoReferenciaDto.Id > 0)
                query = query.Where(x => x.Id == tipoReferenciaDto.Id);

            if (!string.IsNullOrEmpty(tipoReferenciaDto.Nome))
                query = query.Where(x => x.Nome == tipoReferenciaDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<TipoReferenciaDto> Listar(TipoReferenciaDto tipoReferenciaDto)
        {
            IQueryable<TipoReferencia> query = projetoTransportadoraEntities.TipoReferencia;

            if (tipoReferenciaDto.Id > 0)
                query = query.Where(x => x.Id == tipoReferenciaDto.Id);

            if (!string.IsNullOrEmpty(tipoReferenciaDto.Nome))
                query = query.Where(x => x.Nome.Contains(tipoReferenciaDto.Nome));

            if (tipoReferenciaDto.Ativo)
                query = query.Where(x => x.Ativo == tipoReferenciaDto.Ativo);

            return query.Select(x => new TipoReferenciaDto()
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

        public int Incluir(TipoReferenciaDto tipoReferenciaDto)
        {
            var tipoReferencia = new TipoReferencia()
            {
                Nome = tipoReferenciaDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = tipoReferenciaDto.IdUsuarioCadastro,
                DataCadastro = DateTime.UtcNow
            };

            projetoTransportadoraEntities.TipoReferencia.Add(tipoReferencia);
            projetoTransportadoraEntities.SaveChanges();

            return tipoReferencia.Id;
        }
    }
}
