using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class GrupoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public GrupoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }
        public GrupoDto Obter(GrupoDto grupoDto)
        {
            IQueryable<Grupo> query = projetoTransportadoraEntities.Grupo;

            if (grupoDto.Id > 0)
                query = query.Where(x => x.Id == grupoDto.Id);

            return query.Select(x => new GrupoDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(GrupoDto grupoDto)
        {
            IQueryable<Grupo> query = projetoTransportadoraEntities.Grupo;

            if (grupoDto.Id > 0)
                query = query.Where(x => x.Id == grupoDto.Id);

            if (!string.IsNullOrEmpty(grupoDto.Nome))
                query = query.Where(x => x.Nome == grupoDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<GrupoDto> Listar(GrupoDto grupoDto = null)
        {
            IQueryable<Grupo> query = projetoTransportadoraEntities.Grupo;

            if (grupoDto != null)
            {

                if (grupoDto.Id > 0)
                    query = query.Where(x => x.Id == grupoDto.Id);

                if (!string.IsNullOrEmpty(grupoDto.Nome))
                    query = query.Where(x => x.Nome.Contains(grupoDto.Nome));

                if (grupoDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == grupoDto.Ativo.Value);
            }

            return query.Select(x => new GrupoDto()
            {
                Id = x.Id,
                Ativo = x.Ativo,
                Nome = x.Nome,
                DataCadastro = x.DataCadastro,
                IdUsuarioCadastro = x.IdUsuarioCadastro,
                DataInativacao = x.DataInativacao,
                IdUsuarioInativacao = x.IdUsuarioInativacao,
                GrupoFuncionalidadeDto = x.GrupoFuncionalidade.Select(w => new GrupoFuncionalidadeDto()
                {
                    Id = w.Id,
                    IdGrupo = w.IdGrupo,
                    Inserir = w.Inserir,
                    Ler = w.Ler,
                    Atualizar = w.Atualizar,
                    Excluir = w.Excluir,
                    Executar = w.Executar,
                    IdFuncionalidade = w.IdFuncionalidade,
                    FuncionalidadeDto = new FuncionalidadeDto()
                    {
                        Id = w.Id,
                        Nome = w.Funcionalidade.Nome,
                        Descricao = w.Funcionalidade.Descricao
                    }
                }).ToList()
            }).OrderBy(x => x.Nome).ToList();
        }

        public int Incluir(GrupoDto grupoDto)
        {
            var grupo = new Grupo()
            {
                Nome = grupoDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = grupoDto.IdUsuarioCadastro,
                DataCadastro = grupoDto.DataCadastro
            };

            projetoTransportadoraEntities.Grupo.Add(grupo);
            projetoTransportadoraEntities.SaveChanges();

            return grupo.Id;
        }

        public void Alterar(GrupoDto grupoDto)
        {
            var grupo = projetoTransportadoraEntities.Grupo.FirstOrDefault(x => x.Id == grupoDto.Id);

            if (!string.IsNullOrEmpty(grupoDto.Nome))
                grupo.Nome = grupoDto.Nome;

            if (grupoDto.Ativo.HasValue)
            {
                grupo.Ativo = grupoDto.Ativo.Value;
                grupo.IdUsuarioInativacao = grupoDto.IdUsuarioInativacao;
                grupo.DataInativacao = grupoDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(grupo).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
