using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class TipoResidenciaBusiness
    {
        TipoResidenciaRepository tipoResidenciaRepository;
        public TipoResidenciaBusiness()
        {
            tipoResidenciaRepository = new TipoResidenciaRepository();
        }

        public List<TipoResidenciaDto> Listar(TipoResidenciaDto tipoResidenciaDto = null)
        {
            return tipoResidenciaRepository.Listar(tipoResidenciaDto);
        }

        public int Incluir(TipoResidenciaDto tipoResidenciaDto)
        {
            var idTipoResidencia = 0;

            if (tipoResidenciaDto == null)
                throw new BusinessException("TipoResidenciaDto é nulo");

            if (string.IsNullOrEmpty(tipoResidenciaDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            var existeTipoResidencia = tipoResidenciaRepository.Existe(new TipoResidenciaDto() { Nome = tipoResidenciaDto.Nome });

            if (existeTipoResidencia)
                throw new BusinessException($"TipoResidencia ({tipoResidenciaDto.Nome}) já está cadastrado");

            idTipoResidencia = tipoResidenciaRepository.Incluir(tipoResidenciaDto);

            return idTipoResidencia;
        }

        public void Alterar(TipoResidenciaDto tipoResidenciaDto)
        {
            if (tipoResidenciaDto == null)
                throw new BusinessException("TipoResidenciaDto é nulo");

            if (tipoResidenciaDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeTipoResidenciaPorId = tipoResidenciaRepository.Obter(new TipoResidenciaDto() { Id = tipoResidenciaDto.Id });

            if (existeTipoResidenciaPorId == null)
                throw new BusinessException($"TipoResidencia ({tipoResidenciaDto.Id}) não está cadastrado");

            if (!string.IsNullOrEmpty(tipoResidenciaDto.Nome))
            {
                if (tipoResidenciaDto.Nome != existeTipoResidenciaPorId.Nome)
                {
                    var existeTipoResidenciaPorNome = tipoResidenciaRepository.Existe(new TipoResidenciaDto() { Nome = tipoResidenciaDto.Nome });

                    if (existeTipoResidenciaPorNome)
                        throw new BusinessException($"Tipo de Residência ({tipoResidenciaDto.Nome}) já está cadastrada");
                }
            }

            tipoResidenciaRepository.Alterar(tipoResidenciaDto);
        }

        public void AlterarStatus(TipoResidenciaDto tipoResidenciaDto)
        {
            if (tipoResidenciaDto == null)
                throw new BusinessException("TipoResidenciaDto é nulo");

            if (tipoResidenciaDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!tipoResidenciaDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeTipoResidencia = tipoResidenciaRepository.Existe(new TipoResidenciaDto() { Id = tipoResidenciaDto.Id });

            if (!existeTipoResidencia)
                throw new BusinessException($"TipoResidencia ({tipoResidenciaDto.Id}) não está cadastrado");

            if (tipoResidenciaDto.Ativo.HasValue)
            {
                if (!tipoResidenciaDto.Ativo.Value)
                {
                    if (tipoResidenciaDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (tipoResidenciaDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            tipoResidenciaRepository.Alterar(tipoResidenciaDto);
        }
    }
}
