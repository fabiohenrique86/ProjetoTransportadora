using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class MontadoraRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public MontadoraRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public MontadoraDto Obter(MontadoraDto montadoraDto)
        {
            IQueryable<Montadora> query = projetoTransportadoraEntities.Montadora;

            if (montadoraDto.Id > 0)
                query = query.Where(x => x.Id == montadoraDto.Id);

            return query.Select(x => new MontadoraDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(MontadoraDto montadoraDto)
        {
            IQueryable<Montadora> query = projetoTransportadoraEntities.Montadora;

            if (montadoraDto.Id > 0)
                query = query.Where(x => x.Id == montadoraDto.Id);

            if (!string.IsNullOrEmpty(montadoraDto.Nome))
                query = query.Where(x => x.Nome == montadoraDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<MontadoraDto> Listar(MontadoraDto montadoraDto = null)
        {
            IQueryable<Montadora> query = projetoTransportadoraEntities.Montadora;

            if (montadoraDto != null)
            {

                if (montadoraDto.Id > 0)
                    query = query.Where(x => x.Id == montadoraDto.Id);

                if (!string.IsNullOrEmpty(montadoraDto.Nome))
                    query = query.Where(x => x.Nome.Contains(montadoraDto.Nome));

                if (montadoraDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == montadoraDto.Ativo.Value);
            }

            return query.Select(x => new MontadoraDto()
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

        public int Incluir(MontadoraDto montadoraDto)
        {
            var Montadora = new Montadora()
            {
                Nome = montadoraDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = montadoraDto.IdUsuarioCadastro,
                DataCadastro = montadoraDto.DataCadastro
            };

            projetoTransportadoraEntities.Montadora.Add(Montadora);
            projetoTransportadoraEntities.SaveChanges();

            return Montadora.Id;
        }

        public void Alterar(MontadoraDto montadoraDto)
        {
            var montadora = projetoTransportadoraEntities.Montadora.FirstOrDefault(x => x.Id == montadoraDto.Id);

            if (!string.IsNullOrEmpty(montadoraDto.Nome))
                montadora.Nome = montadoraDto.Nome;

            if (montadoraDto.Ativo.HasValue)
            {
                montadora.Ativo = montadoraDto.Ativo.Value;
                montadora.IdUsuarioInativacao = montadoraDto.IdUsuarioInativacao;
                montadora.DataInativacao = montadoraDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(montadora).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
