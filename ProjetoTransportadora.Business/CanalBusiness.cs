using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class CanalBusiness : BaseBusiness
    {
        CanalRepository canalRepository;

        public CanalBusiness()
        {
            canalRepository = new CanalRepository();
        }

        public List<CanalDto> Listar(CanalDto canalDto = null)
        {
            return canalRepository.Listar(canalDto);
        }

        public int Incluir(CanalDto canalDto)
        {
            var idCanal = 0;

            if (canalDto == null)
                throw new BusinessException("CanalDto é nulo");

            if (string.IsNullOrEmpty(canalDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            var existeCanal = canalRepository.Existe(new CanalDto() { Nome = canalDto.Nome });

            if (existeCanal)
                throw new BusinessException($"Canal ({canalDto.Nome}) já está cadastrado");

            idCanal = canalRepository.Incluir(canalDto);

            return idCanal;
        }

        public void Alterar(CanalDto canalDto)
        {
            if (canalDto == null)
                throw new BusinessException("CanalDto é nulo");

            if (canalDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeCanalPorId = canalRepository.Obter(new CanalDto() { Id = canalDto.Id });

            if (existeCanalPorId == null)
                throw new BusinessException($"Canal ({canalDto.Id}) não está cadastrado");

            if (!string.IsNullOrEmpty(canalDto.Nome))
            {
                if (canalDto.Nome != existeCanalPorId.Nome)
                {
                    var existeCanalPorNome = canalRepository.Existe(new CanalDto() { Nome = canalDto.Nome });

                    if (existeCanalPorNome)
                        throw new BusinessException($"Canal ({canalDto.Nome}) já está cadastrado");
                }
            }

            canalRepository.Alterar(canalDto);
        }

        public void AlterarStatus(CanalDto canalDto)
        {
            if (canalDto == null)
                throw new BusinessException("CanalDto é nulo");

            if (canalDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!canalDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeCanal = canalRepository.Existe(new CanalDto() { Id = canalDto.Id });

            if (!existeCanal)
                throw new BusinessException($"Canal ({canalDto.Id}) não está cadastrado");

            if (canalDto.Ativo.HasValue)
            {
                if (!canalDto.Ativo.Value)
                {
                    if (canalDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (canalDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            canalRepository.Alterar(canalDto);
        }
    }
}
