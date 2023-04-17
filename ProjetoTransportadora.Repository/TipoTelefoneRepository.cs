using ProjetoTransportadora.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class TipoTelefoneRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public TipoTelefoneRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public TipoTelefoneDto Obter(TipoTelefoneDto tipoTelefoneDto)
        {
            IQueryable<TipoTelefone> query = projetoTransportadoraEntities.TipoTelefone;

            if (tipoTelefoneDto.Id > 0)
                query = query.Where(x => x.Id == tipoTelefoneDto.Id);

            return query.Select(x => new TipoTelefoneDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(TipoTelefoneDto tipoTelefoneDto)
        {
            IQueryable<TipoTelefone> query = projetoTransportadoraEntities.TipoTelefone;

            if (tipoTelefoneDto.Id > 0)
                query = query.Where(x => x.Id == tipoTelefoneDto.Id);

            if (!string.IsNullOrEmpty(tipoTelefoneDto.Nome))
                query = query.Where(x => x.Nome == tipoTelefoneDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<TipoTelefoneDto> Listar(TipoTelefoneDto tipoTelefoneDto)
        {
            IQueryable<TipoTelefone> query = projetoTransportadoraEntities.TipoTelefone;

            if (tipoTelefoneDto.Id > 0)
                query = query.Where(x => x.Id == tipoTelefoneDto.Id);

            if (!string.IsNullOrEmpty(tipoTelefoneDto.Nome))
                query = query.Where(x => x.Nome.Contains(tipoTelefoneDto.Nome));

            if (tipoTelefoneDto.Ativo)
                query = query.Where(x => x.Ativo == tipoTelefoneDto.Ativo);

            return query.Select(x => new TipoTelefoneDto()
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

        public int Incluir(TipoTelefoneDto tipoTelefoneDto)
        {
            var tipoTelefone = new TipoTelefone()
            {
                Nome = tipoTelefoneDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = tipoTelefoneDto.IdUsuarioCadastro,
                DataCadastro = DateTime.UtcNow
            };

            projetoTransportadoraEntities.TipoTelefone.Add(tipoTelefone);
            projetoTransportadoraEntities.SaveChanges();

            return tipoTelefone.Id;
        }
    }
}
