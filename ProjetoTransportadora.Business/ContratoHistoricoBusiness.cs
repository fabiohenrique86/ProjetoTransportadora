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

        public int Incluir(ContratoHistoricoDto contratoHistoricoDto)
        {
            var idContratoHistorico = 0;

            if (contratoHistoricoDto == null)
                throw new BusinessException("ContratoHistoricoDto é nulo");

            if (contratoHistoricoDto.IdContrato <= 0)
                throw new BusinessException("IdContrato é obrigatório");

            if (contratoHistoricoDto.DataHistorico == DateTime.MinValue)
                throw new BusinessException("Data Histórico é obrigatório");

            if (string.IsNullOrEmpty(contratoHistoricoDto.Descricao))
                throw new BusinessException("Descrição Histórico é obrigatório");

            idContratoHistorico = contratoHistoricoRepository.Incluir(contratoHistoricoDto);

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
