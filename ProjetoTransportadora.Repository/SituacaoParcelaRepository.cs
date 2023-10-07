using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class SituacaoParcelaRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public SituacaoParcelaRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public SituacaoParcelaDto Obter(SituacaoParcelaDto situacaoParcelaDto)
        {
            IQueryable<SituacaoParcela> query = projetoTransportadoraEntities.SituacaoParcela;

            if (situacaoParcelaDto.Id > 0)
                query = query.Where(x => x.Id == situacaoParcelaDto.Id);

            if (!string.IsNullOrEmpty(situacaoParcelaDto.Nome))
                query = query.Where(x => x.Nome == situacaoParcelaDto.Nome);

            return query.Select(x => new SituacaoParcelaDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(SituacaoParcelaDto situacaoParcelaDto)
        {
            IQueryable<SituacaoParcela> query = projetoTransportadoraEntities.SituacaoParcela;

            if (situacaoParcelaDto.Id > 0)
                query = query.Where(x => x.Id == situacaoParcelaDto.Id);

            if (!string.IsNullOrEmpty(situacaoParcelaDto.Nome))
                query = query.Where(x => x.Nome == situacaoParcelaDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<SituacaoParcelaDto> Listar(SituacaoParcelaDto situacaoParcelaDto = null)
        {
            IQueryable<SituacaoParcela> query = projetoTransportadoraEntities.SituacaoParcela;

            if (situacaoParcelaDto != null)
            {

                if (situacaoParcelaDto.Id > 0)
                    query = query.Where(x => x.Id == situacaoParcelaDto.Id);

                if (!string.IsNullOrEmpty(situacaoParcelaDto.Nome))
                    query = query.Where(x => x.Nome.Contains(situacaoParcelaDto.Nome));

                if (situacaoParcelaDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == situacaoParcelaDto.Ativo.Value);
            }

            return query.Select(x => new SituacaoParcelaDto()
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
    }
}
