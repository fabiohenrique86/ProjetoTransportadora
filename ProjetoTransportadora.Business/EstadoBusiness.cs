using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class EstadoBusiness
    {
        EstadoRepository estadoRepository;
        public EstadoBusiness()
        {
            estadoRepository = new EstadoRepository();
        }

        public List<EstadoDto> Listar(EstadoDto estadoDto)
        {
            return estadoRepository.Listar(estadoDto);
        }
    }
}
