using CleanArch.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CleanArch.API.Test
{
    public class DemoContextSqlite : FuncionarioDbContext
    {
        /*
         * This context is here for to generate migrations.
         *
         *  - Delete Migrations folder if needed then
         *  - Run following command in main, solution directory:
         *
         *          dotnet ef migrations add InitialMigrationSqlite --context DemoContextSqlite --project AspnetCore6ApiTestingDemo.Test
         *
         */

        //empty ctor required for generating migrations from dotnet tool
        public DemoContextSqlite()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ApplicationFactory<Program>.ConnectionString);
        }

        public DemoContextSqlite(DbContextOptions<FuncionarioDbContext> options) : base(options)
        {
        }
    }
}
