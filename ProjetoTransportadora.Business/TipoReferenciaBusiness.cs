using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoTransportadora.Business
{
    public class TipoReferenciaBusiness
    {
        TipoReferenciaRepository tipoReferenciaRepository;
        public TipoReferenciaBusiness()
        {
            tipoReferenciaRepository = new TipoReferenciaRepository();
        }

        public List<TipoReferenciaDto> Listar(TipoReferenciaDto tipoReferenciaDto)
        {
            return tipoReferenciaRepository.Listar(tipoReferenciaDto);
        }

        public int Incluir(TipoReferenciaDto tipoReferenciaDto)
        {
            var idTipoReferencia = 0;

            if (tipoReferenciaDto == null)
                throw new BusinessException("TipoReferenciaDto é nulo");

            if (string.IsNullOrEmpty(tipoReferenciaDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            var tipoReferenciaExiste = tipoReferenciaRepository.Existe(new TipoReferenciaDto() { Nome = tipoReferenciaDto.Nome });

            if (tipoReferenciaExiste)
                throw new BusinessException($"Tipo de Referência ({tipoReferenciaDto.Nome}) já está cadastrado");

            idTipoReferencia = tipoReferenciaRepository.Incluir(tipoReferenciaDto);

            return idTipoReferencia;
        }
    }
}
