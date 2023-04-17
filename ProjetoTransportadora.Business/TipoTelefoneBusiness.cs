using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoTransportadora.Business
{
    public class TipoTelefoneBusiness
    {
        TipoTelefoneRepository tipoTelefoneRepository;
        public TipoTelefoneBusiness()
        {
            tipoTelefoneRepository = new TipoTelefoneRepository();
        }

        public List<TipoTelefoneDto> Listar(TipoTelefoneDto tipoTelefoneDto)
        {
            return tipoTelefoneRepository.Listar(tipoTelefoneDto);
        }

        public int Incluir(TipoTelefoneDto tipoTelefoneDto)
        {
            var idTipoTelefone = 0;

            if (tipoTelefoneDto == null)
                throw new BusinessException("TipoTelefoneDto é nulo");

            if (string.IsNullOrEmpty(tipoTelefoneDto.Nome))
                throw new BusinessException("Nome é obrigatório");

            var tipoTelefoneExiste = tipoTelefoneRepository.Existe(new TipoTelefoneDto() { Nome = tipoTelefoneDto.Nome });

            if (tipoTelefoneExiste)
                throw new BusinessException($"Tipo de Telefone ({tipoTelefoneDto.Nome}) já está cadastrado");

            idTipoTelefone = tipoTelefoneRepository.Incluir(tipoTelefoneDto);

            return idTipoTelefone;
        }
    }
}
