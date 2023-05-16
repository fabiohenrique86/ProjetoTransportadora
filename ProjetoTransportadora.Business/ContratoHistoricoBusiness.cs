using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;

namespace ProjetoTransportadora.Business
{
    public class ContratoHistoricoBusiness
    {
        ContratoHistoricoRepository contratoHistoricoRepository;
        public ContratoHistoricoBusiness()
        {
            contratoHistoricoRepository = new ContratoHistoricoRepository();
        }

        public int Incluir(ContratoHistoricoDto ContratoHistoricoDto)
        {
            var idContratoHistorico = 0;

            if (ContratoHistoricoDto == null)
                throw new BusinessException("ContratoHistoricoDto é nulo");

            if (ContratoHistoricoDto.IdContrato <= 0)
                throw new BusinessException("IdContrato é obrigatório");

            if (ContratoHistoricoDto.DataHistorico == DateTime.MinValue)
                throw new BusinessException("Data Histórico é obrigatório");

            if (string.IsNullOrEmpty(ContratoHistoricoDto.Descricao))
                throw new BusinessException("Descrição Histórico é obrigatório");

            idContratoHistorico = contratoHistoricoRepository.Incluir(ContratoHistoricoDto);

            return idContratoHistorico;
        }

        public void Excluir(int idContrato)
        {
            if (idContrato <= 0)
                throw new BusinessException("idContrato é obrigatório");

            contratoHistoricoRepository.Excluir(idContrato);
        }
    }
}
