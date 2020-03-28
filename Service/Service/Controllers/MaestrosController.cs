namespace Service.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Service.Modelo;
    using Service.Servicios;
    using System.Collections.Generic;

    [Route("api/Maestros")]
    [ApiController]
    public class MaestrosController : ControllerBase
    {
        public readonly IMaestroService _maestroService;

        public MaestrosController(IMaestroService maestroService)
        {
            _maestroService = maestroService;
        }
        
        [HttpGet]
        public IEnumerable<Maestro> Get()
        {
            return _maestroService.Get();
        }
    }
}