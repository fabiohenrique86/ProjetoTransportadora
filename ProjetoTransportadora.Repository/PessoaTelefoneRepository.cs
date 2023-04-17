using ProjetoTransportadora.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class PessoaTelefoneRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public PessoaTelefoneRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public bool Existe(PessoaTelefoneDto pessoaTelefoneDto)
        {
            IQueryable<PessoaTelefone> query = projetoTransportadoraEntities.PessoaTelefone;

            if (pessoaTelefoneDto.Id > 0)
                query = query.Where(x => x.Id == pessoaTelefoneDto.Id);

            return query.FirstOrDefault() != null ? true : false;
        }

        public int Incluir(PessoaTelefoneDto pessoaTelefoneDto)
        {
            var pessoaTelefone = new PessoaTelefone()
            {
                IdPessoa = pessoaTelefoneDto.IdPessoa,
                Pais = pessoaTelefoneDto.Pais,
                DDD = pessoaTelefoneDto.DDD,
                Numero = pessoaTelefoneDto.Numero,
                NomeContato = pessoaTelefoneDto.NomeContato,
                IdTipoTelefone = pessoaTelefoneDto.IdTipoTelefone
            };

            projetoTransportadoraEntities.PessoaTelefone.Add(pessoaTelefone);
            projetoTransportadoraEntities.SaveChanges();

            return pessoaTelefone.Id;
        }

        public void Alterar(PessoaTelefoneDto pessoaTelefoneDto)
        {
            var pessoaTelefone = projetoTransportadoraEntities.PessoaTelefone.FirstOrDefault(x => x.Id == pessoaTelefoneDto.Id);

            pessoaTelefone.Pais = pessoaTelefoneDto.Pais;
            pessoaTelefone.DDD = pessoaTelefoneDto.DDD;
            pessoaTelefone.Numero = pessoaTelefoneDto.Numero;
            pessoaTelefone.NomeContato = pessoaTelefoneDto.NomeContato;
            pessoaTelefone.IdTipoTelefone = pessoaTelefoneDto.IdTipoTelefone;

            projetoTransportadoraEntities.Entry(pessoaTelefone).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
        public void Excluir(int idPessoa)
        {
            var pessoaTelefone = projetoTransportadoraEntities.PessoaTelefone.Where(x => x.IdPessoa == idPessoa);
            
            projetoTransportadoraEntities.PessoaTelefone.RemoveRange(pessoaTelefone);
            
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
