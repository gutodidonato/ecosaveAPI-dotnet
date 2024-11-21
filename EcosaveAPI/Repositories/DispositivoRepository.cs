using EcosaveAPI.Data;
using EcosaveAPI.Models;
using EcosaveAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcosaveAPI.Repositories
{
    public class DispositivoRepository : IDispositivoRepository
    {
        private readonly EcosaveContext _context;

        public DispositivoRepository(EcosaveContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dispositivo>> GetAllAsync()
        {
            return await _context.Dispositivos.ToListAsync();
        }

        public async Task<Dispositivo> GetByIdAsync(int id)
        {
            return await _context.Dispositivos.FindAsync(id);
        }

        public async Task AddAsync(Dispositivo dispositivo)
        {
            await _context.Dispositivos.AddAsync(dispositivo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Dispositivo dispositivo)
        {
            _context.Entry(dispositivo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dispositivo = await _context.Dispositivos.FindAsync(id);
            if (dispositivo != null)
            {
                _context.Dispositivos.Remove(dispositivo);
                await _context.SaveChangesAsync();
            }
        }
    }
}