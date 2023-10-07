using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;

namespace ProjetoTransportadora.Business
{
    public class ContratoParcelaHistoricoBusiness
    {
        ContratoParcelaHistoricoRepository contratoParcelaHistoricoRepository;
        public ContratoParcelaHistoricoBusiness()
        {
            contratoParcelaHistoricoRepository = new ContratoParcelaHistoricoRepository();
        }

        public int Incluir(ContratoParcelaHistoricoDto contratoParcelaHistoricoDto)
        {
            var idContratoParcelaHistorico = 0;

            if (contratoParcelaHistoricoDto == null)
                throw new BusinessException("ContratoHistoricoDto é nulo");

            if (contratoParcelaHistoricoDto.IdContratoParcela <= 0)
                throw new BusinessException("IdContratoParcela é obrigatório");

            if (contratoParcelaHistoricoDto.DataHistorico == DateTime.MinValue)
                throw new BusinessException("Data Histórico é obrigatório");

            if (string.IsNullOrEmpty(contratoParcelaHistoricoDto.Descricao))
                throw new BusinessException("Descrição Histórico é obrigatório");

            idContratoParcelaHistorico = contratoParcelaHistoricoRepository.Incluir(contratoParcelaHistoricoDto);

            return idContratoParcelaHistorico;
        }

        public void Excluir(int idContratoParcela)
        {
            if (idContratoParcela <= 0)
                throw new BusinessException("idContratoParcela é obrigatório");

            contratoParcelaHistoricoRepository.Excluir(idContratoParcela);
        }
    }
}
