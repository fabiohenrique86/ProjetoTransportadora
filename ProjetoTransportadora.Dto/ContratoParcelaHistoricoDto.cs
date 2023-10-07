using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoTransportadora.Dto
{
    public class ContratoParcelaHistoricoDto
    {
        public int Id { get; set; }
        public int IdContratoParcela { get; set; }
        public System.DateTime DataHistorico { get; set; }
        public string Descricao { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public System.DateTime DataCadastro { get; set; }
    }
}
