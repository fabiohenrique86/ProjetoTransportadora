using ProjetoTransportadora.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class CanalRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public CanalRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }
        public CanalDto Obter(CanalDto canalDto)
        {
            IQueryable<Canal> query = projetoTransportadoraEntities.Canal;

            if (canalDto.Id > 0)
                query = query.Where(x => x.Id == canalDto.Id);

            return query.Select(x => new CanalDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(CanalDto canalDto)
        {
            IQueryable<Canal> query = projetoTransportadoraEntities.Canal;

            if (canalDto.Id > 0)
                query = query.Where(x => x.Id == canalDto.Id);

            if (!string.IsNullOrEmpty(canalDto.Nome))
                query = query.Where(x => x.Nome == canalDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<CanalDto> Listar(CanalDto canalDto = null)
        {
            IQueryable<Canal> query = projetoTransportadoraEntities.Canal;

            if (canalDto != null)
            {
                if (canalDto.Id > 0)
                    query = query.Where(x => x.Id == canalDto.Id);

                if (!string.IsNullOrEmpty(canalDto.Nome))
                    query = query.Where(x => x.Nome.Contains(canalDto.Nome));

                if (canalDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == canalDto.Ativo.Value);
            }

            return query.Select(x => new CanalDto()
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

        public int Incluir(CanalDto canalDto)
        {
            var canal = new Canal()
            {
                Nome = canalDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = canalDto.IdUsuarioCadastro,
                DataCadastro = canalDto.DataCadastro
            };

            projetoTransportadoraEntities.Canal.Add(canal);
            projetoTransportadoraEntities.SaveChanges();

            return canal.Id;
        }

        public void Alterar(CanalDto canalDto)
        {
            var canal = projetoTransportadoraEntities.Canal.FirstOrDefault(x => x.Id == canalDto.Id);

            if (!string.IsNullOrEmpty(canalDto.Nome))
                canal.Nome = canalDto.Nome;

            if (canalDto.Ativo.HasValue)
            {
                canal.Ativo = canalDto.Ativo.Value;
                canal.IdUsuarioInativacao = canalDto.IdUsuarioInativacao;
                canal.DataInativacao = canalDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(canal).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
