using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class FuncionalidadeBusiness
    {
        FuncionalidadeRepository funcionalidadeRepository;
        public FuncionalidadeBusiness()
        {
            funcionalidadeRepository = new FuncionalidadeRepository();
        }

        public List<FuncionalidadeDto> Listar(FuncionalidadeDto funcionalidadeDto = null)
        {
            return funcionalidadeRepository.Listar(funcionalidadeDto);
        }
    }
}
