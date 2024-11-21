using EcosaveAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcosaveAPI.Repositories.Interfaces
{
    public interface IConsumoRepository
    {
        Task<IEnumerable<Consumo>> GetAllAsync();
        Task<Consumo> GetByIdAsync(int id);
        Task AddAsync(Consumo consumo);
        Task UpdateAsync(Consumo consumo);
        Task DeleteAsync(int id);
    }
}