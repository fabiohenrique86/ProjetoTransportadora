using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class SituacaoMultaRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public SituacaoMultaRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public SituacaoMultaDto Obter(SituacaoMultaDto situacaoMultaDto)
        {
            IQueryable<SituacaoMulta> query = projetoTransportadoraEntities.SituacaoMulta;

            if (situacaoMultaDto.Id > 0)
                query = query.Where(x => x.Id == situacaoMultaDto.Id);

            return query.Select(x => new SituacaoMultaDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(SituacaoMultaDto SituacaoMultaDto)
        {
            IQueryable<SituacaoMulta> query = projetoTransportadoraEntities.SituacaoMulta;

            if (SituacaoMultaDto.Id > 0)
                query = query.Where(x => x.Id == SituacaoMultaDto.Id);

            if (!string.IsNullOrEmpty(SituacaoMultaDto.Nome))
                query = query.Where(x => x.Nome == SituacaoMultaDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<SituacaoMultaDto> Listar(SituacaoMultaDto situacaoMultaDto = null)
        {
            IQueryable<SituacaoMulta> query = projetoTransportadoraEntities.SituacaoMulta;

            if (situacaoMultaDto != null)
            {

                if (situacaoMultaDto.Id > 0)
                    query = query.Where(x => x.Id == situacaoMultaDto.Id);

                if (!string.IsNullOrEmpty(situacaoMultaDto.Nome))
                    query = query.Where(x => x.Nome.Contains(situacaoMultaDto.Nome));
            }

            return query.Select(x => new SituacaoMultaDto()
            {
                Id = x.Id,
                Nome = x.Nome
            }).OrderBy(x => x.Nome).ToList();
        }

        public int Incluir(SituacaoMultaDto situacaoMultaDto)
        {
            var situacaoMulta = new SituacaoMulta()
            {
                Nome = situacaoMultaDto.Nome
            };

            projetoTransportadoraEntities.SituacaoMulta.Add(situacaoMulta);
            projetoTransportadoraEntities.SaveChanges();

            return situacaoMulta.Id;
        }

        public void Alterar(SituacaoMultaDto situacaoMultaDto)
        {
            var SituacaoMulta = projetoTransportadoraEntities.SituacaoMulta.FirstOrDefault(x => x.Id == situacaoMultaDto.Id);

            if (!string.IsNullOrEmpty(situacaoMultaDto.Nome))
                SituacaoMulta.Nome = situacaoMultaDto.Nome;

            projetoTransportadoraEntities.Entry(SituacaoMulta).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
