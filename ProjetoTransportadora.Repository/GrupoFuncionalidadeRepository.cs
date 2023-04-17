using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class GrupoFuncionalidadeRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public GrupoFuncionalidadeRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public bool Existe(GrupoFuncionalidadeDto grupoFuncionalidadeDto)
        {
            IQueryable<GrupoFuncionalidade> query = projetoTransportadoraEntities.GrupoFuncionalidade;

            if (grupoFuncionalidadeDto.Id > 0)
                query = query.Where(x => x.Id == grupoFuncionalidadeDto.Id);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<GrupoFuncionalidadeDto> Listar(GrupoFuncionalidadeDto grupoFuncionalidadeDto = null)
        {
            IQueryable<GrupoFuncionalidade> query = projetoTransportadoraEntities.GrupoFuncionalidade;

            if (grupoFuncionalidadeDto != null)
            {
                if (grupoFuncionalidadeDto.Id > 0)
                    query = query.Where(x => x.Id == grupoFuncionalidadeDto.Id);

                if (grupoFuncionalidadeDto.IdGrupo > 0)
                    query = query.Where(x => x.IdGrupo == grupoFuncionalidadeDto.IdGrupo);

                if (grupoFuncionalidadeDto.IdFuncionalidade > 0)
                    query = query.Where(x => x.IdFuncionalidade == grupoFuncionalidadeDto.IdFuncionalidade);
            }

            return query.Select(x => new GrupoFuncionalidadeDto()
            {
                Id = x.Id,
                IdGrupo = x.IdGrupo,
                IdFuncionalidade = x.IdFuncionalidade,
                Inserir = x.Inserir,
                Ler = x.Ler,
                Atualizar = x.Atualizar,
                Excluir = x.Excluir,
                Executar = x.Executar,
                FuncionalidadeDto = new FuncionalidadeDto() { Id = x.IdFuncionalidade, Nome = x.Funcionalidade.Nome, Descricao = x.Funcionalidade.Descricao },
                GrupoDto = new GrupoDto() { Id = x.IdGrupo, Nome = x.Grupo.Nome }
            }).OrderBy(x => x.GrupoDto.Nome).ToList();
        }

        public int Incluir(GrupoFuncionalidadeDto grupoFuncionalidadeDto)
        {
            var grupoFuncionalidade = new GrupoFuncionalidade()
            {
                IdGrupo = grupoFuncionalidadeDto.IdGrupo,
                IdFuncionalidade = grupoFuncionalidadeDto.IdFuncionalidade,
                Inserir = grupoFuncionalidadeDto.Inserir.GetValueOrDefault(),
                Ler = grupoFuncionalidadeDto.Ler.GetValueOrDefault(),
                Atualizar = grupoFuncionalidadeDto.Atualizar.GetValueOrDefault(),
                Excluir = grupoFuncionalidadeDto.Excluir.GetValueOrDefault(),
                Executar = grupoFuncionalidadeDto.Executar.GetValueOrDefault()
            };

            projetoTransportadoraEntities.GrupoFuncionalidade.Add(grupoFuncionalidade);
            projetoTransportadoraEntities.SaveChanges();

            return grupoFuncionalidade.Id;
        }

        public void Excluir(int idGrupo)
        {
            var grupoFuncionalidade = projetoTransportadoraEntities.GrupoFuncionalidade.Where(x => x.IdGrupo == idGrupo);

            projetoTransportadoraEntities.GrupoFuncionalidade.RemoveRange(grupoFuncionalidade);

            projetoTransportadoraEntities.SaveChanges();
        }

        public void AlterarStatus(GrupoFuncionalidadeDto grupoFuncionalidadeDto)
        {
            var grupoFuncionalidade = projetoTransportadoraEntities.GrupoFuncionalidade.FirstOrDefault(x => x.Id == grupoFuncionalidadeDto.Id);

            if (grupoFuncionalidadeDto.Inserir.HasValue)
                grupoFuncionalidade.Inserir = grupoFuncionalidadeDto.Inserir.GetValueOrDefault();

            if (grupoFuncionalidadeDto.Ler.HasValue)
                grupoFuncionalidade.Ler = grupoFuncionalidadeDto.Ler.GetValueOrDefault();

            if (grupoFuncionalidadeDto.Atualizar.HasValue)
                grupoFuncionalidade.Atualizar = grupoFuncionalidadeDto.Atualizar.GetValueOrDefault();

            if (grupoFuncionalidadeDto.Excluir.HasValue)
                grupoFuncionalidade.Excluir = grupoFuncionalidadeDto.Excluir.GetValueOrDefault();

            if (grupoFuncionalidadeDto.Executar.HasValue)
                grupoFuncionalidade.Executar = grupoFuncionalidadeDto.Executar.GetValueOrDefault();

            projetoTransportadoraEntities.Entry(grupoFuncionalidade).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
