using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class SituacaoParcelaBusiness
    {
        SituacaoParcelaRepository situacaoParcelaRepository;
        public SituacaoParcelaBusiness()
        {
            situacaoParcelaRepository = new SituacaoParcelaRepository();
        }

        public SituacaoParcelaDto Obter(SituacaoParcelaDto situacaoParcelaDto = null)
        {
            return situacaoParcelaRepository.Obter(situacaoParcelaDto);
        }

        public bool Existe(SituacaoParcelaDto situacaoParcelaDto = null)
        {
            return situacaoParcelaRepository.Existe(situacaoParcelaDto);
        }

        public List<SituacaoParcelaDto> Listar(SituacaoParcelaDto situacaoParcelaDto = null)
        {
            return situacaoParcelaRepository.Listar(situacaoParcelaDto);
        }
    }
}
