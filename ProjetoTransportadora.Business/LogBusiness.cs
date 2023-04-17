using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System.Collections.Generic;

namespace ProjetoTransportadora.Business
{
    public class LogBusiness
    {
        LogRepository LogRepository;
        public LogBusiness()
        {
            LogRepository = new LogRepository();
        }

        public int Incluir(LogDto logDto)
        {
            var idLog = 0;

            if (logDto == null)
                throw new BusinessException("LogDto é nulo");

            if (string.IsNullOrEmpty(logDto.Exception))
                throw new BusinessException("Exception é obrigatório");

            idLog = LogRepository.Incluir(logDto);

            return idLog;
        }
    }
}
