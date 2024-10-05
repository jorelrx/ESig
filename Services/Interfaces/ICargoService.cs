using SalarioWeb.Models;

namespace SalarioWeb.Services.Interfaces;

public interface ICargoService
{
    Task<List<Cargo>> GetAllAsync();
    Task<Cargo?> GetByIdAsync(int id);
    Task AddAsync(Cargo cargo);
    Task UpdateAsync(Cargo cargo);
    Task DeleteAsync(int id);
}
