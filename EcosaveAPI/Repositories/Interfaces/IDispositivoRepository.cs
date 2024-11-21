using EcosaveAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcosaveAPI.Repositories.Interfaces
{
    public interface IDispositivoRepository
    {
        Task<IEnumerable<Dispositivo>> GetAllAsync();
        Task<Dispositivo> GetByIdAsync(int id);
        Task AddAsync(Dispositivo dispositivo);
        Task UpdateAsync(Dispositivo dispositivo);
        Task DeleteAsync(int id);
    }
}