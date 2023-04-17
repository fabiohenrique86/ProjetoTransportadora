using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class RegimeCasamentoBusiness
    {
        RegimeCasamentoRepository regimeCasamentoRepository;
        public RegimeCasamentoBusiness()
        {
            regimeCasamentoRepository = new RegimeCasamentoRepository();
        }

        public List<RegimeCasamentoDto> Listar(RegimeCasamentoDto regimeCasamentoDto = null)
        {
            return regimeCasamentoRepository.Listar(regimeCasamentoDto);
        }

        public int Incluir(RegimeCasamentoDto regimeCasamentoDto)
        {
            var idRegimeCasamento = 0;

            if (regimeCasamentoDto == null)
                throw new BusinessException("RegimeCasamentoDto é nulo");

            if (string.IsNullOrEmpty(regimeCasamentoDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            var existeRegimeCasamento = regimeCasamentoRepository.Existe(new RegimeCasamentoDto() { Nome = regimeCasamentoDto.Nome });

            if (existeRegimeCasamento)
                throw new BusinessException($"RegimeCasamento ({regimeCasamentoDto.Nome}) já está cadastrado");

            idRegimeCasamento = regimeCasamentoRepository.Incluir(regimeCasamentoDto);

            return idRegimeCasamento;
        }

        public void Alterar(RegimeCasamentoDto regimeCasamentoDto)
        {
            if (regimeCasamentoDto == null)
                throw new BusinessException("RegimeCasamentoDto é nulo");

            if (regimeCasamentoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeRegimeCasamentoPorId = regimeCasamentoRepository.Obter(new RegimeCasamentoDto() { Id = regimeCasamentoDto.Id });

            if (existeRegimeCasamentoPorId == null)
                throw new BusinessException($"Regime de Casamento ({regimeCasamentoDto.Id}) não está cadastrado");

            if (!string.IsNullOrEmpty(regimeCasamentoDto.Nome))
            {
                if (regimeCasamentoDto.Nome != existeRegimeCasamentoPorId.Nome)
                {
                    var existeRegimeCasamentoPorNome = regimeCasamentoRepository.Existe(new RegimeCasamentoDto() { Nome = regimeCasamentoDto.Nome });

                    if (existeRegimeCasamentoPorNome)
                        throw new BusinessException($"Regime de Casamento ({regimeCasamentoDto.Nome}) já está cadastrado");
                }
            }

            regimeCasamentoRepository.Alterar(regimeCasamentoDto);
        }

        public void AlterarStatus(RegimeCasamentoDto regimeCasamentoDto)
        {
            if (regimeCasamentoDto == null)
                throw new BusinessException("RegimeCasamentoDto é nulo");

            if (regimeCasamentoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!regimeCasamentoDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeRegimeCasamento = regimeCasamentoRepository.Existe(new RegimeCasamentoDto() { Id = regimeCasamentoDto.Id });

            if (!existeRegimeCasamento)
                throw new BusinessException($"RegimeCasamento ({regimeCasamentoDto.Id}) não está cadastrado");

            if (regimeCasamentoDto.Ativo.HasValue)
            {
                if (!regimeCasamentoDto.Ativo.Value)
                {
                    if (regimeCasamentoDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (regimeCasamentoDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            regimeCasamentoRepository.Alterar(regimeCasamentoDto);
        }
    }
}
