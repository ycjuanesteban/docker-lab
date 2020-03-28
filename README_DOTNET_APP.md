## Configurando la aplicación de .Net Core

Después de haber creado la aplicación Web Api de .Net Core y de haberse asegurado que todo funciona desde el inicio, se deben hacer las siguientes configuraciones para que la aplicación funcione correctamente para las pruebas que haremos.

Es necesario instalar el paquete de conexión a la base de datos y luego configurar el contexto y modelo que vamos a utilizar para la conexión.

### 1. Instalación de dependencias.

Instalación del paquete Nuget desde la consola. (Este pase se puede hacer con el ayudante de visual studio para instalar los paquetes)

```
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

### 2. Creación de elementos necesarios

Cuando se finalice la instalación, se crea una carpeta llamada `Modelo` y dentro de esta una clase que va a contener el contexto y el modelo que vamos a utilizar para la prueba
 
`Repository.cs` 

```cs
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DataBaseContext : DbContext
    {
        public DbSet<Maestro> Maestros { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Maestro>().ToTable("TABLA_BASE_DATOS");
        }

    }

    public class Maestro
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string nombre { get; set; }
        public bool activo { get; set; }
    }
```

Finalizado este paso, se procede a crear una interface y una clase que van a servir como el servicio que va a consumir la base de datos y, a futuro, va a contener la lógica de la aplicación

Dentro de una carpeta llamada `Servicios` se crean dos elementos:

`IMaestroService.cs`

```cs
namespace Service.Servicios
{
    using Service.Modelo;
    using System.Collections.Generic;

    public interface IMaestroService
    {
        IEnumerable<Maestro> Get();
    }
}

```

`MaestroService.cs`

```cs
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
```

Y finalmente se va a crear el controlador dentro de la carpeta `Controllers` ya existente:

`MaestrosController.cs`

```cs
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
```

### 3. Modificación de archivos

Cuando ya se tengas todos los archivos creados, a continuación se debe configurar la conexión a la base de datos, para eso se debe modificar el archivo `appsettings.json` agregando la sección `ConnectionStrings` como se ve a continuación:

```json
"ConnectionStrings": {
   "DataBaseContext": "CONNECTION_STRING"
}
```

Una vez finallizado esto, se debe configurar la conexión en el archivo `Startup.cs` dentro del método `ConfigureServices` de la siguiente forma:

```cs
services.AddDbContext<DataBaseContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("DataBaseContext")));
```

Y se configura la inyección de dependencias:

```cs
services.AddTransient<IMaestroService, MaestroService>();
```

Para habilitar `CORS` se agregar la siguiente instrucción:

```cs
 services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", 
                    builder => 
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
```

y en el método `Configure` de la misma clase se agrega la siguiente instrucción al inicio del mismo de la siguiente forma:

```cs 
   //Primera instrucción del método
   app.UseCors("AllowAllOrigin");

   //Las demás instrucciones que vienen por defecto
   ...
```

[<- Volver al inicio](README.md)