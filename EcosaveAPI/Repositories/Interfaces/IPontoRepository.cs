using EcosaveAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcosaveAPI.Repositories.Interfaces
{
    public interface IPontoRepository
    {
        Task<IEnumerable<Ponto>> GetAllAsync();
        Task<Ponto> GetByIdAsync(int id);
        Task AddAsync(Ponto ponto);
        Task UpdateAsync(Ponto ponto);
        Task DeleteAsync(int id);
    }
}