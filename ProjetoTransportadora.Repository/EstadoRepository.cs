using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class EstadoRepository
    {
        public List<EstadoDto> Listar(EstadoDto estadoDto)
        {
            var lista = new List<EstadoDto>();

            lista.Add(new EstadoDto() { Uf = "AC", Nome = "Acre" });
            lista.Add(new EstadoDto() { Uf = "AL", Nome = "Alagoas" });
            lista.Add(new EstadoDto() { Uf = "AM", Nome = "Amazonas" });
            lista.Add(new EstadoDto() { Uf = "AP", Nome = "Amapá" });
            lista.Add(new EstadoDto() { Uf = "BA", Nome = "Bahia" });
            lista.Add(new EstadoDto() { Uf = "DF", Nome = "Distrito Federal" });
            lista.Add(new EstadoDto() { Uf = "ES", Nome = "Espírito Santo" });
            lista.Add(new EstadoDto() { Uf = "GO", Nome = "Goiás" });
            lista.Add(new EstadoDto() { Uf = "MA", Nome = "Maranhão" });
            lista.Add(new EstadoDto() { Uf = "MG", Nome = "Minas Gerais" });
            lista.Add(new EstadoDto() { Uf = "MS", Nome = "Mato Grosso do Sul" });
            lista.Add(new EstadoDto() { Uf = "MT", Nome = "Mato Grosso" });
            lista.Add(new EstadoDto() { Uf = "PA", Nome = "Pará" });
            lista.Add(new EstadoDto() { Uf = "PB", Nome = "Paraíba" });
            lista.Add(new EstadoDto() { Uf = "PE", Nome = "Pernambuco" });
            lista.Add(new EstadoDto() { Uf = "PI", Nome = "Piauí" });
            lista.Add(new EstadoDto() { Uf = "PR", Nome = "Paraná" });
            lista.Add(new EstadoDto() { Uf = "RJ", Nome = "Rio de Janeiro" });
            lista.Add(new EstadoDto() { Uf = "RN", Nome = "Rio Grande do Norte" });
            lista.Add(new EstadoDto() { Uf = "RO", Nome = "Rondônia" });
            lista.Add(new EstadoDto() { Uf = "RR", Nome = "Roraima" });
            lista.Add(new EstadoDto() { Uf = "RG", Nome = "Rio Grande do Sul" });
            lista.Add(new EstadoDto() { Uf = "SC", Nome = "Santa Catarina" });
            lista.Add(new EstadoDto() { Uf = "SE", Nome = "Sergipe" });
            lista.Add(new EstadoDto() { Uf = "SP", Nome = "São Paulo" });
            lista.Add(new EstadoDto() { Uf = "TO", Nome = "Tocantis" });

            if (!string.IsNullOrEmpty(estadoDto.Uf))
                lista = lista.Where(x => x.Uf == estadoDto.Uf).ToList();

            return lista;
        }
    }
}
