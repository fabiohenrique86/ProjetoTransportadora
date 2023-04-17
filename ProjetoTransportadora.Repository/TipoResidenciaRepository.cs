using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class TipoResidenciaRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public TipoResidenciaRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public TipoResidenciaDto Obter(TipoResidenciaDto tipoResidenciaDto)
        {
            IQueryable<TipoResidencia> query = projetoTransportadoraEntities.TipoResidencia;

            if (tipoResidenciaDto.Id > 0)
                query = query.Where(x => x.Id == tipoResidenciaDto.Id);

            return query.Select(x => new TipoResidenciaDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(TipoResidenciaDto tipoResidenciaDto)
        {
            IQueryable<TipoResidencia> query = projetoTransportadoraEntities.TipoResidencia;

            if (tipoResidenciaDto.Id > 0)
                query = query.Where(x => x.Id == tipoResidenciaDto.Id);

            if (!string.IsNullOrEmpty(tipoResidenciaDto.Nome))
                query = query.Where(x => x.Nome == tipoResidenciaDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<TipoResidenciaDto> Listar(TipoResidenciaDto tipoResidenciaDto = null)
        {
            IQueryable<TipoResidencia> query = projetoTransportadoraEntities.TipoResidencia;

            if (tipoResidenciaDto != null)
            {

                if (tipoResidenciaDto.Id > 0)
                    query = query.Where(x => x.Id == tipoResidenciaDto.Id);

                if (!string.IsNullOrEmpty(tipoResidenciaDto.Nome))
                    query = query.Where(x => x.Nome.Contains(tipoResidenciaDto.Nome));

                if (tipoResidenciaDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == tipoResidenciaDto.Ativo.Value);
            }

            return query.Select(x => new TipoResidenciaDto()
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

        public int Incluir(TipoResidenciaDto tipoResidenciaDto)
        {
            var TipoResidencia = new TipoResidencia()
            {
                Nome = tipoResidenciaDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = tipoResidenciaDto.IdUsuarioCadastro,
                DataCadastro = tipoResidenciaDto.DataCadastro
            };

            projetoTransportadoraEntities.TipoResidencia.Add(TipoResidencia);
            projetoTransportadoraEntities.SaveChanges();

            return TipoResidencia.Id;
        }

        public void Alterar(TipoResidenciaDto tipoResidenciaDto)
        {
            var tipoResidencia = projetoTransportadoraEntities.TipoResidencia.FirstOrDefault(x => x.Id == tipoResidenciaDto.Id);

            if (!string.IsNullOrEmpty(tipoResidenciaDto.Nome))
                tipoResidencia.Nome = tipoResidenciaDto.Nome;

            if (tipoResidenciaDto.Ativo.HasValue)
            {
                tipoResidencia.Ativo = tipoResidenciaDto.Ativo.Value;
                tipoResidencia.IdUsuarioInativacao = tipoResidenciaDto.IdUsuarioInativacao;
                tipoResidencia.DataInativacao = tipoResidenciaDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(tipoResidencia).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
