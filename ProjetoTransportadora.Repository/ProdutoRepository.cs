using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class ProdutoRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public ProdutoRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }
        
        public ProdutoDto Obter(ProdutoDto produtoDto)
        {
            IQueryable<Produto> query = projetoTransportadoraEntities.Produto;

            if (produtoDto.Id > 0)
                query = query.Where(x => x.Id == produtoDto.Id);

            return query.Select(x => new ProdutoDto() { Id = x.Id, Nome = x.Nome }).FirstOrDefault();
        }

        public bool Existe(ProdutoDto produtoDto)
        {
            IQueryable<Produto> query = projetoTransportadoraEntities.Produto;

            if (produtoDto.Id > 0)
                query = query.Where(x => x.Id == produtoDto.Id);

            if (!string.IsNullOrEmpty(produtoDto.Nome))
                query = query.Where(x => x.Nome == produtoDto.Nome);

            return query.FirstOrDefault() != null ? true : false;
        }

        public List<ProdutoDto> Listar(ProdutoDto produtoDto = null)
        {
            IQueryable<Produto> query = projetoTransportadoraEntities.Produto;

            if (produtoDto != null)
            {
                if (produtoDto.Id > 0)
                    query = query.Where(x => x.Id == produtoDto.Id);

                if (!string.IsNullOrEmpty(produtoDto.Nome))
                    query = query.Where(x => x.Nome.Contains(produtoDto.Nome));

                if (produtoDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == produtoDto.Ativo.Value);
            }

            return query.Select(x => new ProdutoDto()
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

        public int Incluir(ProdutoDto produtoDto)
        {
            var Produto = new Produto()
            {
                Nome = produtoDto.Nome,
                Ativo = true,
                IdUsuarioCadastro = produtoDto.IdUsuarioCadastro,
                DataCadastro = produtoDto.DataCadastro
            };

            projetoTransportadoraEntities.Produto.Add(Produto);
            projetoTransportadoraEntities.SaveChanges();

            return Produto.Id;
        }

        public void Alterar(ProdutoDto produtoDto)
        {
            var Produto = projetoTransportadoraEntities.Produto.FirstOrDefault(x => x.Id == produtoDto.Id);

            if (!string.IsNullOrEmpty(produtoDto.Nome))
                Produto.Nome = produtoDto.Nome;

            if (produtoDto.Ativo.HasValue)
            {
                Produto.Ativo = produtoDto.Ativo.Value;
                Produto.IdUsuarioInativacao = produtoDto.IdUsuarioInativacao;
                Produto.DataInativacao = produtoDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(Produto).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
