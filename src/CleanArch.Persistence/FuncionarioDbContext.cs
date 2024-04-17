using CleanArch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Persistence
{
    public class FuncionarioDbContext : DbContext
    {
        public FuncionarioDbContext(DbContextOptions<FuncionarioDbContext> options):base(options)
        {
                
        }

        public FuncionarioDbContext()
        {
                
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FuncionarioDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Funcionario> Funcionarios { get; set; }    

    }
}
