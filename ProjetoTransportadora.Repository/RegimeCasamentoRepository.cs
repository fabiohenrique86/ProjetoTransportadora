using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class RegimeCasamentoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public RegimeCasamentoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public RegimeCasamentoDto Obter(RegimeCasamentoDto regimeCasamentoDto)
        {
            IQueryable<RegimeCasamento> query = projetoTransportadoraEntities.RegimeCasamento;

            if (regimeCasamentoDto.Id > 0)
                query = query.Where(x => x.Id == regimeCasamentoDto.Id);

            return query.Select(x => new RegimeCasamentoDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(RegimeCasamentoDto regimeCasamentoDto)
        {
            IQueryable<RegimeCasamento> query = projetoTransportadoraEntities.RegimeCasamento;

            if (regimeCasamentoDto.Id > 0)
                query = query.Where(x => x.Id == regimeCasamentoDto.Id);

            if (!string.IsNullOrEmpty(regimeCasamentoDto.Nome))
                query = query.Where(x => x.Nome == regimeCasamentoDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<RegimeCasamentoDto> Listar(RegimeCasamentoDto regimeCasamentoDto = null)
        {
            IQueryable<RegimeCasamento> query = projetoTransportadoraEntities.RegimeCasamento;

            if (regimeCasamentoDto != null)
            {

                if (regimeCasamentoDto.Id > 0)
                    query = query.Where(x => x.Id == regimeCasamentoDto.Id);

                if (!string.IsNullOrEmpty(regimeCasamentoDto.Nome))
                    query = query.Where(x => x.Nome.Contains(regimeCasamentoDto.Nome));

                if (regimeCasamentoDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == regimeCasamentoDto.Ativo.Value);
            }

            return query.Select(x => new RegimeCasamentoDto()
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

        public int Incluir(RegimeCasamentoDto regimeCasamentoDto)
        {
            var RegimeCasamento = new RegimeCasamento()
            {
                Nome = regimeCasamentoDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = regimeCasamentoDto.IdUsuarioCadastro,
                DataCadastro = regimeCasamentoDto.DataCadastro
            };

            projetoTransportadoraEntities.RegimeCasamento.Add(RegimeCasamento);
            projetoTransportadoraEntities.SaveChanges();

            return RegimeCasamento.Id;
        }

        public void Alterar(RegimeCasamentoDto regimeCasamentoDto)
        {
            var regimeCasamento = projetoTransportadoraEntities.RegimeCasamento.FirstOrDefault(x => x.Id == regimeCasamentoDto.Id);

            if (!string.IsNullOrEmpty(regimeCasamentoDto.Nome))
                regimeCasamento.Nome = regimeCasamentoDto.Nome;

            if (regimeCasamentoDto.Ativo.HasValue)
            {
                regimeCasamento.Ativo = regimeCasamentoDto.Ativo.Value;
                regimeCasamento.IdUsuarioInativacao = regimeCasamentoDto.IdUsuarioInativacao;
                regimeCasamento.DataInativacao = regimeCasamentoDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(regimeCasamento).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
