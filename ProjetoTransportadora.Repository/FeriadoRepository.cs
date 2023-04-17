using ProjetoTransportadora.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class FeriadoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public FeriadoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }
        public FeriadoDto Obter(FeriadoDto feriadoDto)
        {
            IQueryable<Feriado> query = projetoTransportadoraEntities.Feriado;

            if (feriadoDto.Id > 0)
                query = query.Where(x => x.Id == feriadoDto.Id);

            return query.Select(x => new FeriadoDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(FeriadoDto feriadoDto)
        {
            IQueryable<Feriado> query = projetoTransportadoraEntities.Feriado;

            if (feriadoDto.Id > 0)
                query = query.Where(x => x.Id == feriadoDto.Id);

            if (!string.IsNullOrEmpty(feriadoDto.Nome))
                query = query.Where(x => x.Nome == feriadoDto.Nome);

            if (feriadoDto.DataFeriado != DateTime.MinValue)
                query = query.Where(x => x.DataFeriado == feriadoDto.DataFeriado);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<FeriadoDto> Listar(FeriadoDto feriadoDto = null, DateTime? dtInicial = null, DateTime? dtFinal = null)
        {
            IQueryable<Feriado> query = projetoTransportadoraEntities.Feriado;

            if (feriadoDto != null)
            {
                if (feriadoDto.Id > 0)
                    query = query.Where(x => x.Id == feriadoDto.Id);

                if (!string.IsNullOrEmpty(feriadoDto.Nome))
                    query = query.Where(x => x.Nome.Contains(feriadoDto.Nome));

                if (dtInicial != null && dtInicial != DateTime.MinValue && dtFinal != null && dtFinal != DateTime.MinValue)
                    query = query.Where(x => x.DataFeriado >= dtInicial && x.DataFeriado <= dtFinal);
                else if (dtInicial != null && dtInicial != DateTime.MinValue)
                    query = query.Where(x => x.DataFeriado >= dtInicial);
                else if (dtFinal != null && dtFinal != DateTime.MinValue)
                    query = query.Where(x => x.DataFeriado <= dtFinal);

                if (feriadoDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == feriadoDto.Ativo.Value);
            }

            return query.Select(x => new FeriadoDto()
            {
                Id = x.Id,
                Ativo = x.Ativo,
                Nome = x.Nome,
                DataFeriado = x.DataFeriado,
                DataCadastro = x.DataCadastro,
                IdUsuarioCadastro = x.IdUsuarioCadastro,
                DataInativacao = x.DataInativacao,
                IdUsuarioInativacao = x.IdUsuarioInativacao
            }).OrderBy(x => x.DataFeriado).ToList();
        }

        public int Incluir(FeriadoDto feriadoDto)
        {
            var feriado = new Feriado()
            {
                Nome = feriadoDto.Nome,
                DataFeriado = feriadoDto.DataFeriado,
                Ativo = true,
                IdUsuarioCadastro = feriadoDto.IdUsuarioCadastro,
                DataCadastro = feriadoDto.DataCadastro
            };

            projetoTransportadoraEntities.Feriado.Add(feriado);
            projetoTransportadoraEntities.SaveChanges();

            return feriado.Id;
        }

        public void Alterar(FeriadoDto feriadoDto)
        {
            var feriado = projetoTransportadoraEntities.Feriado.FirstOrDefault(x => x.Id == feriadoDto.Id);

            if (!string.IsNullOrEmpty(feriadoDto.Nome))
                feriado.Nome = feriadoDto.Nome;

            if (feriadoDto.DataFeriado != DateTime.MinValue)
                feriado.DataFeriado = feriadoDto.DataFeriado;

            if (feriadoDto.Ativo.HasValue)
            {
                feriado.Ativo = feriadoDto.Ativo.Value;
                feriado.IdUsuarioInativacao = feriadoDto.IdUsuarioInativacao;
                feriado.DataInativacao = feriadoDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(feriado).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
