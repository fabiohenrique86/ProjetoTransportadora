using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class FuncionalidadeRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public FuncionalidadeRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public List<FuncionalidadeDto> Listar(FuncionalidadeDto funcionalidadeDto = null)
        {
            IQueryable<Funcionalidade> query = projetoTransportadoraEntities.Funcionalidade;

            if (funcionalidadeDto != null)
            {
                if (funcionalidadeDto.Id > 0)
                    query = query.Where(x => x.Id == funcionalidadeDto.Id);

                if (!string.IsNullOrEmpty(funcionalidadeDto.Nome))
                    query = query.Where(x => x.Nome.Contains(funcionalidadeDto.Nome));
            }

            return query.Select(x => new FuncionalidadeDto()
            {
                Id = x.Id,
                Nome = x.Nome,
                Descricao = x.Descricao,
            }).OrderBy(x => x.Nome).ToList();
        }
    }
}
