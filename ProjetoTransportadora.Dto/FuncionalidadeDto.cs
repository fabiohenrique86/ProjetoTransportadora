using System.Collections.Generic;

namespace ProjetoTransportadora.Dto
{
    public class FuncionalidadeDto
    {
        public FuncionalidadeDto()
        {
            this.GrupoFuncionalidadeDto = new List<GrupoFuncionalidadeDto>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public virtual List<GrupoFuncionalidadeDto> GrupoFuncionalidadeDto { get; set; }
    }
}
