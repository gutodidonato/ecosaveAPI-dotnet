using EcosaveAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcosaveAPI.Repositories.Interfaces
{
    public interface IComodoRepository
    {
        Task<IEnumerable<Comodo>> GetAllAsync();
        Task<Comodo> GetByIdAsync(int id);
        Task AddAsync(Comodo comodo);
        Task UpdateAsync(Comodo comodo);
        Task DeleteAsync(int id);
    }
}