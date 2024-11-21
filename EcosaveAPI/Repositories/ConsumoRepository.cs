using EcosaveAPI.Data;
using EcosaveAPI.Models;
using EcosaveAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcosaveAPI.Repositories
{
    public class ConsumoRepository : IConsumoRepository
    {
        private readonly EcosaveContext _context;

        public ConsumoRepository(EcosaveContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Consumo>> GetAllAsync()
        {
            return await _context.Consumos.ToListAsync();
        }

        public async Task<Consumo> GetByIdAsync(int id)
        {
            return await _context.Consumos.FindAsync(id);
        }

        public async Task AddAsync(Consumo consumo)
        {
            await _context.Consumos.AddAsync(consumo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Consumo consumo)
        {
            _context.Entry(consumo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var consumo = await _context.Consumos.FindAsync(id);
            if (consumo != null)
            {
                _context.Consumos.Remove(consumo);
                await _context.SaveChangesAsync();
            }
        }
    }
}