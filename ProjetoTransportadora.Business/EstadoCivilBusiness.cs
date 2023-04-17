using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class EstadoCivilBusiness
    {
        EstadoCivilRepository estadoCivilRepository;
        public EstadoCivilBusiness()
        {
            estadoCivilRepository = new EstadoCivilRepository();
        }

        public List<EstadoCivilDto> Listar(EstadoCivilDto estadoCivilDto = null)
        {
            return estadoCivilRepository.Listar(estadoCivilDto);
        }

        public int Incluir(EstadoCivilDto estadoCivilDto)
        {
            var idEstadoCivil = 0;

            if (estadoCivilDto == null)
                throw new BusinessException("EstadoCivilDto é nulo");

            if (string.IsNullOrEmpty(estadoCivilDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            var existeEstadoCivil = estadoCivilRepository.Existe(new EstadoCivilDto() { Nome = estadoCivilDto.Nome });

            if (existeEstadoCivil)
                throw new BusinessException($"EstadoCivil ({estadoCivilDto.Nome}) já está cadastrado");

            idEstadoCivil = estadoCivilRepository.Incluir(estadoCivilDto);

            return idEstadoCivil;
        }

        public void Alterar(EstadoCivilDto estadoCivilDto)
        {
            if (estadoCivilDto == null)
                throw new BusinessException("EstadoCivilDto é nulo");

            if (estadoCivilDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeEstadoCivilPorId = estadoCivilRepository.Obter(new EstadoCivilDto() { Id = estadoCivilDto.Id });

            if (existeEstadoCivilPorId == null)
                throw new BusinessException($"EstadoCivil ({estadoCivilDto.Id}) não está cadastrado");

            if (!string.IsNullOrEmpty(estadoCivilDto.Nome))
            {
                if (estadoCivilDto.Nome != existeEstadoCivilPorId.Nome)
                {
                    var existeEstadoCivilPorNome = estadoCivilRepository.Existe(new EstadoCivilDto() { Nome = estadoCivilDto.Nome });

                    if (existeEstadoCivilPorNome)
                        throw new BusinessException($"Estado Civil ({estadoCivilDto.Nome}) já está cadastrado");
                }
            }

            estadoCivilRepository.Alterar(estadoCivilDto);
        }

        public void AlterarStatus(EstadoCivilDto estadoCivilDto)
        {
            if (estadoCivilDto == null)
                throw new BusinessException("EstadoCivilDto é nulo");

            if (estadoCivilDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!estadoCivilDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeEstadoCivil = estadoCivilRepository.Existe(new EstadoCivilDto() { Id = estadoCivilDto.Id });

            if (!existeEstadoCivil)
                throw new BusinessException($"EstadoCivil ({estadoCivilDto.Id}) não está cadastrado");

            if (estadoCivilDto.Ativo.HasValue)
            {
                if (!estadoCivilDto.Ativo.Value)
                {
                    if (estadoCivilDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (estadoCivilDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            estadoCivilRepository.Alterar(estadoCivilDto);
        }
    }
}
