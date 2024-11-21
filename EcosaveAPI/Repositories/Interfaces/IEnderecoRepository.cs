using EcosaveAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcosaveAPI.Repositories.Interfaces
{
    public interface IEnderecoRepository
    {
        Task<IEnumerable<Endereco>> GetAllAsync();
        Task<Endereco> GetByIdAsync(int id);
        Task AddAsync(Endereco endereco);
        Task UpdateAsync(Endereco endereco);
        Task DeleteAsync(int id);
    }
}