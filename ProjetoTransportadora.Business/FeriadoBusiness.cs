using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class FeriadoBusiness
    {
        FeriadoRepository feriadoRepository;
        public FeriadoBusiness()
        {
            feriadoRepository = new FeriadoRepository();
        }

        public List<FeriadoDto> Listar(FeriadoDto feriadoDto = null)
        {
            var cultureInfo = new System.Globalization.CultureInfo("pt-BR");

            DateTime dtInicial = DateTime.MinValue;
            DateTime dtFinal = DateTime.MinValue;

            if (feriadoDto != null)
            {
                if (!string.IsNullOrEmpty(feriadoDto.DataFeriadoInicial))
                {
                    if (!DateTime.TryParseExact(feriadoDto.DataFeriadoInicial, "dd/MM/yyyy", cultureInfo, System.Globalization.DateTimeStyles.None, out dtInicial))
                        throw new BusinessException("Data Feriado Inicial é inválida");
                }

                if (!string.IsNullOrEmpty(feriadoDto.DataFeriadoFinal))
                {
                    if (!DateTime.TryParseExact(feriadoDto.DataFeriadoFinal, "dd/MM/yyyy", cultureInfo, System.Globalization.DateTimeStyles.None, out dtFinal))
                        throw new BusinessException("Data Feriado Final é inválida");
                }
            }

            return feriadoRepository.Listar(feriadoDto, dtInicial, dtFinal);
        }

        public int Incluir(FeriadoDto feriadoDto)
        {
            var idFeriado = 0;

            if (feriadoDto == null)
                throw new BusinessException("FeriadoDto é nulo");

            if (string.IsNullOrEmpty(feriadoDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            if (feriadoDto.DataFeriado == DateTime.MinValue)
                throw new BusinessException("DataFeriado é obrigatório");

            var existeFeriadoPorData = feriadoRepository.Existe(new FeriadoDto() { DataFeriado = feriadoDto.DataFeriado });

            if (existeFeriadoPorData)
                throw new BusinessException($"Feriado Data {feriadoDto.DataFeriado.ToString("dd/MM/yyyy")} já está cadastrado");

            idFeriado = feriadoRepository.Incluir(feriadoDto);

            return idFeriado;
        }

        public void Alterar(FeriadoDto feriadoDto)
        {
            if (feriadoDto == null)
                throw new BusinessException("FeriadoDto é nulo");

            if (feriadoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeFeriadoPorId = feriadoRepository.Obter(new FeriadoDto() { Id = feriadoDto.Id });

            if (existeFeriadoPorId == null)
                throw new BusinessException($"Feriado ({feriadoDto.Id}) não está cadastrado");

            if (!string.IsNullOrEmpty(feriadoDto.Nome))
            {
                if (feriadoDto.Nome != existeFeriadoPorId.Nome)
                {
                    var existeFeriadoPorNome = feriadoRepository.Existe(new FeriadoDto() { Nome = feriadoDto.Nome });

                    if (existeFeriadoPorNome)
                        throw new BusinessException($"Feriado ({feriadoDto.Nome}) já está cadastrado");
                }
            }

            if (feriadoDto.DataFeriado != DateTime.MinValue)
            {
                if (feriadoDto.DataFeriado != existeFeriadoPorId.DataFeriado)
                {
                    var existeFeriadoPorData = feriadoRepository.Existe(new FeriadoDto() { DataFeriado = feriadoDto.DataFeriado });

                    if (existeFeriadoPorData)
                        throw new BusinessException($"Feriado ({feriadoDto.DataFeriado}) já está cadastrado");
                }
            }

            feriadoRepository.Alterar(feriadoDto);
        }

        public void AlterarStatus(FeriadoDto feriadoDto)
        {
            if (feriadoDto == null)
                throw new BusinessException("FeriadoDto é nulo");

            if (feriadoDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!feriadoDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeFeriado = feriadoRepository.Existe(new FeriadoDto() { Id = feriadoDto.Id });

            if (!existeFeriado)
                throw new BusinessException($"Feriado ({feriadoDto.Id}) não está cadastrado");

            if (feriadoDto.Ativo.HasValue)
            {
                if (!feriadoDto.Ativo.Value)
                {
                    if (feriadoDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (feriadoDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            feriadoRepository.Alterar(feriadoDto);
        }
    }
}
