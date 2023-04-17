using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class EstadoCivilRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public EstadoCivilRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }
        public EstadoCivilDto Obter(EstadoCivilDto estadoCivilDto)
        {
            IQueryable<EstadoCivil> query = projetoTransportadoraEntities.EstadoCivil;

            if (estadoCivilDto.Id > 0)
                query = query.Where(x => x.Id == estadoCivilDto.Id);

            return query.Select(x => new EstadoCivilDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(EstadoCivilDto estadoCivilDto)
        {
            IQueryable<EstadoCivil> query = projetoTransportadoraEntities.EstadoCivil;

            if (estadoCivilDto.Id > 0)
                query = query.Where(x => x.Id == estadoCivilDto.Id);

            if (!string.IsNullOrEmpty(estadoCivilDto.Nome))
                query = query.Where(x => x.Nome == estadoCivilDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<EstadoCivilDto> Listar(EstadoCivilDto estadoCivilDto = null)
        {
            IQueryable<EstadoCivil> query = projetoTransportadoraEntities.EstadoCivil;

            if (estadoCivilDto != null)
            {
                if (estadoCivilDto.Id > 0)
                    query = query.Where(x => x.Id == estadoCivilDto.Id);

                if (!string.IsNullOrEmpty(estadoCivilDto.Nome))
                    query = query.Where(x => x.Nome.Contains(estadoCivilDto.Nome));

                if (estadoCivilDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == estadoCivilDto.Ativo.Value);
            }

            return query.Select(x => new EstadoCivilDto()
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

        public int Incluir(EstadoCivilDto estadoCivilDto)
        {
            var EstadoCivil = new EstadoCivil()
            {
                Nome = estadoCivilDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = estadoCivilDto.IdUsuarioCadastro,
                DataCadastro = estadoCivilDto.DataCadastro
            };

            projetoTransportadoraEntities.EstadoCivil.Add(EstadoCivil);
            projetoTransportadoraEntities.SaveChanges();

            return EstadoCivil.Id;
        }

        public void Alterar(EstadoCivilDto estadoCivilDto)
        {
            var estadoCivil = projetoTransportadoraEntities.EstadoCivil.FirstOrDefault(x => x.Id == estadoCivilDto.Id);

            if (!string.IsNullOrEmpty(estadoCivilDto.Nome))
                estadoCivil.Nome = estadoCivilDto.Nome;

            if (estadoCivilDto.Ativo.HasValue)
            {
                estadoCivil.Ativo = estadoCivilDto.Ativo.Value;
                estadoCivil.IdUsuarioInativacao = estadoCivilDto.IdUsuarioInativacao;
                estadoCivil.DataInativacao = estadoCivilDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(estadoCivil).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
