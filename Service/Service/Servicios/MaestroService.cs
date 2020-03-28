using Service.Modelo;
using System.Collections.Generic;

namespace Service.Servicios
{
    public class MaestroService : IMaestroService
    {
        private DataBaseContext dbContext;

        public MaestroService(DataBaseContext context)
        {
            dbContext = context;
        }

        public IEnumerable<Maestro> Get()
        {
            return dbContext.Maestros;
        }
    }
}
