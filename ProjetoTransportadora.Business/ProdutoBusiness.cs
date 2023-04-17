using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class ProdutoBusiness
    {
        ProdutoRepository produtoRepository;
        public ProdutoBusiness()
        {
            produtoRepository = new ProdutoRepository();
        }

        public List<ProdutoDto> Listar(ProdutoDto produtoDto = null)
        {
            return produtoRepository.Listar(produtoDto);
        }

        public int Incluir(ProdutoDto produtoDto)
        {
            var idProduto = 0;

            if (produtoDto == null)
                throw new BusinessException("ProdutoDto é nulo");

            if (string.IsNullOrEmpty(produtoDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            var existeProduto = produtoRepository.Existe(new ProdutoDto() { Nome = produtoDto.Nome });

            if (existeProduto)
                throw new BusinessException($"Produto ({produtoDto.Nome}) já está cadastrado");

            idProduto = produtoRepository.Incluir(produtoDto);

            return idProduto;
        }

        public void Alterar(ProdutoDto produtoDto)
        {
            if (produtoDto == null)
                throw new BusinessException("ProdutoDto é nulo");

            if (produtoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeProdutoPorId = produtoRepository.Obter(new ProdutoDto() { Id = produtoDto.Id });

            if (existeProdutoPorId == null)
                throw new BusinessException($"Produto ({produtoDto.Id}) não está cadastrado");

            if (!string.IsNullOrEmpty(produtoDto.Nome))
            {
                if (produtoDto.Nome != existeProdutoPorId.Nome)
                {
                    var existeProdutoPorNome = produtoRepository.Existe(new ProdutoDto() { Nome = produtoDto.Nome });

                    if (existeProdutoPorNome)
                        throw new BusinessException($"Produto ({produtoDto.Nome}) já está cadastrado");
                }
            }

            produtoRepository.Alterar(produtoDto);
        }

        public void AlterarStatus(ProdutoDto produtoDto)
        {
            if (produtoDto == null)
                throw new BusinessException("ProdutoDto é nulo");

            if (produtoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!produtoDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeProduto = produtoRepository.Existe(new ProdutoDto() { Id = produtoDto.Id });

            if (!existeProduto)
                throw new BusinessException($"Produto ({produtoDto.Id}) não está cadastrado");

            if (produtoDto.Ativo.HasValue)
            {
                if (!produtoDto.Ativo.Value)
                {
                    if (produtoDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (produtoDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            produtoRepository.Alterar(produtoDto);
        }
    }
}
