namespace Service.Modelo
{
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
            modelBuilder.Entity<Maestro>().ToTable("adt_ccaa");
        }

    }

    public class Maestro
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string nombre { get; set; }
        public bool activo { get; set; }
    }
}
