using ProjetoTransportadora.Dto;

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
                Exception = logDto.Exception,
                DataCadastro = logDto.DataCadastro
            };

            projetoTransportadoraEntities.Log.Add(log);
            projetoTransportadoraEntities.SaveChanges();

            return log.Id;
        }
    }
}
