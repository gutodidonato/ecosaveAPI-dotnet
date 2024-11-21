using EcosaveAPI.Data;
using EcosaveAPI.Models;
using EcosaveAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcosaveAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly EcosaveContext _context; 

        public UsuarioRepository(EcosaveContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Dispositivos)
                .Include(u => u.Enderecos)
                .Include(u => u.Pontos)
                .ToListAsync();
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Dispositivos)
                    .ThenInclude(d => d.Consumos)
                .Include(u => u.Enderecos)
                .Include(u => u.Pontos)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetNumeroDeDispositivosAsync(int usuarioId)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Dispositivos)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            return usuario?.Dispositivos.Count ?? 0;
        }

        public async Task<decimal> ObterGastoMedioAsync(int usuarioId)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Dispositivos)
                    .ThenInclude(d => d.Consumos)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null || usuario.Dispositivos == null)
                return 0;

            return usuario.Dispositivos
                .SelectMany(d => d.Consumos)
                .Sum(c => c.Custo);
        }
    }
}
