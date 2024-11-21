using EcosaveAPI.Data;
using EcosaveAPI.Models;
using EcosaveAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcosaveAPI.Repositories
{
    public class ComodoRepository : IComodoRepository
    {
        private readonly EcosaveContext _context;

        public ComodoRepository(EcosaveContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comodo>> GetAllAsync()
        {
            return await _context.Comodos.ToListAsync();
        }

        public async Task<Comodo> GetByIdAsync(int id)
        {
            return await _context.Comodos.FindAsync(id);
        }

        public async Task AddAsync(Comodo comodo)
        {
            await _context.Comodos.AddAsync(comodo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comodo comodo)
        {
            _context.Entry(comodo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var comodo = await _context.Comodos.FindAsync(id);
            if (comodo != null)
            {
                _context.Comodos.Remove(comodo);
                await _context.SaveChangesAsync();
            }
        }
    }
}