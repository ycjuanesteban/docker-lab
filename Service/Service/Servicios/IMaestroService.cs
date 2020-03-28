namespace Service.Servicios
{
    using Service.Modelo;
    using System.Collections.Generic;

    public interface IMaestroService
    {
        IEnumerable<Maestro> Get();
    }
}
