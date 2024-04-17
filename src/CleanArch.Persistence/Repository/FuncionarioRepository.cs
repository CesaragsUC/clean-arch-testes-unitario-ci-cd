using CleanArch.Domain.Entities;
using CleanArch.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace CleanArch.Persistence.Repository
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly FuncionarioDbContext _db;

        public FuncionarioRepository(FuncionarioDbContext context)
        {
            _db = context;
        }

        public async Task Create(Funcionario funcionario)
        {
            if (funcionario is not null)
            {
                _db.Funcionarios.Add(funcionario);
                await _db.SaveChangesAsync();
            }
        }

        public async Task Update(Funcionario funcionario)
        {
            if (funcionario is not null)
            {
                _db.Funcionarios.Update(funcionario);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Funcionario funcionario)
        {
            if (funcionario is not null)
            {
                // Desanexa o objeto do contexto para que ele se torne desconectado 
                _db.Entry(funcionario).State = EntityState.Detached;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Funcionario?> GetById(Guid? id)
        {
            if (id != Guid.Empty)
                return await _db.Funcionarios.FindAsync(id);

            return null;
        }

        public async Task<List<Funcionario>> List()
        {
            return await _db.Funcionarios.AsNoTracking().ToListAsync();
        }

        public async Task Remove(Funcionario funcionario)
        {
            if (funcionario is not null)
            {
                _db.Funcionarios.Remove(funcionario);
                await _db.SaveChangesAsync();
            }
        }

    }
}
