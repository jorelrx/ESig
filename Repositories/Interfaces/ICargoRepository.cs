using SalarioWeb.Models;

namespace SalarioWeb.Repositories.Interfaces;

public interface ICargoRepository
{
    Task<List<Cargo>> GetAllAsync();
    Task<Cargo?> GetByIdAsync(int id);
    Task AddAsync(Cargo cargo);
    Task UpdateAsync(Cargo cargo);
    Task DeleteAsync(int id);
}
