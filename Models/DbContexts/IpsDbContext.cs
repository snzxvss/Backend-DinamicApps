using Microsoft.EntityFrameworkCore;
using Models.Model;

namespace Models.DbContexts
{
    public class IpsDbContext : DbContext
    {
        public IpsDbContext(DbContextOptions<IpsDbContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Cita> Citas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paciente>().ToTable("Paciente");
            modelBuilder.Entity<Medico>().ToTable("Medico");
            modelBuilder.Entity<Cita>().ToTable("Cita");
        }
    }
}
