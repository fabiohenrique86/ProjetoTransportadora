using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class MontadoraBusiness
    {
        MontadoraRepository montadoraRepository;
        public MontadoraBusiness()
        {
            montadoraRepository = new MontadoraRepository();
        }

        public MontadoraDto Obter(MontadoraDto montadoraDto = null)
        {
            return montadoraRepository.Obter(montadoraDto);
        }

        public List<MontadoraDto> Listar(MontadoraDto montadoraDto = null)
        {
            return montadoraRepository.Listar(montadoraDto);
        }

        public int Incluir(MontadoraDto montadoraDto)
        {
            var idMontadora = 0;

            if (montadoraDto == null)
                throw new BusinessException("MontadoraDto é nulo");

            if (string.IsNullOrEmpty(montadoraDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            var existeMontadoraPorNome = montadoraRepository.Existe(new MontadoraDto() { Nome = montadoraDto.Nome });

            if (existeMontadoraPorNome)
                throw new BusinessException($"Montadora ({montadoraDto.Nome}) já está cadastrada");

            idMontadora = montadoraRepository.Incluir(montadoraDto);

            return idMontadora;
        }

        public void Alterar(MontadoraDto montadoraDto)
        {
            if (montadoraDto == null)
                throw new BusinessException("MontadoraDto é nulo");

            if (montadoraDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeMontadoraPorId = montadoraRepository.Obter(new MontadoraDto() { Id = montadoraDto.Id });

            if (existeMontadoraPorId == null)
                throw new BusinessException($"Montadora ({montadoraDto.Id}) não está cadastrada");

            if (!string.IsNullOrEmpty(montadoraDto.Nome))
            {
                if (montadoraDto.Nome != existeMontadoraPorId.Nome)
                {
                    var existeMontadoraPorNome = montadoraRepository.Existe(new MontadoraDto() { Nome = montadoraDto.Nome });

                    if (existeMontadoraPorNome)
                        throw new BusinessException($"Montadora ({montadoraDto.Nome}) já está cadastrada");
                }
            }

            montadoraRepository.Alterar(montadoraDto);
        }

        public void AlterarStatus(MontadoraDto montadoraDto)
        {
            if (montadoraDto == null)
                throw new BusinessException("MontadoraDto é nulo");

            if (montadoraDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!montadoraDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeMontadora = montadoraRepository.Existe(new MontadoraDto() { Id = montadoraDto.Id });

            if (!existeMontadora)
                throw new BusinessException($"Montadora ({montadoraDto.Id}) não está cadastrada");

            if (montadoraDto.Ativo.HasValue)
            {
                if (!montadoraDto.Ativo.Value)
                {
                    if (montadoraDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (montadoraDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            montadoraRepository.Alterar(montadoraDto);
        }
    }
}
