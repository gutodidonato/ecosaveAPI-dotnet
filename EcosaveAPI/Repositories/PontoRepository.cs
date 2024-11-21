using EcosaveAPI.Data;
using EcosaveAPI.Models;
using EcosaveAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcosaveAPI.Repositories
{
    public class PontoRepository : IPontoRepository
    {
        private readonly EcosaveContext _context;

        public PontoRepository(EcosaveContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ponto>> GetAllAsync()
        {
            return await _context.Pontos.ToListAsync();
        }

        public async Task<Ponto> GetByIdAsync(int id)
        {
            return await _context.Pontos.FindAsync(id);
        }

        public async Task AddAsync(Ponto ponto)
        {
            await _context.Pontos.AddAsync(ponto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ponto ponto)
        {
            _context.Entry(ponto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ponto = await _context.Pontos.FindAsync(id);
            if (ponto != null)
            {
                _context.Pontos.Remove(ponto);
                await _context.SaveChangesAsync();
            }
        }
    }
}