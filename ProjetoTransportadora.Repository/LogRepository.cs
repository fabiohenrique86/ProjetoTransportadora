using ProjetoTransportadora.Dto;
using System;

namespace ProjetoTransportadora.Repository
{
    public class LogRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public LogRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public int Incluir(LogDto logDto)
        {
            var log = new Log()
            {
                Id = logDto.Id,
                Exception = logDto.Exception,
                DataCadastro = DateTime.UtcNow
            };

            projetoTransportadoraEntities.Log.Add(log);
            projetoTransportadoraEntities.SaveChanges();

            return log.Id;
        }
    }
}
