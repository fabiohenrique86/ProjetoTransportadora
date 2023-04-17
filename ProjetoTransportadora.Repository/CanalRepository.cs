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
            var Canal = new Canal()
            {
                Nome = canalDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = canalDto.IdUsuarioCadastro,
                DataCadastro = canalDto.DataCadastro
            };

            projetoTransportadoraEntities.Canal.Add(Canal);
            projetoTransportadoraEntities.SaveChanges();

            return Canal.Id;
        }

        public void Alterar(CanalDto canalDto)
        {
            var Canal = projetoTransportadoraEntities.Canal.FirstOrDefault(x => x.Id == canalDto.Id);

            if (!string.IsNullOrEmpty(canalDto.Nome))
                Canal.Nome = canalDto.Nome;

            if (canalDto.Ativo.HasValue)
            {
                Canal.Ativo = canalDto.Ativo.Value;
                Canal.IdUsuarioInativacao = canalDto.IdUsuarioInativacao;
                Canal.DataInativacao = canalDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(Canal).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
